namespace Lines.Presentation.Endpoints.Users.UpdateUserEmail
{
    public class UpdateUserEmailResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool EmailVerified { get; set; }
        public string UpdatedFields { get; set; } = string.Empty;
    }
}
