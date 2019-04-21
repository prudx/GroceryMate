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
        [Newtonsoft.Json.JsonProperty("Id")] //maybe can't be caps?
        public string Id { get; set; }
        
        [Newtonsoft.Json.JsonProperty("userId")]
        public string UserId { get; set; } //primary key

        public List<Receipt> Receipts { get; set; } //list of receipts



        public string Username { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public int Phone { get; set; }

        [Microsoft.WindowsAzure.MobileServices.Version]
        public string AzureVersion { get; set; }

        public User()
        {

        }
    }
}