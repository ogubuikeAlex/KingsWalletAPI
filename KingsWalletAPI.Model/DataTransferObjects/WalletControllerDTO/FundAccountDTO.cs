using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsWalletAPI.Model.DataTransferObjects.WalletControllerDTO
{
    public class FundAccountDTO
    {

        [Required(ErrorMessage = "Invalid Amount!")]
        [Range(typeof(decimal), "1000", "900000000000000000", ErrorMessage = "Deposit amount must be between $100000 - $900000000000000000\nAmount less than $100000, Please use the Atm\nAmount over  $900000000000 is not allowed at a go --CBU")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }       
        public string WalletId { get; set; }
    }
}
