using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TestProject.Data;

namespace TestProject.Commands.AuthController.Register;

public class AuthRegisterHandler : IRequestHandler<AuthRegisterRequest, AuthRegisterResponse>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    public AuthRegisterHandler(DataContext context, IMapper mapper, IConfiguration configuration)
    {
        _context = context;
        _mapper = mapper;
        _configuration = configuration;
    }

    public async Task<AuthRegisterResponse> Handle(AuthRegisterRequest request, CancellationToken cancellationToken)
    {
        var existingUser =
            await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == request.User.Email.ToLower(), cancellationToken);
        if (existingUser != null)
        {
            return new AuthRegisterResponse
            {
                Message = "User with this email already exist",
                StatusCode = StatusCodes.Status400BadRequest,
                Item = existingUser.Id.ToString()
            };
        }
        
        CreatePasswordHash(request.User.Password, out var passwordHash, out var passwordSalt);
        
        var user = _mapper.Map<User>(request.User);
        user.Id = Guid.NewGuid();
        user.PasswordSalt = passwordSalt;
        user.PasswordHash = passwordHash;
        user.CreationDate = DateTime.UtcNow;
        user.LastModified = DateTime.UtcNow;
        
        foreach (var roleId in request.User.RoleIds)
        {
            var role = await _context.Roles.SingleOrDefaultAsync(x => x.Id == roleId, cancellationToken);
            if (role != null)
            {
                user.UserRoles.Add(new UserRole
                {
                    UserId = user.Id,
                    RoleId = roleId
                });
            }
        }
        
        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);
        
        var model = GenerateToken(user);
        return new AuthRegisterResponse
        {
            Message = "User have been successfully registered",
            StatusCode = StatusCodes.Status201Created,
            Item = model
        };
    }
    
    private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512();
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }
    
    private string GenerateToken(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email)
        };
            
        var userRoles = _context.UserRoles
            .Include(ur => ur.Role)
            .Where(ur => ur.UserId == user.Id)
            .Select(ur => ur.Role.Name)
            .ToList();

        claims.AddRange(userRoles.Select(roleName => new Claim(ClaimTypes.Role, roleName)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(14),
            SigningCredentials = credentials,
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}