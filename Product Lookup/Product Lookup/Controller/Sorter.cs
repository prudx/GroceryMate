using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Product_Lookup.Model
{
    class Sorter
    {
        public Receipt DetermineStore(string receipt)
        {
            Receipt r = null;

            if (receipt.ToUpper().Contains("TESCO"))
            {
                r = new TescoReceipt(receipt);
     
            }
            else if (receipt.ToUpper().Contains("DUNNES"))
            {
                //Receipt r = new DunnesReceipt(receipt);
            };
     
            return r;
        }
    }
}