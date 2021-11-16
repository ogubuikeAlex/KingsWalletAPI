using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KingsWalletAPI.Model.Entites;

namespace KingsWalletAPI.Model.Helpers
{
    public static class Converter
    {
        public static IEnumerable ConvertEnumToList(Enum model)
        {
            var agencyNames = Enum.GetNames(model.GetType()); 

            return agencyNames.ToList();
        }
    }
}
