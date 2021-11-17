using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsWalletAPI.Model.Helpers
{
    public class ReturnModel
    {
        public ReturnModel()
        {

        }

        public ReturnModel(bool success, string message)
        {
            Success = success;

            Message = message;
        }

        public ReturnModel(bool success, string message, dynamic _object) : this (success, message)
        {
            Object = _object;
        }

        public bool Success { get; set; }

        public string Message { get; set; }

        public dynamic Object { get; set; }

    }
}
