namespace AdminLine.Service.IService;

public interface IDriverService
{
    Task<int> GetAllDriversCountAsync();
    Task<int> GetOnlineDriversCountAsync();
    Task<int> GetOnTripDriversCountAsync();
}

