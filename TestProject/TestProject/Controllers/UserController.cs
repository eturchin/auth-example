using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestProject.Commands.UserController.AddRole;
using TestProject.Commands.UserController.Delete;
using TestProject.Commands.UserController.GetById;
using TestProject.Commands.UserController.GetList;
using TestProject.Commands.UserController.Put;
using TestProject.Models;

namespace TestProject.Controllers;

/// <summary>
/// Manages users
/// </summary>
[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    /// <summary>
    /// Get list of users, depending on parameters
    /// </summary>
    /// <param name="filterRequest"></param>
    /// <returns></returns>
    [HttpGet]
    [Authorize(Roles = "super-admin, admin, support")]
    [ProducesResponseType(typeof(UserGetListResponse), 200)]
    public async Task<IActionResult> GetUsers([FromQuery] FilterRequest filterRequest)
    {
        var response = await _mediator.Send(new UserGetListRequest
        {
            Filter = filterRequest
        });
        
        return response.StatusCode != StatusCodes.Status200OK
            ? StatusCode(response.StatusCode, response)
            : Ok(response);
    }
    
    /// <summary>
    /// Get user by id
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpGet("{userId}")]
    [Authorize(Roles = "super-admin, admin, support")]
    [ProducesResponseType(typeof(UserGetByIdResponse), 200)]
    public async Task<IActionResult> GetUserById([FromRoute] Guid userId)
    {
        var response = await _mediator.Send(new UserGetByIdRequest
        {
            Id = userId
        });
        
        return response.StatusCode != StatusCodes.Status200OK
            ? StatusCode(response.StatusCode, response)
            : Ok(response);
    }
    
    /// <summary>
    /// Add role to user
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="roleId"></param>
    /// <returns></returns>
    [HttpPost("{userId}/roles")]
    [Authorize(Roles = "super-admin")]
    [ProducesResponseType(typeof(UserAddRoleResponse), 201)]
    public async Task<IActionResult> AddRoleToUser(Guid userId, [FromBody] Guid roleId)
    {
        var response = await _mediator.Send(new UserAddRoleRequest
        {
            UserId = userId,
            RoleId = roleId
        });
        
        return response.StatusCode != StatusCodes.Status200OK
            ? StatusCode(response.StatusCode, response)
            : Ok(response);
    }
    
    /// <summary>
    /// Update user information
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPut]
    [Authorize]
    [ProducesResponseType(typeof(UserPutResponse), 200)]
    public async Task<IActionResult> UpdateUser([FromBody] UserViewModelSummary model)
    {
        var response = await _mediator.Send(new UserPutRequest
        {
            User = model
        });
        
        return response.StatusCode != StatusCodes.Status200OK
            ? StatusCode(response.StatusCode, response)
            : Ok(response);
    }
    
    /// <summary>
    /// Delete user by id
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpDelete("{userId}")]
    [Authorize(Roles = "super-admin, admin")]
    [ProducesResponseType(typeof(UserDeleteResponse), 200)]
    public async Task<IActionResult> DeleteUser([FromRoute] Guid userId)
    {
        var response = await _mediator.Send(new UserDeleteRequest
        {
            UserId = userId
        });
        
        return response.StatusCode != StatusCodes.Status200OK
            ? StatusCode(response.StatusCode, response)
            : Ok(response);
    }
};