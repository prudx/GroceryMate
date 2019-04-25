using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Azure.Mobile.Server;

namespace GroceryFriendService.DataObjects
{
    public class Item : EntityData
    {
        [Newtonsoft.Json.JsonProperty("itemId")]
        public int ItemId { get; set; }

        [Newtonsoft.Json.JsonProperty("receiptId")]
        public int ReceiptId { get; set; } //foreign key of receipt class

        public Receipt Receipt { get; set; } //refrence

        [Newtonsoft.Json.JsonProperty("name")]
        public string Name { get; set; }

        [Newtonsoft.Json.JsonProperty("price")]
        public double Price { get; set; }
    }
}