
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace contactgroupAPIefMySQL
{
    public static class MyPublicClass
    {
        private static string secureKey = "" +System.Environment.GetEnvironmentVariable("SecredKey");

        public static Boolean VerifyPassword(String text, String hash) {
            if (BCrypt.Net.BCrypt.Verify(text, hash))
                return true;
            return false;
        }
        public static string EncryptPassword(String pwd) {
            return BCrypt.Net.BCrypt.HashPassword(pwd);
        }
        public static string GenerateJwt(int id) {
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secureKey));
            var cred = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha512Signature);
            var header = new JwtHeader(cred);

            var payload = new JwtPayload(id.ToString(), null, null, null, DateTime.Today.AddDays(7));
            var securityToken = new JwtSecurityToken(header, payload);
            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }
        public static JwtSecurityToken VerifyJwt(string jwt)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secureKey);
            tokenHandler.ValidateToken(jwt, new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false
            }, out SecurityToken validatedToken);

            return (JwtSecurityToken) validatedToken;
        }
    }
}
