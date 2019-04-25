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
using GroceryMate;

namespace GroceryMate.Model
{
    public class Receipt
    {
        [Newtonsoft.Json.JsonProperty("Id")] //PK unused 
        public string Id { get; set; }

        [Newtonsoft.Json.JsonProperty("receiptId")]
        public int ReceiptId { get; set; } //pseudo pk (non garble identifier)

        [Newtonsoft.Json.JsonProperty("userId")]
        public string UserId { get; set; } //foreign key of user class

        public User User { get; set; } //refrence

        [Newtonsoft.Json.JsonProperty("storeName")]
        public string StoreName { get; set; }

        public ICollection<Item> Items { get; set; } //collection of items






        [Microsoft.WindowsAzure.MobileServices.Version]
        public string AzureVersion { get; set; }

        public Receipt()
        {

        }

        public Receipt(string storeName, List<Item> sortedReciept)
        {
            StoreName = storeName;
            Items = sortedReciept;
        }
    }
}