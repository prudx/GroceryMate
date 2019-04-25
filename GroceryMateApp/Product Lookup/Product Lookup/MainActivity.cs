using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using Android.Views;
using Android.Views.InputMethods;
using GroceryMate.API;
using GroceryMate.JsonData;
using Refit;
using EDMTDialog;
using System.Collections.Generic;
using System;
using Android.Content;
using GroceryMate.Model;
using GroceryMate.Resources.adapters;
using Microsoft.WindowsAzure.MobileServices;
using GroceryMate.Services;
using GroceryMate.Helpers;

namespace GroceryMate
{
    [Activity(Label = "GroceryMate", Theme = "@style/Theme.AppCompat.Light", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {

        Button btn_SearchProducts;
        Button btn_Camera;
        ListView list_Products;
        EditText editText;
        string queryString;

        ITescoAPI tescoAPI;

        public AzureService azureService = new AzureService(); //Not sure how to pass through activities?

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //initiate azure app service
            Settings.UserSid = null; //reset userId 
            Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, savedInstanceState);

            SetContentView(GroceryMate.Resource.Layout.activity_main);

            btn_SearchProducts = FindViewById<Button>(GroceryMate.Resource.Id.btn_SearchProduct);
            btn_Camera = FindViewById<Button>(GroceryMate.Resource.Id.btn_Camera);

            list_Products = FindViewById<ListView>(GroceryMate.Resource.Id.list_Products);

            editText = FindViewById<EditText>(GroceryMate.Resource.Id.queryInput);
            editText.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) => {
                queryString = e.Text.ToString();
            };            

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
                    //https://dev.tescolabs.com/grocery/products/?query=milk&offset=0&limit=10
                    //hardcoded instead of httpUtil obj
                    //queryString = "query=orange&offset=0&limit=10";
                    
                    
                    Android.Support.V7.App.AlertDialog dialog = new EDMTDialogBuilder()
                    .SetContext(this)
                    .SetMessage("Searching for " + queryString)
                    .Build();

                    if (!dialog.IsShowing)
                        dialog.Show();

                    //API SEARCH REQUEST
                    RootObject results = await tescoAPI.GetUsers(queryString, 0, 50);
                    List<Item> resultList = new List<Item>();
                    
                    foreach (var result in results.Uk.Ghs.Products.Results)
                    {
                        Item temp = new Item()
                        {
                            Image = result.Image,
                            Name = result.Name,
                            Price = result.Price
                        };
                        resultList.Add(temp);
                    }


                    //foreach (var result in results.Uk.Ghs.Products.Results)
                    //resultList.Add(result.Name);
                    var adapter = new ProductSearch_Adapter(this, resultList);
                    list_Products.Adapter = adapter;

                    /*
                    var adapter = new ArrayAdapter<string>(this,
                        Android.Resource.Layout.SimpleListItem1, resultList);
                    list_Products.Adapter = adapter;
                    */
             
                    if (dialog.IsShowing)
                        dialog.Dismiss();

                    
                }
                catch (Exception ex)
                {
                    Toast.MakeText(this, "" + ex.Message, ToastLength.Long).Show();
                }

            };

            btn_Camera.Click += (s, e) =>
            {
                Intent cameraActivity = new Intent(this, typeof(CameraActivity));
                StartActivity(cameraActivity);
            };
        }

        [Java.Interop.Export()]
        public async void LoginUser(View view)
        {
            // Load data only after authentication succeeds.
            if (await azureService.Authenticate())
            {
                //Hide the button after authentication succeeds.
                FindViewById<Button>(GroceryMate.Resource.Id.buttonLoginUser).Visibility = ViewStates.Gone;

                //var found = await azureService.FindUser();
                //Console.WriteLine("was user found?: " + found);                 
            }
        }
        /*
         * hide keyboard after search
         * 
        public override bool OnTouchEvent(MotionEvent e)
        {
            InputMethodManager imm = (InputMethodManager)GetSystemService(InputMethodService);
            imm.HideSoftInputFromWindow(editText.WindowToken, 0);
            return base.OnTouchEvent(e);
        }
        */

    }

}
