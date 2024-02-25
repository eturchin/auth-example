using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TestProject.Data;
using TestProject.Models;

namespace TestProject.Commands.RoleController.GetList;

public class RoleGetListHandler : IRequestHandler<RoleGetListRequest, RoleGetListResponse>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public RoleGetListHandler(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<RoleGetListResponse> Handle(RoleGetListRequest request, CancellationToken cancellationToken)
    {
        var roles = await _context.Roles.ToListAsync(cancellationToken);
        if (!roles.Any())
        {
            return new RoleGetListResponse
            {
                Message = "Roles not found",
                StatusCode = StatusCodes.Status404NotFound,
                Elements = new List<RoleViewModelSummary>()
            };
        }

        var model = _mapper.Map<List<RoleViewModelSummary>>(roles);
        return new RoleGetListResponse
        {
            Message = "Roles have been successfully received",
            StatusCode = StatusCodes.Status200OK,
            Elements = model
        };
    }
}