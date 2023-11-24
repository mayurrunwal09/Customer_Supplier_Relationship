using Domain.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace WEBAPI.Middleware.Auth
{
    public class JWTAuthManager : IJWTAuthManager
    {
        private readonly IConfiguration _configuration;

        public JWTAuthManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateJWT(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
              _configuration["Jwt:Issuer"],
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token); //return access token       
        }
    }
    /* public class JWTAuthManager : IJWTAuthManager
     {
         private readonly IConfiguration _configuration;
         public JWTAuthManager(IConfiguration configuration)
         {
             _configuration = configuration;
         }
         public string GenerateJWT(User user)
         {
             var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
             var credential = new SigningCredentials(securitykey,SecurityAlgorithms.HmacSha256);
             var token = new JwtSecurityToken(
                 _configuration["Jwt:Issuer"],
                 _configuration["Jwt:Issuer"],
                 expires: DateTime.Now.AddMinutes(120),
                 signingCredentials: credential);
             return new JwtSecurityTokenHandler().WriteToken(token);

         }
     }*/
}
