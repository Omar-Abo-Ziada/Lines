namespace Lines.Application.Features.TermsAndConditions;

public record AddTermsCommand(
        TermsType Type,
        string Header,
        string Content,
        int Order
        ) : IRequest<Guid>;
public class AddTermsCommandHandler : RequestHandlerBase<AddTermsCommand, Guid>
{
    private readonly IRepository<Domain.Models.TermsAndConditions.TermsAndConditions> _repository;
    public AddTermsCommandHandler(RequestHandlerBaseParameters parameters, IRepository<Domain.Models.TermsAndConditions.TermsAndConditions> repository) : base(parameters)
    {
        _repository = repository;
    }

    public override async Task<Guid> Handle(AddTermsCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.AddAsync(new Domain.Models.TermsAndConditions.TermsAndConditions
        {
            Content = request.Content,
          Order = request.Order,
          Header = request.Header,
          TermsType = request.Type,

        }, cancellationToken);

        _repository.SaveChanges();
        return entity.Id;
    }
}
