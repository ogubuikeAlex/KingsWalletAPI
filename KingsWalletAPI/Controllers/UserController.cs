using System;
using System.Text;
using System.Threading.Tasks;
using KingsWalletAPI.Data.Interfaces;
using KingsWalletAPI.Model.DataTransferObjects.UserControllerDTO;
using KingsWalletAPI.Model.Entites;
using KingsWalletAPI.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace KingsWalletAPI.Controllers
{
    [Route("api/User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IServiceFactory _serviceFactory;
        private readonly IUserService _userService;      

        public UserController(IServiceFactory serviceFactory, IUserService userService)
        {
            _serviceFactory = serviceFactory;
            _userService = userService;           
        }

        [HttpPost(Name = "Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO user)
        {
            var _authManager = _serviceFactory.GetServices<IAuthentication>();
            var _userManager = _serviceFactory.GetServices<UserManager<User>>();

            var loggedInUser = await _userManager.FindByEmailAsync(user.Email);
            if (loggedInUser is null)
                return NotFound("User Not found, please Register to continue");

            if (!await _authManager.ValidateUser(user))
                return Unauthorized($"{nameof(Login)}: Authentication failed. Wrong user name or password.");

            HttpContext.Session.Set("Email", Encoding.ASCII.GetBytes(loggedInUser?.Email));
            return Ok(new { Token = await _authManager.CreateToken() });
        }

        [HttpPost("register", Name = "Register")]
        public async Task<IActionResult> Register(RegisterDTO model)
        {
            if (model is null) 
                return BadRequest("RegisterDTO sent from client is null");
             
            if (!ModelState.IsValid) 
                return UnprocessableEntity("The model for registration is not valid");
            
            var result = await _userService.Register(model);

            var entityToReturn = result.Object as UserReturnDTO;

            if (!result.Success)
                return BadRequest(result.Message);

            return CreatedAtRoute("GetUser", new { id = entityToReturn.Id }, entityToReturn);
        }

        [HttpPut(Name = "DeactivateUser")]
        public async Task<IActionResult> DeactivateUser(DeactivateDTO model)
        {
            if (model is null)
                return BadRequest("RegisterDTO sent from client is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity("The model for registration is not valid");

            var result = await _userService.DeactivateUser(model.UserId);

            if (!result.Success)
                return BadRequest(result.Message);

            return NoContent();
        }
    }
}
