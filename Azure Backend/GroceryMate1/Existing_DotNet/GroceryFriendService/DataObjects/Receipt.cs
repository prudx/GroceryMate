using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Azure.Mobile.Server;

namespace GroceryFriendService.DataObjects
{
    public class Receipt : EntityData
    {
        [Newtonsoft.Json.JsonProperty("receiptId")]
        public int ReceiptId { get; set; }

        [Newtonsoft.Json.JsonProperty("userId")]
        public string UserId { get; set; } //foreign key of user class

        public User User { get; set; } //refrence

        [Newtonsoft.Json.JsonProperty("storeName")]
        public string StoreName { get; set; }

        public ICollection<Item> Items { get; set; } //collection of items
    }
}