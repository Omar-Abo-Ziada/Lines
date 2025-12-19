using Lines.Application.Common;
using Lines.Application.Features.RewardOffers.DTOs;
using Lines.Application.Interfaces;
using Lines.Domain.Enums;
using Lines.Domain.Models.Drivers;
using Lines.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace Lines.Application.Features.RewardOffers.ActivateOffer.Commands;

public record ActivateOfferCommand(Guid DriverId, Guid OfferId) : IRequest<Result<ActivateOfferDto>>;

public class ActivateOfferCommandHandler : RequestHandlerBase<ActivateOfferCommand, Result<ActivateOfferDto>>
{
    private readonly IRepository<DriverServiceFeeOffer> _offerRepository;
    private readonly IRepository<DriverOfferActivation> _activationRepository;
    private readonly IRepository<Wallet> _walletRepository;
    private readonly IRepository<WalletTransaction> _transactionRepository;
    private readonly IApplicationUserService _appUserService;

    public ActivateOfferCommandHandler(
        RequestHandlerBaseParameters parameters,
        IRepository<DriverServiceFeeOffer> offerRepository,
        IRepository<DriverOfferActivation> activationRepository,
        IRepository<Wallet> walletRepository,
        IRepository<WalletTransaction> transactionRepository,
        IApplicationUserService appUserService)
        : base(parameters)
    {
        _offerRepository = offerRepository;
        _activationRepository = activationRepository;
        _walletRepository = walletRepository;
        _transactionRepository = transactionRepository;
        _appUserService = appUserService;
    }

    public override async Task<Result<ActivateOfferDto>> Handle(
        ActivateOfferCommand request,
        CancellationToken cancellationToken)
    {
        // 1. Validate offer exists and is active
        var offer = await _offerRepository.GetByIdAsync(request.OfferId, cancellationToken);
        
        if (offer == null || offer.IsDeleted)
        {
            return Result<ActivateOfferDto>.Failure(new Error(
                Code: "OFFER:NOTFOUND",
                Description: "The specified offer does not exist.",
                Type: ErrorType.NotFound));
        }

        if (!offer.IsAvailableForPurchase())
        {
            return Result<ActivateOfferDto>.Failure(new Error(
                Code: "OFFER:NOTACTIVE",
                Description: "This offer is not currently available for activation.",
                Type: ErrorType.Validation));
        }

        // 2. Check for overlapping active offers in the same period
        // Calculate the new offer's activation period
        var activationDate = DateTime.UtcNow;
        var expirationDate = activationDate.AddDays(offer.DurationDays);

        // Check if any existing active offer overlaps with the new offer's period
        // Two periods overlap if: existingStart < newEnd AND existingEnd > newStart
        var overlappingOffer = await _activationRepository
            .Get(a => a.DriverId == request.DriverId 
                   && a.IsActive 
                   && a.ActivationDate < expirationDate  // Existing starts before new ends
                   && a.ExpirationDate > activationDate) // Existing ends after new starts
            .Include(a => a.Offer)
            .FirstOrDefaultAsync(cancellationToken);

        if (overlappingOffer != null)
        {
            var daysRemaining = (int)(overlappingOffer.ExpirationDate - DateTime.UtcNow).TotalDays;
            return Result<ActivateOfferDto>.Failure(new Error(
                Code: "OFFER:ALREADYACTIVE",
                Description: $"You already have an active offer '{overlappingOffer.Offer?.Title}' " +
                            $"that expires on {overlappingOffer.ExpirationDate:yyyy-MM-dd} " +
                            $"({daysRemaining} days remaining). " +
                            "Please wait until it expires before activating a new one.",
                Type: ErrorType.Validation));
        }

        var userDriverId = await _appUserService.GetUserIdByDriverIdAsync(request.DriverId);

        if (userDriverId == null)
        {
            return Result<ActivateOfferDto>.Failure(
                Error.Create("User.NotFound", "No user found for this driver.")
            );
        }


        // 3. Get or create wallet
        var wallet = await _walletRepository
            .Get(w => w.UserId == userDriverId.Value)
            .FirstOrDefaultAsync(cancellationToken);

        if (wallet == null)
        {
            wallet = new Wallet(userDriverId.Value);
            await _walletRepository.AddAsync(wallet, cancellationToken);
            await _walletRepository.SaveChangesAsync(cancellationToken);
        }

        // 4. Check sufficient balance
        if (!wallet.HasSufficientBalance(offer.Price))
        {
            return Result<ActivateOfferDto>.Failure(new Error(
                Code: "WALLET:INSUFFICIENTBALANCE",
                Description: $"Insufficient wallet balance. Required: {offer.Price:C}, Available: {wallet.Balance:C}",
                Type: ErrorType.Validation));
        }

        // 5. Deduct from wallet
        wallet.Debit(offer.Price);
        await _walletRepository.UpdateAsync(wallet, cancellationToken);

        // 6. Create wallet transaction
        var paymentReference = $"OFFER-{request.OfferId}-{Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper()}";
        var transaction = new WalletTransaction(
            walletId: wallet.Id,
            amount: offer.Price,
            type: TransactionType.Debit,
            transactionCategory: WalletTransactionCategory.OfferPurchase,
            reference: paymentReference,
            description: $"Purchase of offer: {offer.Title}"
        );

        await _transactionRepository.AddAsync(transaction, cancellationToken);

        // 7. Create activation (using dates calculated earlier for overlap check)
        var activation = new DriverOfferActivation(
            driverId: request.DriverId,
            offerId: request.OfferId,
            activationDate: activationDate,
            expirationDate: expirationDate,
            paymentReference: paymentReference
        );

        await _activationRepository.AddAsync(activation, cancellationToken);
        await _activationRepository.SaveChangesAsync(cancellationToken);

        // 8. Return success response
        var response = new ActivateOfferDto
        {
            ActivationId = activation.Id,
            OfferId = offer.Id,
            OfferTitle = offer.Title,
            ActivationDate = activationDate,
            ExpirationDate = expirationDate,
            AmountPaid = offer.Price,
            NewWalletBalance = wallet.Balance,
            PaymentReference = paymentReference
        };

        return Result<ActivateOfferDto>.Success(response);
    }
}

