using System.Threading.Tasks;
using AutoMapper;
using KingsWalletAPI.Data.Implementations;
using KingsWalletAPI.Data.Interfaces;
using KingsWalletAPI.Model.DataTransferObjects.UserControllerDTO;
using KingsWalletAPI.Model.Entites;
using KingsWalletAPI.Service.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace KingsWalletAPI.Service.Implementations
{
    public class UserService : IUserService
    {
        private IMapper _mapper;
       // private readonly IRepository<User> _userRepo;
        private readonly UserManager<User> _userManager;

        public UserService(IMapper mapper, IRepository<User> userRepo, UserManager<User> userManager)
        {
            _mapper = mapper;
            //_userRepo = userRepo;
            _userManager = userManager;
        }
        public void DeactivateUser()
        {
            throw new System.NotImplementedException();
        }

        public void Login()
        {
            throw new System.NotImplementedException();
        }


        public async Task<ReturnModel> Register(RegisterDTO model)
        {
            //should a person get a token immediately after registering or they will need to login!! 
            var (success, message, Id) = await CreateUserAsync(model, "User");

            if (!success)
                return new ReturnModel(false, "User not created");

            var wallet = new Wallet { WalletId = RandomGenerator.GenerateWalletId() };

            /*var result = await _libraryUserRepo.AddAsync(libraryUser);*/

            /*if (result is null)
            {
                //await deleteUser(model.Email);
                return new ReturnModel(false, "Internal Db error, registration failed");
            }*/

            return new ReturnModel(true, "Registration successfully");
        }

        public void Register()
        {
            throw new System.NotImplementedException();
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
                //await deleteUser(model.Email);
                return (false, "Role Add failed", null); //delete created user!
            }
            return (true, "User Created", userEntity.Id);
        }
    }
}
