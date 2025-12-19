namespace AdminLine.Service.IService;

public interface IRegistrationService
{
    Task<int> GetPendingRegistrationsCountAsync();
}

