using System;99
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Product_Lookup;
//make sure it's taking in in order camera 
//can possibly use Azure ocr if it returns in chronological unlike GOOGLE api and would be hosted ijn azure with my db

//use seperate dll library looks good software engineering for updatable code

//make a user model class with unique id's and login emails, create a db from it hosted in azure
//authentication of users
//have capture create a reciept item and
//user 1 many shopping order 1 many items

//try get the ocr performed on an IMAGE instead of VIDEO stream, may increase performance.
//google vision api is an android.gms library and didn't ahve to set up in azure
//azure would be running alonside database but performs about as well as google vision api anyway.



namespace Product_Lookup.Model
{
    class TescoReceipt : Receipt
    {
        public override string StoreName => "Tesco";

        public TescoReceipt()
        {

        }

        public TescoReceipt(string receipt) : base()
        {
            ReceiptData = receipt;

        }

        public override string GetItems(string ReceiptData)
        {
            //List<string> CleaningData;
            string clean;

            string[] strArr;

            ReceiptData = ReceiptData.ToUpper();
            ReceiptData.Replace("SIGN UP FOR CLUBCARD!", "");
            ReceiptData.Replace("YOU COULD HAVE EARNED", "");
            ReceiptData.Replace("CLUBCARD POINTS IN THIS TRANSACTION", "");
            ReceiptData.Replace("VISA CONTACTLESS", ""); 
            ReceiptData.Replace("AID", "");
            ReceiptData.Replace("NUMBER", "");
            ReceiptData.Replace("PAN SEQ NO", "");
            ReceiptData.Replace("AUTH CODE", "");
            ReceiptData.Replace("MERCHANT", "");
            ReceiptData.Replace("A CHANCE TO WIN", "");
            ReceiptData.Replace("TESCO GIFTCARD", "");
            ReceiptData.Replace("BY TELLING US ABOUT YOUR TRIP", "");
            ReceiptData.Replace("WWW . TESCOVIEWS . IE", "");
            ReceiptData.Replace("AND COLLECT 25 CLUBCARD POINTS.", "");
            ReceiptData.Replace("FOR FULL TERMS AND CONDITIONS", "");
            ReceiptData.Replace("PLEASE VISIT TESCOVIEWS.IE", "");
            ReceiptData.Replace("THANK YOU FOR", "");
            ReceiptData.Replace("SHOPPING AT", "");
            ReceiptData.Replace("SLAN ABHAILE", "");
            //ReceiptData.Replace("AID", "");



            //strArr = ReceiptData.Split("TOTAL");

            //get everything before the total
            //ReceiptData = strArr[0];

            //strArr = ReceiptData.Split("TESCO");



            return ReceiptData;
        }
    }
}