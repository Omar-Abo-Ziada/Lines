using AutoMapper;

namespace Lines.Application.Features.Examples;

public class CreateExampleResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}

public class CreateExampleResponseMapper : Profile
{
    public CreateExampleResponseMapper()
    {
        CreateMap<CreateExampleResponse, CreateExampleResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));
    }
}

public static class CreateExampleResponseExtension
{
    public static CreateExampleResponse ToResponse(this CreateExampleDto dto)
    {
        return new CreateExampleResponse
        {
            Id = dto.Id,
            Name = dto.Name,
            Description = dto.Description
        };
    }
}