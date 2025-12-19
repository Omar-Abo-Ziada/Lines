namespace Lines.Application.Features.Users.LoginUser.DTOs;

public class LoginDTO
{
    public Guid? UserId { get; set; }
    public string Token { get; set; }
    public string Role { get; set; }

    public LoginDTO(string token, string role, Guid? userId = null)
    {
        Token = token;
        Role = role;
        UserId = userId;
    }
}