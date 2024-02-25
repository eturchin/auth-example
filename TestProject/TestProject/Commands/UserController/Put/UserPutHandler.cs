using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TestProject.Data;
using TestProject.Models;

namespace TestProject.Commands.UserController.Put;

public class UserPutHandler : IRequestHandler<UserPutRequest, UserPutResponse>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    
    public UserPutHandler(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<UserPutResponse> Handle(UserPutRequest request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.User.Id, cancellationToken);
        if (user == null)
        {
            return new UserPutResponse
            {
                Message = "User not found",
                StatusCode = StatusCodes.Status404NotFound,
                Item = new UserViewModelDetails()
            };
        }

        user.Name = request.User.Name;
        user.Age = request.User.Age;
        user.Email = request.User.Email;

        _context.Users.Update(user);
        await _context.SaveChangesAsync(cancellationToken);

        var model = _mapper.Map<UserViewModelSummary>(user);
        return new UserPutResponse
        {
            Message = "User have been successfully updated",
            StatusCode = StatusCodes.Status200OK,
            Item = model
        };
    }
}