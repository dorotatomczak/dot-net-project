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
        [HttpPost("RegisterPatient")]
        public IActionResult RegisterPatient([FromBody]Patient patient)
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

        [AllowAnonymous]
        [HttpPost("RegisterPhysician")]
        public IActionResult RegisterPhysician([FromBody]Physician physician)
        {
            try
            {
                _userService.Create(physician, physician.Password);
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

        [HttpPut("UpdatePassword")]
        public async Task<IActionResult> UpdatePassword([FromBody]ChangePasswordModel model)
        {
            var user = await _userService.UpdatePassword(model.Email, model.OldPassword, model.NewPassword);
            return Ok(user);
        }

        [HttpPut("UpdateEmail")]
        public async Task<IActionResult> UpdateEmail([FromBody]ChangeEmailModel model)
        {
            var user = await _userService.UpdateEmail(model.Email, model.NewEmail, model.Password);
            return Ok(user);
        }

    }
}