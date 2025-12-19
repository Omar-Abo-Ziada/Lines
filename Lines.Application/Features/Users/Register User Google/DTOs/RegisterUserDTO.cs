namespace Lines.Application.Features.Users.Register_User.DTOs;
public class RegisterUserDTO
{
    public Guid UserId { get; set; }
    public string Token { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}