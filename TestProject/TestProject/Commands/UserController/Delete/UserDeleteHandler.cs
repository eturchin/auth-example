using MediatR;
using Microsoft.EntityFrameworkCore;
using TestProject.Data;

namespace TestProject.Commands.UserController.Delete;

public class UserDeleteHandler : IRequestHandler<UserDeleteRequest, UserDeleteResponse>
{
    private readonly DataContext _context;

    public UserDeleteHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<UserDeleteResponse> Handle(UserDeleteRequest request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);
        if (user == null)
        {
            return new UserDeleteResponse
            {
                Message = "User not found",
                StatusCode = StatusCodes.Status404NotFound
            };
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync(cancellationToken);
        
        return new UserDeleteResponse
        {
            Message = "User have been successfully deleted",
            StatusCode = StatusCodes.Status200OK
        };
    }
}