﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KingsWalletAPI.Model.Enums;

namespace KingsWalletAPI.Model.Entites
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public Guid UserId {get; set;}
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public TransactionType TransactionType { get; set; }
        public TransactionMode TransactionMode { get; set; }
        public string ReceiverWalletId { get; set; }
        public string SenderWalletId { get; set; }
    }
}
