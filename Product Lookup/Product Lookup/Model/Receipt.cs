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
    public class Receipt
    {
        public string ReceiptId { get; set; } //primary key

        public string UserId { get; set; } //foreign key of user class

        public string StoreName { get; set; }

        public ICollection<Item> Items { get; set; } //can be list

        public Receipt(string sn, List<Item> sortedReciept)
        {
            StoreName = sn;
            Items = sortedReciept;
        }
    }
}