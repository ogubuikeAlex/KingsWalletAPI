using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KingsWalletAPI.Model.DataTransferObjects.WalletControllerDTO;
using KingsWalletAPI.Model.Entites;
using KingsWalletAPI.Model.Helpers;

namespace KingsWalletAPI.Service.Interfaces
{
    public interface IWalletService
    {
        Task<ReturnModel> Transfer(TransferDTO model);
        Task<ReturnModel> FundAccount(FundAccountDTO model);
        Task<ReturnModel> PayBills(PayBillDTO model);
        ReturnModel ViewTransactions(Guid id);
        IEnumerable<Transaction> ViewAllTransactions();
    }
}
