using Auth.Domain.Models;

namespace Auth.Application.Interfaces;

public interface IJwtService
{
    string GenerateJwtToken(User user);
}
