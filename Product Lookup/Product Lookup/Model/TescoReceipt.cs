using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
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
    public class TescoReceipt : Receipt
    {
        public override string StoreName => "Tesco";

        public List<Item> Items { get; set; }

        public TescoReceipt()
        {

        }

        public TescoReceipt(string receipt) : base()
        {
            ReceiptData = receipt;
            Items = new List<Item>();

        }

        public override List<Item> GetItems()
        {
            //List<string> CleaningData;
            //string clean;

            //string[] strArr;

            ReceiptData = ReceiptData.ToUpper();
            ReceiptData.Replace("TESCO", "");
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
            

            List<string> individual = new List<string>(ReceiptData.Split("\n"));
            List<double> prices = new List<double>();
            List<string> names = new List<string>();

            for (int i = 0; i < individual.Count; i++)
            {
                bool isDigitPresent = individual[i].Any(c => char.IsDigit(c));

                if (isDigitPresent)
                {
                    prices.Add(Convert.ToDouble(Regex.Replace(individual[i], "[^0-9.]", "")));
                } else
                {
                    names.Add(individual[i]);
                }
            }

            for(int i = 0; i < individual.Count/2; i++)
            {
                Item tempItem = new Item(names[i], prices[i]);
                Items.Add(tempItem);
            }

            return Items;
        }
    }
}


/*    
 *    this is what 2am code looks like
 *    
Items = new List<Item>();
for (int i = 0; i < individual.Count; i++)
{
    Item newitem = new Item();
    bool isDigitPresent = individual[i].Any(c => char.IsDigit(c)); // was throwing exception

    for (int j = 0; j < individual.Count; j++)
    {
        if (isDigitPresent)
        {
            newitem.Price = Convert.ToDouble(Regex.Replace(individual[i], "[^0-9.]", ""));
        }
    }
    for (int j = 0; j < individual.Count; j++)
    { 
        if (!isDigitPresent)
        {
            newitem.Name = individual[i];

        }
    }
    Items.Add(newitem);
}
*/
