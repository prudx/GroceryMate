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
using Product_Lookup;

namespace Product_Lookup.Model
{
    class TescoReceipt : Receipt
    {
        public override string StoreName => "Tesco";

        public TescoReceipt(string receipt) : base()
        {
            ReceiptData = receipt;

        }

        public override void GetItems(string ReceiptData)
        {
            //List<string> CleaningData;
            string clean;

            string[] strArr;

            ReceiptData = ReceiptData.ToUpper();
            ReceiptData.Replace("SIGN UP FOR CLUBCARD!", "");
            ReceiptData.Replace("YOU COULD HAVE EARNED", "");
            ReceiptData.Replace("CLUBCARD POINTS IN THIS TRANSACTION", "");

            strArr = ReceiptData.Split("TOTAL");

            //get everything before the total
            ReceiptData = strArr[0];

            strArr = ReceiptData.Split("TESCO");



            //return null;
        }
    }
}