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
using AlertDialog = Android.Support.V7.App.AlertDialog;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace GroceryMate
{
    [Activity(Label = "@string/app_name", Theme = "@style/Theme.AppCompat.Light.DarkActionBar", Icon ="@mipmap/ic_launcher", MainLauncher = true)]
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
            AppCenter.Start("e09f8279-4d9e-464c-b9a2-e5df32e1a0f8", typeof(Analytics), typeof(Crashes));

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

            //SEARCH BUTTON
            btn_SearchProducts.Click += delegate
            {
                string searchMessage = Resources.GetText(Resource.String.searchingFor);
                Helper.CloseKeyboard();

                AlertDialog dialog = new EDMTDialogBuilder()
               .SetContext(this)
               .SetMessage(searchMessage +" " +queryString)
               .Build();

                if (!dialog.IsShowing)
                    dialog.Show();

                SearchProducts(queryString, dialog);
            };

            //CAMERA BUTTON
            btn_Camera.Click += (s, e) =>
            {
                Intent cameraActivity = new Intent(this, typeof(CameraActivity));
                StartActivity(cameraActivity);
            };
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            var inflater = MenuInflater;
            inflater.Inflate(Resource.Menu.menu1, menu);
            return true;
        }


        //API SEARCH REQUEST
        public async void SearchProducts(string queryString, AlertDialog dialog)
        {
            try
            {
                RootObject results = await tescoAPI.GetItems(queryString, 0, 16);
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

                var adapter = new ProductSearch_Adapter(this, resultList);
                list_Products.Adapter = adapter;

                if (dialog.IsShowing)
                    dialog.Dismiss();
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, "" + ex.Message, ToastLength.Long).Show();
            }      
        }

        [Java.Interop.Export()]
        public async void LoginUser(View view)
        {
            // Load data only after authentication succeeds.
            if (await azureService.Authenticate())
            {
                //Hide the button after authentication succeeds.
                FindViewById<Button>(Resource.Id.buttonLoginUser).Visibility = ViewStates.Gone;
                FindViewById<Button>(Resource.Id.btn_Receipts).Visibility = ViewStates.Visible;
                FindViewById<Button>(Resource.Id.btn_Graphs).Visibility = ViewStates.Visible;


                //var found = await azureService.FindUser();
                //Console.WriteLine("was user found?: " + found); 
                //FindViewById<Button>(GroceryMate.Resource.Id.btnHistory).Visibility = ViewStates.Gone;
            }
        }

        [Java.Interop.Export()]
        public void StartReceipts(View view)
        {
            Intent receiptActivity = new Intent(this, typeof(ReceiptActivity));
            StartActivity(receiptActivity);
        }
        
    }
}
