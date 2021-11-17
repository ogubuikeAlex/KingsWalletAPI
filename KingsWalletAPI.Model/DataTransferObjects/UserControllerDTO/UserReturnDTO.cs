using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsWalletAPI.Model.DataTransferObjects.UserControllerDTO
{
    public class UserReturnDTO
    {
        public string Id { get; set; }
        public string FullName{ get; set; }

        public bool IsActive { get; set; }
        
    }
}
