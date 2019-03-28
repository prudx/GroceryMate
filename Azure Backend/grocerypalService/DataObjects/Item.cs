using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace grocerypalService.DataObjects
{
    public class Item : EntityData
    {
        public string Image { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }
    }
}