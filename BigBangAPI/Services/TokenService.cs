using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BigBangAPI.Services
{
    public class TokenService : ITokenService
    {
        public JwtSecurityToken GenerateAccessToken(
            IEnumerable<Claim> claims,
            IConfiguration configuration)
        {
            var secretKey = configuration["JWT:SecretKey"]
                ?? throw new InvalidOperationException("JWT:SecretKey não encontrada.");

            var issuer = configuration["JWT:ValidIssuer"]
                ?? throw new InvalidOperationException("JWT:ValidIssuer não encontrado.");

            var audience = configuration["JWT:ValidAudience"]
                ?? throw new InvalidOperationException("JWT:ValidAudience não encontrado.");

            var expiration = configuration.GetValue<double>("JWT:TokenValidityInMinutes");

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),

                Expires = DateTime.UtcNow.AddMinutes(expiration),

                Issuer = issuer,

                Audience = audience,

                SigningCredentials = signingCredentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            return tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[128];

            using var rng = RandomNumberGenerator.Create();

            rng.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(
            string token,
            IConfiguration configuration)
        {
            var secretKey = configuration["JWT:SecretKey"]
                ?? throw new InvalidOperationException("JWT:SecretKey não encontrada.");

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = false,

                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(secretKey))
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var principal = tokenHandler.ValidateToken(
                token,
                tokenValidationParameters,
                out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(
                    SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Token inválido.");
            }

            return principal;
        }
    }
}
