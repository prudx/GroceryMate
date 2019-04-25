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
    public class User
    {
        [Newtonsoft.Json.JsonProperty("id")] //required but unused? PK 
        public string Id { get; set; }
        
        [Newtonsoft.Json.JsonProperty("userId")]
        public string UserId { get; set; } //setting ssid or maybe set to the above ID property?

        public List<Receipt> Receipts { get; set; } //list of receipts

        [Newtonsoft.Json.JsonProperty("username")]
        public string Username { get; set; }

        [Newtonsoft.Json.JsonProperty("name")]
        public string Name { get; set; }

        [Newtonsoft.Json.JsonProperty("address")]
        public string Address { get; set; }

        [Newtonsoft.Json.JsonProperty("phone")]
        public int Phone { get; set; }





        [Microsoft.WindowsAzure.MobileServices.Version]
        public string AzureVersion { get; set; }

        

        public User()
        {

        }
    }
}