using Auth.Domain.Models;

namespace Auth.Application.Interfaces;

public interface IUserContextService
{
    Task<User> GetCurrentUser();
}
