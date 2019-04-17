using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

/*                      
 *                      DEPRECATED
 *                      CLASS
 *                      
 *                      CODE MOVED TO RECEIPTSORTER.CS IN HELPERS FOLDER
 *                      
 *           
namespace Product_Lookup.Model
{
    public class Receipt_Lidl : Receipt
    {
        public override string StoreName => "Lidl";

        public List<Item> Items { get; set; }

        public Receipt_Lidl()
        {

        }

        public Receipt_Lidl(string receipt) : base()
        {
            ReceiptData = receipt;
            Items = new List<Item>();

        }

        public override List<Item> GetItems()
        {
            ReceiptData = ReceiptData.ToUpper();
            ReceiptData = ReceiptData.Replace("LIDL", "");
            ReceiptData = ReceiptData.Replace("I R E LA N D", "");
            ReceiptData = ReceiptData.Replace("I REL AND", "");
            ReceiptData = ReceiptData.Replace("IREL AND", "");
            ReceiptData = ReceiptData.Replace("R E L A ND", "");
            ReceiptData = ReceiptData.Replace("I E", "");
            ReceiptData = ReceiptData.Replace("VISIT VIE", "");
            ReceiptData = ReceiptData.Replace("CHANGE DUE", "");
            ReceiptData = ReceiptData.Replace("SIGN UP FOR CLUBCARD!", "");
            ReceiptData = ReceiptData.Replace("R CI.UBCARD", "");
            ReceiptData = ReceiptData.Replace("YOU COULD HAVE EARNED", "");
            ReceiptData = ReceiptData.Replace("CLUBCARD POINTS IN THIS TRANSACTION", "");
            ReceiptData = ReceiptData.Replace("CLUBCAR D POINTS IN THIS TRARSACTION", "");
            ReceiptData = ReceiptData.Replace("VISA CONTACTLESS", "");
            ReceiptData = ReceiptData.Replace("AID", "");
            ReceiptData = ReceiptData.Replace("NUMBER", "");
            ReceiptData = ReceiptData.Replace("PAN SEQ NO", "");
            ReceiptData = ReceiptData.Replace("AUTH CODE", "");
            ReceiptData = ReceiptData.Replace("MERCHANT", "");
            ReceiptData = ReceiptData.Replace("A CHANCE TO WIN", "");
            ReceiptData = ReceiptData.Replace("BY TELLING US ABOUT YOUR TRIP", "");
            ReceiptData = ReceiptData.Replace("BY TEL LING US ABOUT YOUR TRIP", "");
            ReceiptData = ReceiptData.Replace("VI EWS. IE", "");
            ReceiptData = ReceiptData.Replace("AND COLLECT 25 CLUBCARD POINTS.", "");
            ReceiptData = ReceiptData.Replace("FOR FULL TERMS AND CONDITIONS", "");
            ReceiptData = ReceiptData.Replace("THANK YOU FOR", "");
            ReceiptData = ReceiptData.Replace("SHOPPING AT", "");
            ReceiptData = ReceiptData.Replace("SLAN ABHAILE", "");
            ReceiptData = ReceiptData.Replace("SLAN ABHAI L E", "");
            ReceiptData = ReceiptData.Replace("TALLAGHT", "");

            //build item list using generalized sorter class
            Items = Sorter.ItemListBuilder(ReceiptData);

            return Items;
        }
    }
}
*/