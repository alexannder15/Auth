using Auth.Application.Exceptions;
using Auth.Application.Interfaces;
using Auth.Domain.Dtos;
using Auth.Domain.Models;
using Microsoft.AspNetCore.Identity;
using System.Text.Json;

namespace Auth.Application.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly IJwtService _jwtService;

    public AuthService(
        UserManager<User> userManager,
        IJwtService jwtService)
    {
        _userManager = userManager;
        _jwtService = jwtService;
    }

    public async Task<string> RegisterAsync(CreateUserDto createUser)
    {
        var userByEmail = await _userManager.FindByEmailAsync(createUser.Email);

        if (userByEmail != null)
            throw new EmailAlreadyExistException("Email already exist");

        var user = new User
        {
            Email = createUser.Email,
            UserName = createUser.Email,
            FirstName = createUser.FirstName,
            LastName = createUser.LastName,
        };

        var isCreated = await _userManager.CreateAsync(user, createUser.Password);

        if (isCreated.Succeeded)
            return _jwtService.GenerateJwtToken(user);

        throw new UnhandledException($"Something was wrong with Register!:  {JsonSerializer.Serialize(isCreated.Errors)}");
    }

    public async Task<string> LoginAsync(LoginUserDto loginUser)
    {
        User? user = await _userManager.FindByEmailAsync(loginUser.Email);

        if(user == null)
            throw new EmailNotFoundException("User doesn't exist");

        bool isCorrect = await _userManager.CheckPasswordAsync(user, loginUser.Password);

        if (!isCorrect)
            throw new InvalidCredentialsException("Invalid credentials");

        return _jwtService.GenerateJwtToken(user);
    }
}
