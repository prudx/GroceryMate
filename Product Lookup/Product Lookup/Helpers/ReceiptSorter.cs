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
using GroceryMate.Model;

namespace GroceryMate.Helpers
{
    static class Sorter
    {
        public static Receipt DetermineStore(string dirtyReceipt)
        {
            Receipt r = null;
            List<Item> sortedReceipt = new List<Item>();

            if (dirtyReceipt.ToUpper().Contains("TESCO"))
            {
                sortedReceipt = Sorter.ReceiptSort("TESCO", dirtyReceipt);
                r = new Receipt("TESCO", sortedReceipt);
            }
            else if (dirtyReceipt.ToUpper().Contains("LIDL"))
            {
                sortedReceipt = Sorter.ReceiptSort("LIDL", dirtyReceipt);
                r = new Receipt("LIDL", sortedReceipt);
            };
            return r;
        }

        public static List<Item> ItemListBuilder(string cleanedReceipt)
        {
            List<Item> Items = new List<Item>();
            List<double> prices = new List<double>();
            List<string> names = new List<string>();

            List<string> individual = new List<string>(cleanedReceipt.Split("\n"));
            individual.RemoveAt(0); //get rid of empty split before \n (was creating blank item)

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
            for (int i = 0; i < loopCounter; i++)
            {
                Item tempItem = new Item(names[i], prices[i]);
                Items.Add(tempItem);
            }

            return Items;
        }

        public static List<Item> ReceiptSort(string store, string dirtyReceipt)
        {
            if(store == "TESCO")
            {
                return FilterTesco(dirtyReceipt);
            }
            else if (store == "LIDL")
            {
                return FilterLidl(dirtyReceipt);
            }
            else
            {
                //create alert
                return null;
            }
        }

        public static List<Item> FilterTesco(string dirtyReceipt)
        {
            List<Item> temp = new List<Item>();

            dirtyReceipt = dirtyReceipt.ToUpper();
            dirtyReceipt = dirtyReceipt.Replace("TESCO", "");
            dirtyReceipt = dirtyReceipt.Replace("TESSCO", "");
            dirtyReceipt = dirtyReceipt.Replace("I R E LA N D", "");
            dirtyReceipt = dirtyReceipt.Replace("I REL AND", "");
            dirtyReceipt = dirtyReceipt.Replace("IREL AND", "");
            dirtyReceipt = dirtyReceipt.Replace("R E L A ND", "");
            dirtyReceipt = dirtyReceipt.Replace("I E", "");
            dirtyReceipt = dirtyReceipt.Replace("VISIT VIE", "");
            dirtyReceipt = dirtyReceipt.Replace("CHANGE DUE", "");
            dirtyReceipt = dirtyReceipt.Replace("SIGN UP FOR CLUBCARD!", "");
            dirtyReceipt = dirtyReceipt.Replace("R CI.UBCARD", "");
            dirtyReceipt = dirtyReceipt.Replace("YOU COULD HAVE EARNED", "");
            dirtyReceipt = dirtyReceipt.Replace("CLUBCARD POINTS IN THIS TRANSACTION", "");
            dirtyReceipt = dirtyReceipt.Replace("CLUBCAR D POINTS IN THIS TRARSACTION", "");
            dirtyReceipt = dirtyReceipt.Replace("VISA CONTACTLESS", "");
            dirtyReceipt = dirtyReceipt.Replace("AID", "");
            dirtyReceipt = dirtyReceipt.Replace("NUMBER", "");
            dirtyReceipt = dirtyReceipt.Replace("PAN SEQ NO", "");
            dirtyReceipt = dirtyReceipt.Replace("AUTH CODE", "");
            dirtyReceipt = dirtyReceipt.Replace("MERCHANT", "");
            dirtyReceipt = dirtyReceipt.Replace("A CHANCE TO WIN", "");
            dirtyReceipt = dirtyReceipt.Replace("TESCO GIFTCARD", "");
            dirtyReceipt = dirtyReceipt.Replace("BY TELLING US ABOUT YOUR TRIP", "");
            dirtyReceipt = dirtyReceipt.Replace("BY TEL LING US ABOUT YOUR TRIP", "");
            dirtyReceipt = dirtyReceipt.Replace("WWW . TESCOVIEWS . IE", "");
            dirtyReceipt = dirtyReceipt.Replace("VI EWS. IE", "");
            dirtyReceipt = dirtyReceipt.Replace("AND COLLECT 25 CLUBCARD POINTS.", "");
            dirtyReceipt = dirtyReceipt.Replace("FOR FULL TERMS AND CONDITIONS", "");
            dirtyReceipt = dirtyReceipt.Replace("PLEASE VISIT TESCOVIEWS.IE", "");
            dirtyReceipt = dirtyReceipt.Replace("THANK YOU FOR", "");
            dirtyReceipt = dirtyReceipt.Replace("SHOPPING AT", "");
            dirtyReceipt = dirtyReceipt.Replace("SLAN ABHAILE", "");
            dirtyReceipt = dirtyReceipt.Replace("SLAN ABHAI L E", "");
            dirtyReceipt = dirtyReceipt.Replace("TALLAGHT", "");

            //build item list using generalized sorter class
            temp = Sorter.ItemListBuilder(dirtyReceipt);
            return temp;
        }

        public static List<Item> FilterLidl(string dirtyReceipt)
        {
            List<Item> temp = new List<Item>();

            dirtyReceipt = dirtyReceipt.ToUpper();
            dirtyReceipt = dirtyReceipt.Replace("LIDL", "");
            //FURTHER STRING PROCCESSING REQUIRED HERE

            //build item list using generalized sorter class
            temp = Sorter.ItemListBuilder(dirtyReceipt);
            return temp;
        }
    }
}