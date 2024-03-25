using apidenemesi.Models;
using apidenemesi.Security;
using apidenemesi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace apidenemesi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly UserService _userservice;

        public UserController(IConfiguration configuration, UserService userService)
        {
            _configuration = configuration;
            _userservice = userService;
        }

        [HttpGet("api/Users")] // Tüm kullanıcıları getirir
        public ActionResult<IEnumerable<User>> GetAllUser()
        {
            var users = _userservice.GetAllUsers();
            return Ok(users);

        }


        [HttpGet("api/User/{id}")]
        [Authorize] // Yalnızca doğrulanmış (yetkilendirilmiş) kullanıcılar bu işlemi yapabilir
        public ActionResult GetUserById(int id)
        {
            var user = _userservice.GetUserById(id);

            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);

        }
        [HttpPost("api/User/add")]
        [Authorize]
        public ActionResult<User> AddUser(User user)
        {
            if (_userservice.IsUsernameTaken(user.username))
            {
                return Conflict("Bu kullanıcı adı zaten alınmış.");
            }
             Token token=TokenHand.CreateToken(_configuration);
            _userservice.AddUser(user);
            return Ok(token);
        }

        [HttpPost("api/User/update")]
        [Authorize]
        public ActionResult UpdateUser(User user,int id)
        {
            var Upuser = _userservice.GetUserById(id);
            if (Upuser== null)
            {
                return NotFound();
            }
            _userservice.UpdateUser(id, user);
            return Ok(user);
        }

        [HttpPost("api/User/delete")]
        [Authorize]
        public ActionResult DeleteUser(int id)
        {
           var Remuser= _userservice.GetUserById(id);
            if(Remuser == null)
            {
                return NotFound();
            }
            _userservice.DeleteUser(id);
            return Ok();
        }
    }

}
