using System.Threading.Tasks;
using KingsWalletAPI.Model.DataTransferObjects.UserControllerDTO;
using KingsWalletAPI.Model.Entites;
using KingsWalletAPI.Model.Helpers;

namespace KingsWalletAPI.Service.Interfaces
{
    public interface IUserService
    {
        Task<ReturnModel> Register(RegisterDTO model);
        Task<ReturnModel> DeactivateUser(string userId);
        Task<UserReturnDTO> GetUserAsync(string id);
    }
}
