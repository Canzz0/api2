using Microsoft.AspNetCore.Mvc;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using apidenemesi.Models;
using apidenemesi.Services;
namespace apidenemesi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly IConfiguration _configuration;

        public LoginController(UserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        // HTTP POST isteği ile gelen login işlemini işleyen metot.
        // 'apidenemesi.Models.Login' tipinde bir model alır.
        [HttpPost("login")]
        public ActionResult Login([FromBody] apidenemesi.Models.Login model)
        {
            // Kullanıcı adı ve şifre kullanılarak kullanıcı doğrulanır.
            var user = _userService.Authenticate(model.username, model.password);

            // Eğer kullanıcı null dönerse, yetkilendirme başarısızdır ve 401 Unauthorized hatası döner.
            if (user == null)
                return Unauthorized();

            // Kullanıcı başarıyla doğrulanırsa, kullanıcı için bir JWT oluşturulur.
            var token = GenerateJwtToken(user);
            // Oluşturulan token, HTTP 200 OK yanıtının bir parçası olarak döner.
            return Ok(new {token , username = user.username}); ;
        }

        // Kullanıcı nesnesi kullanılarak bir JWT oluşturmak için yardımcı metot.
        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            // Yapılandırma dosyasından alınan güvenlik anahtarını kullanarak bir anahtar oluşturur.
            var key = Encoding.UTF32.GetBytes(_configuration["Token:SecurityKey"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                // Token sahibinin id'sini içeren bir claim oluşturur.
                Subject = new ClaimsIdentity(new Claim[]
                {
            new Claim(ClaimTypes.Name, user.username),

                }),
                // Token'in geçerlilik süresini 1 saat olarak ayarlar.
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _configuration["Token:Issuer"],
                Audience = _configuration["Token:Audience"],
                // HMAC SHA256 algoritması kullanarak imza mekanizmasını tanımlar.
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };
            // TokenDescriptor kullanılarak bir token oluşturulur.
            var token = tokenHandler.CreateToken(tokenDescriptor);
            // Oluşturulan token dize formatında döndürülür.
            return tokenHandler.WriteToken(token);
        }
    }

}
