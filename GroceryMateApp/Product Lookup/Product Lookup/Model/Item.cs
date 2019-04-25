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

namespace GroceryMate.Model
{
    public class Item
    {
        [Newtonsoft.Json.JsonProperty("id")] //maybe can't be caps?
        public string Id { get; set; }

        [Newtonsoft.Json.JsonProperty("itemId")] //maybe can't be caps?
        public int ItemId { get; set; } //primary key

        [Newtonsoft.Json.JsonProperty("receiptId")] //maybe can't be caps?
        public int ReceiptId { get; set; } //foreign key of receipt class

        public Receipt Receipt { get; set; } //refrence

        [Newtonsoft.Json.JsonIgnore]
        public string Image { get; set; }

        [Newtonsoft.Json.JsonProperty("name")]
        public string Name { get; set; }

        [Newtonsoft.Json.JsonProperty("price")]
        public double Price { get; set; }


        [Newtonsoft.Json.JsonProperty("createdAt")]
        public DateTimeOffset CreatedAt { get; set; }

        [Microsoft.WindowsAzure.MobileServices.Version]
        public string AzureVersion { get; set; }


        /*
          public string ItemId { get; set; }

        public string ReceiptId { get; set; } //foreign key of receipt class

        public Receipt Receipt { get; set; } //refrence


        public string Name { get; set; }

        public double Price { get; set; }
             
             */


        //[Newtonsoft.Json.JsonIgnore] if you want to not push to backend

        public Item()
        {

        }

        public Item(string n, double p)
        {
            Name = n;
            Price = p;
        }

        public Item(string n, double p, string img)
        {
            Name = n;
            Price = p;
            Image = img;
        }
    }
}