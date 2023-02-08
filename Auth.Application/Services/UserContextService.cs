using Auth.Application.Interfaces;
using Auth.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Auth.Application.Services;

public class UserContextService : IUserContextService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private User? _currentUser;
    private UserManager<User> _userManager;

    public UserContextService(
        IHttpContextAccessor httpContextAccessor,
        UserManager<User> userManager)
    {
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
    }
    public async Task<User> GetCurrentUser()
    {
        if (_currentUser != null)
            return _currentUser;


        //check while request is made by a background (schedule) task. DO NOT REMOVE
        if (_httpContextAccessor.HttpContext == null)
            _currentUser = await _userManager.FindByNameAsync("alexannder15@hotmail.com");

        else
        {
            var contextUser = _httpContextAccessor.HttpContext.User;
            _currentUser = await _userManager.GetUserAsync(contextUser);
        }


        if (_currentUser != null)
            return _currentUser;

        return _currentUser;
    }
}
