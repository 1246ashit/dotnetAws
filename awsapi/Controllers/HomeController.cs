using awsapi.Entities;
using awsapi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BCrypt.Net;

namespace awsapi.Controllers
{
    [Route("api/[controller]")]
    public class HomeController : Controller
    {
        readonly private IUserService _userService;
        private readonly JwtService _jwtService;
      
        public HomeController(IUserService userService, JwtService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }

        [HttpPost("Test")]
        public async Task<ActionResult> Test()
        {
            return Ok("hi");
        }
        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody] LoginEntity loginEntity)
        {
            int id = await _userService.CheckedUser(loginEntity);
            if (id == 0)
            {
                return BadRequest("user isn't exist.");
            }
            else
            {
                var token = _jwtService.GenerateToken(loginEntity.username);
                return Ok(new { id, token });
            }
        }
        [AllowAnonymous]
        [HttpPost("SignUp")]
        public async Task<ActionResult> SignUp([FromBody] UserEntity userEntity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                // 密碼加密
                userEntity.password = BCrypt.Net.BCrypt.HashPassword(userEntity.password);
                int result = await _userService.CreateUser(userEntity);
                ActionResult resultMessage = result switch
                {
                    0 => BadRequest("account is exist."),
                    -1 => BadRequest("register failed."),
                    _ => Ok("register successed.")
                };
                return resultMessage;
            }
        }

    }
}