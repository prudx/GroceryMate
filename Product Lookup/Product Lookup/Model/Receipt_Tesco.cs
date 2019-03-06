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
    public class Receipt_Tesco : Receipt
    {
        public override string StoreName => "Tesco";

        public List<Item> Items { get; set; }

        public Receipt_Tesco()
        {

        }

        public Receipt_Tesco(string receipt) : base()
        {
            ReceiptData = receipt;
            Items = new List<Item>();

        }

        public override List<Item> GetItems()
        {
            ReceiptData = ReceiptData.ToUpper();
            ReceiptData = ReceiptData.Replace("TESCO", "");
            ReceiptData = ReceiptData.Replace("TESSCO", "");
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
            ReceiptData = ReceiptData.Replace("TESCO GIFTCARD", "");
            ReceiptData = ReceiptData.Replace("BY TELLING US ABOUT YOUR TRIP", "");
            ReceiptData = ReceiptData.Replace("BY TEL LING US ABOUT YOUR TRIP", "");
            ReceiptData = ReceiptData.Replace("WWW . TESCOVIEWS . IE", "");
            ReceiptData = ReceiptData.Replace("VI EWS. IE", "");
            ReceiptData = ReceiptData.Replace("AND COLLECT 25 CLUBCARD POINTS.", "");
            ReceiptData = ReceiptData.Replace("FOR FULL TERMS AND CONDITIONS", "");
            ReceiptData = ReceiptData.Replace("PLEASE VISIT TESCOVIEWS.IE", "");
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




/*
 * moved this code snipet to the sorter class as it
 * is generalized among all receipt types.
 * 
    List<string> individual = new List<string>(ReceiptData.Split("\n"));
    List<double> prices = new List<double>();
    List<string> names = new List<string>();

    for (int i = 0; i < individual.Count; i++)
    {
        bool isDigitPresent = individual[i].Any(c => char.IsDigit(c));

        if (isDigitPresent && individual[i].Contains("."))                                  //if it is an actual price
            prices.Add(Convert.ToDouble(Regex.Replace(individual[i], "[^0-9.]", "")));
        else if (!isDigitPresent)                                                           //if there are no digits, add as a name (filtered constants)
            names.Add(individual[i]);

    }

    //this snipet should avoid the array index out of bounds error
    int loopCounter;    
    if (names.Count < prices.Count)
        loopCounter = names.Count;
    else
        loopCounter = prices.Count;

    //iterate through the Items list and add our item each loop
    for(int i = 0; i < loopCounter; i++)
    {
        Item tempItem = new Item(names[i], prices[i]);
        Items.Add(tempItem);
    }

    return Items;
*/


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
