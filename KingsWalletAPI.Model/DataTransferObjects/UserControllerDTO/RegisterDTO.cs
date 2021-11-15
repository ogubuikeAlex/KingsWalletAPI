using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsWalletAPI.Model.DataTransferObjects.UserControllerDTO
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Name Required")]
        public string FullName { get; set; }       
        [Required(ErrorMessage = "Email required")]
        [EmailAddress]
        public string Email { get; set; } 
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
