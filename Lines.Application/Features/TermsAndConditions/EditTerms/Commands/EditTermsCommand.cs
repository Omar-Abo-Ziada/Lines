namespace Lines.Application.Features.TermsAndConditions;

public record EditTermsCommand(
        Guid Id,
        string Header,
        string Content,
        int Order
        ) : IRequest<bool>;
public class EditTermsCommandHandler : RequestHandlerBase<EditTermsCommand, bool>
{
    private readonly IRepository<Domain.Models.TermsAndConditions.TermsAndConditions> _repository;
    public EditTermsCommandHandler(RequestHandlerBaseParameters parameters, IRepository<Domain.Models.TermsAndConditions.TermsAndConditions> repository) : base(parameters)
    {
        _repository = repository;
    }

    public override async Task<bool> Handle(EditTermsCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);

        entity.Content = request.Content;
        entity.Order = request.Order;
        entity.Header = request.Header;

        await _repository.UpdateAsync(entity, cancellationToken);

        _repository.SaveChanges();
        return true;
    }
}
