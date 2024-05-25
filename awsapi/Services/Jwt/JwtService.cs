using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using awsapi.Entities;

namespace awsapi.Services;
public class JwtService(IOptions<JwtSetEntity> options)
{
    private readonly JwtSetEntity _jwtSetEntity = options.Value;
    public string GenerateToken(string username)
    {
        var Claims = new List<Claim>();
        Claims.Add(new Claim(JwtRegisteredClaimNames.Sub, username));
        var userClaimsIdentity = new ClaimsIdentity(Claims);
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSetEntity.SignKey));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _jwtSetEntity.IssUser,
            IssuedAt = DateTime.Now,
            Subject = userClaimsIdentity,
            Expires = DateTime.Now.AddMinutes(_jwtSetEntity.ExpireMinutes),
            SigningCredentials = signingCredentials
        };

        var tokenHandler = new JwtSecurityTokenHandler
        {
            SetDefaultTimesOnTokenCreation = false
        };

        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        var serializeToken = tokenHandler.WriteToken(securityToken);
        return serializeToken;
    }

}