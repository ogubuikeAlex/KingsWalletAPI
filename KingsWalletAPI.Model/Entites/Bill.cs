using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KingsWalletAPI.Model.Enums;

namespace KingsWalletAPI.Model.Entites
{
    public class Bill
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string WalletId { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string BillType { get; set; }  
    }
}
