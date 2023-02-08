using Auth.Domain.Dtos;

namespace Auth.Application.Interfaces;

public interface IAuthService
{
    Task<string> RegisterAsync(CreateUserDto userDto);
    Task<string> LoginAsync(LoginUserDto loginUser);
}
