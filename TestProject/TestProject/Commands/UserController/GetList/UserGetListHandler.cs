using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TestProject.Data;
using TestProject.Models;

namespace TestProject.Commands.UserController.GetList;

public class UserGetListHandler : IRequestHandler<UserGetListRequest, UserGetListResponse>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    
    public UserGetListHandler(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<UserGetListResponse> Handle(UserGetListRequest request, CancellationToken cancellationToken)
    {
        IQueryable<User> query = _context.Users.Include(u => u.UserRoles).ThenInclude(ur => ur.Role);
        if (!await query.AnyAsync(cancellationToken))
        {
            return new UserGetListResponse
            {
                Message = "Users not found",
                StatusCode = StatusCodes.Status404NotFound,
                Count = 0,
                Page = 0,
                Total = 0,
                Elements = new List<UserViewModelSummary>()
            };
        }
        
        if (!string.IsNullOrEmpty(request.Filter.FilterByName))
        {
            query = query.Where(u =>
                u.Name.Contains(request.Filter.FilterByName));
        }
        if (!string.IsNullOrEmpty(request.Filter.FilterByEmail))
        {
            query = query.Where(u =>
                u.Email.Contains(request.Filter.FilterByEmail));
        }
        if (!string.IsNullOrEmpty(request.Filter.FilterByAge))
        {
            query = query.Where(u =>
                u.Age.ToString().Contains(request.Filter.FilterByEmail));
        }
        
        query = request.Filter.SortOrder switch
        {
            "asc" => query.OrderByProperty(request.Filter.SortBy),
            "desc" => query.OrderByPropertyDescending(request.Filter.SortBy),
            _ => query.OrderByProperty(request.Filter.SortBy)
        };

        var totalCount = query.Count();
        var totalPages = (int)Math.Ceiling((double)totalCount / request.Filter.PageSize);

        if (totalPages == 0)
        {
            return new UserGetListResponse
            {
                Message = "Pages not found",
                StatusCode = StatusCodes.Status404NotFound,
                Count = 0,
                Page = 0,
                Total = 0,
                Elements = new List<UserViewModelSummary>()
            };
        }

        var currentPage = Math.Clamp(request.Filter.Page, 1, totalPages);
        var users = await query
            .Skip((currentPage - 1) * request.Filter.PageSize)
            .Take(request.Filter.PageSize)
            .ToListAsync(cancellationToken);

        if (users.Count == 0)
        {
            return new UserGetListResponse
            {
                Message = "Users not found",
                StatusCode = StatusCodes.Status404NotFound,
                Count = 0,
                Page = 0,
                Total = 0,
                Elements = new List<UserViewModelSummary>()
            };
        }

        var model = _mapper.Map<List<UserViewModelSummary>>(users);
        return new UserGetListResponse
        {
            Message = "Users have been successfully received",
            StatusCode = StatusCodes.Status200OK,
            Count = totalCount,
            Page = currentPage,
            Total = totalPages,
            Elements = model
        };
    }
}