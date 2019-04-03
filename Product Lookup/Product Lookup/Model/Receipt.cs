﻿using System;
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
    public abstract class Receipt
    {
        public abstract string StoreName { get; }

        public string ReceiptId { get; set; } //primary key

        public string UserId { get; set; } //foreign key of user class

        public string ReceiptData { get; set; } //maybe make this abstract??


        public abstract List<Item> GetItems(); //return list items?

        public Receipt()
        {

        }

    }
}