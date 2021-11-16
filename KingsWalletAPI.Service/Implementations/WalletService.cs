using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KingsWalletAPI.Data.Interfaces;
using KingsWalletAPI.Model.DataTransferObjects.WalletControllerDTO;
using KingsWalletAPI.Model.Entites;
using KingsWalletAPI.Model.Enums;
using KingsWalletAPI.Model.Helpers;
using KingsWalletAPI.Service.Interfaces;

namespace KingsWalletAPI.Service.Implementations
{
    public class WalletService : IWalletService
    {
        private readonly IRepository<Transaction> _transactionRepo;
        private readonly IRepository<Wallet> _walletRepo;
        private readonly IRepository<Bill> _billRepo;    

        public WalletService(IUnitOfWork unitOfWork)
        {           
            _transactionRepo = unitOfWork.GetRepository<Transaction>();
            _walletRepo = unitOfWork.GetRepository<Wallet>();
            _billRepo = unitOfWork.GetRepository<Bill>();
           
        }
        public async Task<ReturnModel> FundAccount(FundAccountDTO model)
        {
            var wallet = _walletRepo.GetSingleByCondition(w => w.WalletId == model.WalletId);

            if (wallet is null) return new ReturnModel(false, "Your Wallet is Invalid, Please visit the branch you opened your account for clarification");
 
            if (!wallet.User.IsActive) return new ReturnModel(false, "Your account has been deactivated.");           

            wallet.Balance += model.Amount;
            var transaction = new Transaction
            {
                Amount = model.Amount,
                CreatedAt = DateTime.Now,
                TransactionMode = TransactionMode.Credit,
                TransactionType = TransactionType.Deposit,
                UserId = wallet.UserId,
                ReceiverWalletId = wallet.WalletId,
                SenderWalletId = wallet.WalletId
            };
            await _transactionRepo.AddAsync(transaction);           

            return new ReturnModel(true, "Your Deposit was Successful!") ;
        }

        public async Task<ReturnModel> PayBills(PayBillDTO model)
        {
            var receiverWallet = _walletRepo.GetSingleByCondition(w => w.WalletId == model.ReceiverWalletId);
            var senderWallet = _walletRepo.GetSingleByCondition(w => w.WalletId == model.SenderWalletId);

            var validationResult = ValidateTransferCredentials(receiverWallet, senderWallet, model.Amount);

            if (!validationResult.Success)
                return new ReturnModel(false, validationResult.Message);

            var listOfAgencies = Converter.ConvertEnumToList(new BillType());

            foreach(var agency in listOfAgencies)
            {                
                if (receiverWallet.User.FullName == agency.ToString())
                {
                    await CreateTransactionsForTransfer(receiverWallet, senderWallet, model.Amount);                   

                    var billReport = new Bill
                    {
                        WalletId = senderWallet.WalletId,
                        Amount = model.Amount,
                        BillType = agency.ToString(),
                        CreatedAt = DateTime.Now,
                        UserId = senderWallet.UserId
                    };
                    
                    await _billRepo.AddAsync(billReport);

                    return new ReturnModel(true, "Bill Paid Successfully");
                }
            }

            return new ReturnModel(false, "The walletId you supplied is not allocated to a third party agency. Please Try again");
        }

        public async Task<ReturnModel> Transfer(TransferDTO model)
        {
            var receiverWallet = _walletRepo.GetSingleByCondition(w => w.WalletId == model.ReceiverWalletId);

            var senderWallet = _walletRepo.GetSingleByCondition(w => w.WalletId == model.SenderWalletId);

            var validationResult = ValidateTransferCredentials(receiverWallet, senderWallet, model.Amount);

            if (!validationResult.Success)
                return new ReturnModel(false, validationResult.Message);

            await CreateTransactionsForTransfer(receiverWallet, senderWallet, model.Amount);           
           
            return new ReturnModel(true, "Your Transfer was Successful");
        }

        public ReturnModel ViewTransactions(Guid userId)
        {
            //get all transactions where the userId == User id 
            var transactions = _transactionRepo.GetByCondition(t => t.UserId == userId);

            if (transactions == null)
            {
                return new ReturnModel(false, "This User does not have any transactions yet");
            }
            return new ReturnModel(true, "", transactions);
        }
        
        public IEnumerable<Transaction> ViewAllTransactions()
        {
            return _transactionRepo.GetAll();
        }

        private ReturnModel ValidateTransferCredentials(Wallet receiverWallet, Wallet senderWallet, decimal amount)
        {
            var isSenderAccountValid = senderWallet != null;

            var isReciepientAccountValid = receiverWallet != null;

            var isSenderAccountActive = senderWallet?.User.IsActive;

            var isReciepientAccountActive = receiverWallet?.User.IsActive;

            var isAmountValid = amount <= 0;

            var isBalanceSufficient = senderWallet?.Balance >= amount;

            var isReciepientAccountDifferent = receiverWallet?.WalletId != senderWallet?.WalletId;            

            var message = !isSenderAccountValid ? "Your wallet is Invalid,Please visit the branch you opened your account for clarification" :
                       !isSenderAccountActive.Value ? "This wallet is inactive, Please visit the branch you opened your account for clarification" :
                       !isReciepientAccountValid ? "Reciepient wallet is Invalid" :
                       !isReciepientAccountActive.Value ? "Reciepient wallet is inactive" :
                       !isReciepientAccountDifferent ? "You cannot transfer to yourself" :
                       isAmountValid ? "Cant Transfer an amount equal to or less than $0" :
                       !isBalanceSufficient ? "Insufficient funds" :
                       (senderWallet.Balance -= amount) < 1000 ? "Insufficient funds!A maintenance fee of $1000 is required for kingswallet" :
                       string.Empty;

            if (!string.IsNullOrEmpty(message)) return new ReturnModel(false, message);

            return new ReturnModel(true, "Valid");
        } 

        private async Task CreateTransactionsForTransfer(Wallet receiverWallet, Wallet senderWallet, decimal amount)
        {
            senderWallet.Balance -= amount;
            receiverWallet.Balance += amount;

            var senderTransaction = new Transaction
            {
                Amount = amount,
                UserId = senderWallet.UserId,
                CreatedAt = DateTime.Now,
                TransactionMode = TransactionMode.Debit,
                TransactionType = TransactionType.Transfer,
                ReceiverWalletId = receiverWallet.WalletId,
                SenderWalletId = senderWallet.WalletId,
            };

            var reciepientTransaction = new Transaction
            {
                Amount = amount,
                UserId = receiverWallet.UserId,
                CreatedAt = DateTime.Now,
                TransactionMode = TransactionMode.Credit,
                TransactionType = TransactionType.Transfer,
                ReceiverWalletId = receiverWallet.WalletId,
                SenderWalletId = senderWallet.WalletId
            };
            await _transactionRepo.AddAsync(senderTransaction);
            await _transactionRepo.AddAsync(reciepientTransaction);
        }
    }
}
