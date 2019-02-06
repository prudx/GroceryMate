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
        string Name { get; set; }
        double Price { get; set; }

        Item(string n, double p)
        {
            Name = n;
            Price = p;
        }
    }
}