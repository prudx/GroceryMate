using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using ProductAPI;
using Product_Lookup.API;
//using Product_Lookup.Model;
//using Result = Product_Lookup.Model.Result;
using Product_Lookup.Quicktype;
using Result = Product_Lookup.Quicktype.Result;
using Refit;
using EDMTDialog;
using System.Collections.Generic;
using System.Globalization;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;


namespace Product_Lookup
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {

        Button btn_SearchProducts;
        ListView list_Products;

        ITescoAPI tescoAPI;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            btn_SearchProducts = FindViewById<Button>(Resource.Id.btn_SearchProduct);
            list_Products = FindViewById<ListView>(Resource.Id.list_Products);

            /*
            //trying to fix the json to json array problem
            JsonConvert.DefaultSettings =
                () => new JsonSerializerSettings()
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    Converters = { new StringEnumConverter() }
                };
            */



            try
            {
                tescoAPI = RestService.For<ITescoAPI>("https://dev.tescolabs.com");
                //tescoAPI = RestService.For<ITescoAPI>("https://jsonplaceholder.typicode.com");
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, "" + ex.Message, ToastLength.Long).Show();
            }
            
            btn_SearchProducts.Click += async delegate
            {
                try
                {
                    Android.Support.V7.App.AlertDialog dialog = new EDMTDialogBuilder()
                    .SetContext(this)
                    .SetMessage("Loading...")
                    .Build();

                    if (!dialog.IsShowing)
                        dialog.Show();
                    
                    //JsonObjectAttribute json = await tescoAPI.GetUsers();  //JsonConvert.DeserializeObject<List<Result>>(response); 
                    Welcome results = await tescoAPI.GetUsers();//JsonConvert.DeserializeObject<List<Welcome>>(json);
                    List<string> result_name = new List<string>();

                    //result_name = results.Uk.Ghs.Products.Results;
                    
                    foreach (var result in results.Uk.Ghs.Products.Results)
                        result_name.Add(result.Name);
                    

                    var adapter = new ArrayAdapter<string>(this,
                        Android.Resource.Layout.SimpleListItem1, result_name);
                    list_Products.Adapter = adapter;

                    if (dialog.IsShowing)
                        dialog.Dismiss();
                }
                catch (Exception ex)
                {
                    Toast.MakeText(this, "" + ex.Message, ToastLength.Long).Show();
                }
            };
        }
    }
}

/*
string[] result_name = new string[10];

for (var i = 0; i < results.Count; i++)
    result_name[i] = results[i].name;
*/
