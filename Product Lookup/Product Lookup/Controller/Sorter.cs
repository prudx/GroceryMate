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

namespace Product_Lookup.Model
{
    static class Sorter
    {
        public static Receipt DetermineStore(string receipt)
        {
            Receipt r = null;

            if (receipt.ToUpper().Contains("TESCO"))
            {
                r = new Receipt_Tesco(receipt);
     
            }
            else if (receipt.ToUpper().Contains("LIDL"))
            {
                r = new Receipt_Lidl(receipt);
            };
     
            return r;
        }

        public static List<Item> ItemListBuilder(string cleanedReceipt)
        {
            List<Item> Items = new List<Item>();
            List<double> prices = new List<double>();
            List<string> names = new List<string>();

            List<string> individual = new List<string>(cleanedReceipt.Split("\n"));

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
    }
}