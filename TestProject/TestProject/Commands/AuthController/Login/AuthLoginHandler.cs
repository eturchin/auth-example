using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TestProject.Data;

namespace TestProject.Commands.AuthController.Login;

public class AuthLoginHandler : IRequestHandler<AuthLoginRequest, AuthLoginResponse>
{
    private readonly DataContext _context;
    private readonly IConfiguration _configuration;

    public AuthLoginHandler(DataContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<AuthLoginResponse> Handle(AuthLoginRequest request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == request.User.Email.ToLower(), cancellationToken);
        if (user == null)
        {
            return new AuthLoginResponse
            {
                Message = "User not found",
                StatusCode = StatusCodes.Status404NotFound,
                Item = ""
            };
        }

        var verifyPassword = VerifyPassword(request.User.Password, user.PasswordHash, user.PasswordSalt);
        if (!verifyPassword)
        {
            return new AuthLoginResponse
            {
                Message = "Incorrect password",
                StatusCode = StatusCodes.Status400BadRequest,
                Item = ""
            };
        }
        
        var model = GenerateToken(user);
        return new AuthLoginResponse
        {
            Message = "You have successfully logged in",
            StatusCode = StatusCodes.Status200OK,
            Item = model
        };
    }
    
    private bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512(passwordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return computedHash.SequenceEqual(passwordHash);
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