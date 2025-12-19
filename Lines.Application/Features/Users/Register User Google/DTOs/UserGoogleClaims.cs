namespace Lines.Application.Features.Users.Register_User_Google.DTOs;
public record UserGoogleClaims(string FirstName, string LastName, string Email, string googleProviderKey);