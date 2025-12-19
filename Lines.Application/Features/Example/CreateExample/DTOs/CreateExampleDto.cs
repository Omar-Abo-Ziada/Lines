using AutoMapper;

namespace Lines.Application.Features.Examples;

public class CreateExampleDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public class CreateExampleDtoMapper : Profile
{
    public CreateExampleDtoMapper()
    {
        CreateMap<Domain.Models.Example, CreateExampleDto>()
            .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
            .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
            .ForMember(d => d.Description, o => o.MapFrom(s => s.Description));
    }
}