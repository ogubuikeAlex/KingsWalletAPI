using System;
using System.Security.Cryptography;

namespace KingsWalletAPI.Model.Helpers
{
    public class RandomGenerator
    {
        public static string GenerateWalletId()
        {
            var milisecndns = string.Format("{0:000}", DateTime.Now.Millisecond);
            var year = DateTime.Now.ToString("yy");
            var month = string.Format("{0:00}", DateTime.Now.Month);
            var day = (RandomNumberGenerator.GetInt32(100, 999)).ToString();
            return $"{year}{month}{milisecndns}{day}";
        }
    }
}
