using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebClinicAPI.Models;
using WebClinicAPI.Models.Users;
using WebClinicAPI.Services;
using WebClinicAPI.Utils;

namespace WebClinicAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            this._userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("Authenticate")]
        public IActionResult Authenticate([FromBody]AppUser userDto)
        {
            var user = _userService.Authenticate(userDto.Email, userDto.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public IActionResult Register([FromBody]Patient patient)
        {
            try
            {
                _userService.Create(patient, patient.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _userService.GetById(id);
            return Ok(user);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateAsync([FromBody]ChangePasswordModel model)
        {
            var user = await _userService.Update(model.Email, model.OldPassword, model.NewPassword);
            return Ok(user);
        }

    }
}