namespace Lines.Application.Interfaces
{
    public interface IUserStateService
    {
        /// <summary>
        /// Gets the current user ID.
        /// </summary>
        Guid UserId { get; }
        /// <summary>
        /// Gets the current user name.
        /// </summary>
        string? UserName { get; }
    }
}