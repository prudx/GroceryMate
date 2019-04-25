using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Azure.Mobile.Server;

namespace GroceryFriendService.DataObjects
{
    public class User : EntityData
    {
        [Newtonsoft.Json.JsonProperty("userId")]
        public string UserId { get; set; } //needed for context

        public ICollection<Receipt> Receipts { get; set; } //list of receipts

        [Newtonsoft.Json.JsonProperty("username")]
        public string Username { get; set; }

        [Newtonsoft.Json.JsonProperty("name")]
        public string Name { get; set; }

        [Newtonsoft.Json.JsonProperty("address")]
        public string Address { get; set; }

        [Newtonsoft.Json.JsonProperty("phone")]
        public int Phone { get; set; }
    }
}