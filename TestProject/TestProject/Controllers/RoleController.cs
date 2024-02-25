using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestProject.Commands.RoleController.GetList;

namespace TestProject.Controllers;

/// <summary>
/// Manages roles
/// </summary>
[ApiController]
[Route("[controller]")]
public class RoleController : ControllerBase
{
    private readonly IMediator _mediator;

    public RoleController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all roles
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet(Name = "GetRoles")]
    [ProducesResponseType(typeof(RoleGetListResponse), 200)]
    public async Task<IActionResult> GetRoles()
    {
        var response = await _mediator.Send(new RoleGetListRequest());
        
        return response.StatusCode != StatusCodes.Status200OK
            ? StatusCode(response.StatusCode, response)
            : Ok(response);
    }
}