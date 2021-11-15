using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsWalletAPI.Service.Interfaces
{
    public interface IWalletService
    {
        public void Transfer();
        public void FundAccount();
        public void PayBills();
        public void ViewTransactions();
    }
}
