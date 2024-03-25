using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace apidenemesi.Security
{
    public class TokenHand
    {
        public static Token CreateToken(IConfiguration configuration)
        {
            Token token = new();

            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF32.GetBytes(configuration["Token:SecurityKey"]));


            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            token.Expiration= DateTime.Now.AddMinutes(Convert.ToInt16(configuration["token:Expiration"]));

            JwtSecurityToken jwtSecurityToken = new(
                issuer: configuration["Token:Issuer"],
                audience: configuration["Token:Audience"],
                expires: token.Expiration,
                notBefore: DateTime.Now,
                signingCredentials: credentials
                ) ;

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            token.AccessToken=tokenHandler.WriteToken(jwtSecurityToken);
            return token;
        }
    }
}
