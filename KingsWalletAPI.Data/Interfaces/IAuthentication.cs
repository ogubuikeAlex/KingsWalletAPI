using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KingsWalletAPI.Model.DataTransferObjects.UserControllerDTO;

namespace KingsWalletAPI.Data.Interfaces
{
    public interface IAuthentication
    {
        Task<bool> ValidateUser(LoginDTO userForAuth);
        Task<string> CreateToken();
    }
}
