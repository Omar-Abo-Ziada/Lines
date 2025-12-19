using Lines.Application.Common;
using Lines.Application.Interfaces;
using Lines.Domain.Models.Drivers;
using MediatR;

namespace Lines.Application.Features.Offers.DeleteOfferById.Commands
{
    public record DeleteOfferByIdCommand(Guid OfferId) : IRequest<bool>;

    public class DeleteOfferByIdCommandHandler(
        RequestHandlerBaseParameters parameters,
        IRepository<Offer> offerRepository)
        : RequestHandlerBase<DeleteOfferByIdCommand, bool>(parameters)
    {
        private readonly IRepository<Offer> _offerRepository = offerRepository;

        public override async Task<bool> Handle(DeleteOfferByIdCommand request, CancellationToken cancellationToken)
        {
            await _offerRepository.DeleteAsync(request.OfferId, cancellationToken)
                                  .ConfigureAwait(false);

            return true;
        }
    }
}
