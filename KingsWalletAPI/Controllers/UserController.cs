using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using KingsWalletAPI.Data.Implementations;
using KingsWalletAPI.Data.Interfaces;
using KingsWalletAPI.Model.DataTransferObjects.UserControllerDTO;
using KingsWalletAPI.Model.Entites;
using KingsWalletAPI.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KingsWalletAPI.Controllers
{
    [Route("api/User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IServiceFactory _serviceFactory;
        private readonly IUserService _userService;
        private readonly UserManager<User> _userManager;
       


        public UserController(IServiceFactory serviceFactory, IUserService userService)
        {
            _serviceFactory = serviceFactory;
            _userService = userService;
            _userManager =  _serviceFactory.GetServices<UserManager<User>>();
        }

        [HttpPost(Name = "Login")]
        public async Task<IActionResult> Authenticate([FromBody] LoginDTO user)
        {
            var _authManager = _serviceFactory.GetServices<IAuthentication>();
           
            var loggedInUser = await _userManager.FindByEmailAsync(user.Email);
            if (loggedInUser is null)
                return NotFound("User Not found, please Register to continue");

            if (!await _authManager.ValidateUser(user))
                return Unauthorized($"{nameof(Authenticate)}: Authentication failed. Wrong user name or password.");

            HttpContext.Session.Set("Email", Encoding.ASCII.GetBytes(loggedInUser?.Email));
            return Ok(new { Token = await _authManager.CreateToken() });
        }

        
    }
}
