using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsWalletAPI.Model.Entites
{
    public class Wallet
    {
        public Guid Id { get; set; }
        public decimal Balance { get; set; }       
        public string WalletId { get; set } 
    }
}
