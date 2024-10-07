using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Lodgify.Authentication.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Lodgify.Payments.Stripe.Server.IntegrationTests.Helpers;

public class AuthHelper
{
    private static readonly SymmetricSecurityKey SecurityKey = new(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString()));

    public static Action<JwtBearerOptions> JwtBearerOptions => (opt) =>
    {
        opt.TokenValidationParameters.IssuerSigningKey = SecurityKey;
        opt.TokenValidationParameters.ValidateIssuer = false;
        opt.TokenValidationParameters.ValidateAudience = false;
    };

    public static string GenerateToken(int accountId)
    {
        var credentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(LodgifyClaims.PropertyOwnerId, accountId.ToString()),
            new Claim(LodgifyClaims.Scope, "lodgify.api"),
            new Claim(LodgifyClaims.Scope, "private.api.access")
        };
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddDays(1),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}