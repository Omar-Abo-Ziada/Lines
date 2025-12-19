using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Lines.Application.Interfaces;

public interface IApplicationDBContext
{
    DatabaseFacade Database { get; }
}