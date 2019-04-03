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
    class User
    {
        public string UserId { get; set; } //primary key

        public string Username { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public int Phone { get; set; }
    }
}