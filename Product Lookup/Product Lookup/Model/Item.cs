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
    public class Item
    {
        //[PrimaryKey,AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }

        public Item()
        {

        }

        public Item(string n, double p)
        {
            Name = n;
            Price = p;
        }
    }
}