using AutoMapper;
using EmployeeApi.ActionFilters;
using EmployeeApi.Contracts;
using EmployeeApi.DataTransferObject.Models;
using EmployeeApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeApi.Controllers;

[Route("api/authentication")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly ILogger<AuthenticationController> _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;
    private readonly IAuthenticationManager _authManager;
    public AuthenticationController(ILogger<AuthenticationController> logger, IMapper mapper, UserManager<User> userManager, IAuthenticationManager authManager)
    {
        _logger = logger;
        _mapper = mapper;
        _userManager = userManager;
        _authManager = authManager;
    }

    [HttpPost]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistration)
    {
        var user = _mapper.Map<User>(userForRegistration);
        var result = await _userManager.CreateAsync(user, userForRegistration.Password);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.TryAddModelError(error.Code, error.Description);
            }
            return BadRequest(ModelState);
        }
        await _userManager.AddToRolesAsync(user, userForRegistration.Roles);
        return StatusCode(201);
    }

    [HttpPost("login")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDto user)
    {
        if (!await _authManager.ValidateUser(user))
        {
            _logger.LogWarning($"{nameof(Authenticate)}: Authentication failed. Wrong user name or password.");
            return Unauthorized();
        }
        return Ok(new { Token = await _authManager.CreateToken() });
    }
}