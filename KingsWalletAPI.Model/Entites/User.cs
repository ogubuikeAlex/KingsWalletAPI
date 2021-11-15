using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace KingsWalletAPI.Model.Entites
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }
        public bool IsActive { get; set; }
        public Guid WalletId { get; set; }
    }
}
