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
        private IMapper _mapper;


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

        public async Task<ReturnModel> Register(RegisterDTO model)
        {
            //should a person get a token immediately after registering or they will need to login!! 
            var (success, message, Id) = await CreateUserAsync(model, "User");

            if (!success)
                return new ReturnModel(false, "User not created");

            var libraryUser = new LibraryUser { UserId = Id };

            var result = await _libraryUserRepo.AddAsync(libraryUser);

            if (result is null)
            {
                //await deleteUser(model.Email);
                return new ReturnModel(false, "Internal Db error, registration failed");
            }

            return new ReturnModel(true, "Registration successfully");
        }
        protected async Task<(bool success, string message, string userId)> CreateUserAsync(RegisterDTO model, string role)
        {
            var userEntity = _mapper.Map<User>(model);
            userEntity.UserName = model.Email;
            userEntity.IsActive = true;
           
            await _userManager.UpdateAsync(userEntity);
            var createUserResult = await _userManager.CreateAsync(userEntity, model.Password);
            if (!createUserResult.Succeeded) return (false, "Registration failed, User not created!!", null);
            var addRoleResult = await _userManager.AddToRoleAsync(userEntity, role);
            if (!addRoleResult.Succeeded)
            {
               // await deleteUser(model.Email);
                return (false, "Role Add failed", null); //delete created user!
            }
            return (true, "User Created", userEntity.Id);


        }
    }
}
