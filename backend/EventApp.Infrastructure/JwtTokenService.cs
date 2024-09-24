using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using EventApp.Core.Abstractions;
using EventApp.Core.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace EventApp.Infrastructure;

public class JwtTokenService : IJwtTokenService
{
    private readonly IConfiguration _configuration;
    private readonly IUnitOfWork _unitOfWork;

    public JwtTokenService(IConfiguration configuration, IUnitOfWork unitOfWork)
    {
        _configuration = configuration;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<(string accessToken, string refreshToken)> GenerateToken(Guid userId, string userName,
        string role)
    {
        var jwtSettings = _configuration.GetSection("Jwt");

        var claims = new[]
        {
            new Claim("UserId", userId.ToString()),
            new Claim("UserName", userName),
            new Claim(ClaimTypes.Role, role)
        };
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddMinutes(double.Parse(jwtSettings["ExpiresMinutes"])),
            signingCredentials: creds
        );
        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
        try
        {
            var refreshToken = await _unitOfWork.RefreshTokens.GetByUserId(userId);
            var newRefreshToken = refreshToken.Token;
            if (refreshToken.Expires < DateTime.Now.ToUniversalTime())
            {
                newRefreshToken = await _unitOfWork.RefreshTokens.Update(refreshToken.Id, userId, GenerateRefreshToken(),
                    DateTime.Now.ToUniversalTime().AddDays(double.Parse(jwtSettings["RefreshTokenExpiresDay"])));
                await _unitOfWork.Complete();
            }
            return (accessToken, newRefreshToken);

        }
        catch (RefreshTokenNotFound)
        {
            var refreshToken = GenerateRefreshToken();
            await _unitOfWork.RefreshTokens.Create(userId, refreshToken,
                DateTime.Now.ToUniversalTime().AddDays(double.Parse(jwtSettings["RefreshTokenExpiresDay"])));
            await _unitOfWork.Complete();
            return (accessToken, refreshToken);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
        
    }
    public string GenerateRefreshToken()
    {
        var randomBytes = new byte[64];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
    }
}