using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Api.Domain;

using Microsoft.IdentityModel.Tokens;

namespace Api.Application;

public class TokenHandler : ITokenHandler
{
    public string GenerateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(JwtConstants.SecrectKey);
        var tokenDescriptor = CreateTokenDecriptor(user, key);
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    private SecurityTokenDescriptor CreateTokenDecriptor(User user, byte[]? key)
    {
        var tokenDecriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
                [
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                ]
            ),
            Expires = DateTime.UtcNow.AddDays(JwtConstants.ExpirationDays),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            )
        };

        return tokenDecriptor;
    }
}