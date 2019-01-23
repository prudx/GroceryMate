using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
//using Product_Lookup.Model;
using Refit;
//using Result = Product_Lookup.Model.Result;
using Product_Lookup.Quicktype;
using Result = Product_Lookup.Quicktype.Result;
using Newtonsoft.Json;

namespace Product_Lookup.API
{
    [Headers("Ocp-Apim-Subscription-Key: 9c3efb16d55a471781e299822b6b01be")]
    public interface ITescoAPI
    {
        //[Get("/users")]
        //[Headers("Ocp-Apim-Subscription-Key: 9c3efb16d55a471781e299822b6b01be")]
        [Get("/grocery/products/?query=milk&offset=0&limit=10")]
        Task<Welcome> GetUsers();
    }
}