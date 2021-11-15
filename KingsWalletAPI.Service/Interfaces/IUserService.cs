using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsWalletAPI.Service.Interfaces
{
    public interface IUserService
    {
        public void Register();
        public void Login();
        public void DeactivateUser();
    }
}
