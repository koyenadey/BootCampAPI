using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RegisteredUser.Database;
using RegisteredUser.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RegisteredUser.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly UserContext _userContext;
        private readonly IConfiguration _config;
        public LoginController(UserContext context, IConfiguration config) 
        { 
            _userContext = context;
            _config = config;
        }

        [HttpPost]
        public ActionResult Login([FromBody] UserLogin userLogin)
        {
            var user = Authenticate(userLogin);
            if(user != null)
            {
                var token = GenerateToken(user);
                return Ok(token);
            }
            return NotFound("User Does Not Exist");
        }
        private string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            //var claims = new[]
            //{
            //    new Claim(ClaimTypes.NameIdentifier,user.UserName),
            //    new Claim(ClaimTypes.Email,user.Email)
            //};
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: new[]
                {
                    new Claim("name",user.UserName),
                    new Claim("mail",user.Email),
                    new Claim("userId",user.UserId.ToString())
                },
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private User Authenticate(UserLogin user)
        {
            var currentUser = _userContext.Users.SingleOrDefault(u => u.Email.ToLower() == user.Email.ToLower() && u.Password == user.Password);
            
            if (currentUser != null)
            {
                return currentUser;
            }
            
            return null; 
        }

       
    }
}
