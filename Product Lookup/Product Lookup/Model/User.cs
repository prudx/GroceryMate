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
    public class User
    {
        [Newtonsoft.Json.JsonProperty("Id")] //required but unused? PK 
        public string Id { get; set; }
        
        [Newtonsoft.Json.JsonProperty("userId")]
        public int UserId { get; set; } //setting ssid or maybe set to the above ID property?

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