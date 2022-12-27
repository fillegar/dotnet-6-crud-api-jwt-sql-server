using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using User.Core.Model;
using User.Core;

namespace User.API.Handler
{
    public class TokenUtilsHandler
    {
        private readonly IConfiguration configuration;

        public TokenUtilsHandler(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public Token createAccessToken(UserInfo user)
        {
            Token tokenInstance = new Token();
            var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, "Subject"),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim(ClaimTypes.Role, user.Role),
                        new Claim("DisplayName", user.FullName),
                        new Claim("Email", user.Email)
                    };

            tokenInstance.Expiration = DateTime.Now.AddMinutes(10);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                claims,
                expires: tokenInstance.Expiration,
                signingCredentials: signIn);

            tokenInstance.AccessToken = new JwtSecurityTokenHandler().WriteToken(token);

            createRefreshToken(tokenInstance);

            return tokenInstance;
        }

        public string ValidateToken(string token)
        {
            if (token == null)
                return "";

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey
        (Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var email = jwtToken.Claims.First(x => x.Type == "Email").Value;

                return email;
            }
            catch
            {
                return string.Empty;
            }
        }

        private void createRefreshToken(Token tokenInstance)
        {
            byte[] number = new byte[32];
            using (RandomNumberGenerator random = RandomNumberGenerator.Create())
            {
                random.GetBytes(number);
                tokenInstance.RefreshToken = Convert.ToBase64String(number);
            }
        }
    }
}
