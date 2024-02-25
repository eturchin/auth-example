using MediatR;
using Microsoft.EntityFrameworkCore;
using TestProject.Commands.UserController.GetById;
using TestProject.Data;
using TestProject.Models;

namespace TestProject.Commands.UserController.AddRole;

public class UserAddRoleHandler : IRequestHandler<UserAddRoleRequest, UserAddRoleResponse>
{
    private readonly DataContext _context;
    private readonly IMediator _mediator;
    
    public UserAddRoleHandler(DataContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task<UserAddRoleResponse> Handle(UserAddRoleRequest request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .Include(u => u.UserRoles)
            .SingleOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);
        if (user == null)
        {
            return new UserAddRoleResponse
            {
                Message = "User not found",
                StatusCode = StatusCodes.Status404NotFound,
                Item = new UserViewModelDetails()
            };
        }

        var role = await _context.Roles.SingleOrDefaultAsync(r => r.Id == request.RoleId, cancellationToken);
        if (role == null)
        {
            return new UserAddRoleResponse
            {
                Message = "Role not found",
                StatusCode = StatusCodes.Status404NotFound,
                Item = new UserViewModelDetails()
            };
        }
        
        if (user.UserRoles.Any(ur => ur.RoleId == request.RoleId))
        {
            var response = await _mediator.Send(new UserGetByIdRequest
            {
                Id = user.Id
            }, cancellationToken);
            
            return new UserAddRoleResponse
            {
                Message = "User already has this role",
                StatusCode = StatusCodes.Status400BadRequest,
                Item = response.Item
            };
        }

        var userRole = new UserRole
        {
            UserId = request.UserId, 
            RoleId = request.RoleId
        };
        
        user.UserRoles.Add(userRole);
        await _context.SaveChangesAsync(cancellationToken);
        
        var model = (await _mediator.Send(new UserGetByIdRequest
        {
            Id = user.Id
        }, cancellationToken)).Item;
            
        return new UserAddRoleResponse
        {
            Message = "Role have been successfully added",
            StatusCode = StatusCodes.Status201Created,
            Item = model
        };
    }
}