using Auth.Application.Interfaces;
using Auth.Domain.Dtos;
using Auth.Domain.Dtos.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Auth.WebApi.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _userService;

    public AuthController(
        IAuthService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(CreateUserDto userDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        string token = await _userService.RegisterAsync(userDto);

        var response = new Response<string>
        {
            Code = "200.02.001",
            Mesage = "User created",
            Data = token
        };

        return Ok(response);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserDto loginUser)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        string token = await _userService.LoginAsync(loginUser);

        var response = new Response<string>
        {
            Code = "200.02.002",
            Mesage = "User logged in successfully",
            Data = token
        };

        return Ok(response);
    }

}