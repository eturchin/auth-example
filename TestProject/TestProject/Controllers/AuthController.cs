using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using TestProject.Commands.AuthController.Login;
using TestProject.Commands.AuthController.Register;
using TestProject.Models;

namespace TestProject.Controllers;

/// <summary>
/// Registration and login
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController: ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Register user
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost("Register", Name = "RegisterUser")]
    [ProducesResponseType(typeof(AuthRegisterResponse), 201)]
    public async Task<IActionResult> Register([FromBody]UserRegisterModel model)
    {
        if (!ModelState.IsValid)
        {
            Log.Error("Validation is not passed");
            return BadRequest("Check the correctness of the entered fields");
        }

        var response = await _mediator.Send(new AuthRegisterRequest
        {
            User = model
        });

        return response.StatusCode != StatusCodes.Status201Created
            ? StatusCode(response.StatusCode, response)
            : Ok(response);
    }

    /// <summary>
    /// Login
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost("Login", Name = "LoginUser")]
    [ProducesResponseType(typeof(AuthLoginResponse), 200)]
    public async Task<IActionResult> Login([FromBody] UserLoginModel model)
    {
        if (!ModelState.IsValid)
        {
            Log.Error("Validation is not passed");
            return BadRequest("Check the correctness of the entered fields");
        }

        var response = await _mediator.Send(new AuthLoginRequest
        {
            User = model
        });
        
        return response.StatusCode != StatusCodes.Status200OK
            ? StatusCode(response.StatusCode, response)
            : Ok(response);
    }
}