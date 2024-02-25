using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TestProject.Data;
using TestProject.Models;

namespace TestProject.Commands.UserController.GetById;

public class UserGetByIdHandler : IRequestHandler<UserGetByIdRequest, UserGetByIdResponse>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public UserGetByIdHandler(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<UserGetByIdResponse> Handle(UserGetByIdRequest request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .SingleOrDefaultAsync(u => u.Id == request.Id, cancellationToken);
        if (user == null)
        {
            return new UserGetByIdResponse
            {
                Message = "User not found",
                StatusCode = StatusCodes.Status404NotFound,
                Item = new UserViewModelDetails()
            };
        }
        
        var model = _mapper.Map<UserViewModelDetails>(user);
        var roles = _context.UserRoles
            .Where(x => x.UserId == user.Id)
            .Select(ur => ur.Role)
            .Distinct()
            .ToList();
        
        model.Roles = _mapper.Map<List<RoleViewModelSummary>>(roles);
        return new UserGetByIdResponse
        {
            Message = "User have been successfully received",
            StatusCode = StatusCodes.Status200OK,
            Item = model
        };
    }
}