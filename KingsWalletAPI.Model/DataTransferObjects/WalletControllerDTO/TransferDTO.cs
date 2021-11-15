using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsWalletAPI.Model.DataTransferObjects.WalletControllerDTO
{
    public class TransferDTO
    {
        public decimal Amount { get; set; }
        public string ReceiverWalletId { get; set; }
        public string SenderWalletId { get; set; }
        public bool IsSuccessful { get; set; }
    }
}
