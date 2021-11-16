using System;
using System.Threading.Tasks;
using AutoMapper;
using KingsWalletAPI.Data.Implementations;
using KingsWalletAPI.Data.Interfaces;
using KingsWalletAPI.Model.DataTransferObjects.UserControllerDTO;
using KingsWalletAPI.Model.Entites;
using KingsWalletAPI.Model.Enums;
using KingsWalletAPI.Model.Helpers;
using KingsWalletAPI.Service.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace KingsWalletAPI.Service.Implementations
{
    public class UserService : IUserService
    {
        private IMapper _mapper;
        private readonly IRepository<Wallet> _walletRepo;        
        private readonly UserManager<User> _userManager;

        public UserService(IUnitOfWork unitOfwork, IMapper mapper, UserManager<User> userManager)
        {
            _mapper = mapper;
            _walletRepo = unitOfwork.GetRepository<Wallet>();
            _userManager = userManager;
        }
        public async Task<ReturnModel> DeactivateUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            user.IsActive = false;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return new ReturnModel(false, "User not deactivated");

            return new ReturnModel(false, "User Deactivated Successfully");
        }       

        public async Task<ReturnModel> Register(RegisterDTO model)
        {
            var createUserResult = await CreateUserAsync(model);

            if (!createUserResult.Success)
                return new ReturnModel(false, createUserResult.Message);

            var wallet = new Wallet {
                WalletId = RandomGenerator.GenerateWalletId(),
                UserId = Guid.Parse(createUserResult.Object.ToString())
            };

            var result = await _walletRepo.AddAsync(wallet);

            if (result is null)
            {
                await deleteUser(model.Email);
                return new ReturnModel(false, "Internal Db error, registration failed");
            }

            return new ReturnModel(true, "Registration successfully");
        }   

        protected async Task<ReturnModel> CreateUserAsync(RegisterDTO model)
        {
            var userEntity = _mapper.Map<User>(model);
            userEntity.UserName = model.Email;
            userEntity.IsActive = true;

            await _userManager.UpdateAsync(userEntity);
            var createUserResult = await _userManager.CreateAsync(userEntity, model.Password);
            if (!createUserResult.Succeeded) return new ReturnModel(false, "Registration failed, User not created!!", null);
            var addRoleResult = await _userManager.AddToRoleAsync(userEntity, Roles.User.ToString());
            if (!addRoleResult.Succeeded)
            {
                await deleteUser(model.Email);
                return new ReturnModel(false, "Role Add failed", null); //delete created user!
            }
            return new ReturnModel(true, "User Created", userEntity.Id);
        }

        protected async Task deleteUser(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            await _userManager.DeleteAsync(user);
        }
    }
}
