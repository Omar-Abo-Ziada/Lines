namespace Lines.Application.Features.TermsAndConditions;

public record DeleteTermsCommand(
        Guid Id
        ) : IRequest<bool>;
public class DeleteTermsCommandHandler : RequestHandlerBase<DeleteTermsCommand, bool>
{
    private readonly IRepository<Domain.Models.TermsAndConditions.TermsAndConditions> _repository;
    public DeleteTermsCommandHandler(RequestHandlerBaseParameters parameters, IRepository<Domain.Models.TermsAndConditions.TermsAndConditions> repository) : base(parameters)
    {
        _repository = repository;
    }

    public override async Task<bool> Handle(DeleteTermsCommand request, CancellationToken cancellationToken)
    {

        await _repository.DeleteAsync(request.Id, cancellationToken);

        _repository.SaveChanges();
        return true;
    }
}
