using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using Android.Views;
using Android.Views.InputMethods;
using Product_Lookup.API;
using Product_Lookup.JsonData;
using Refit;
using EDMTDialog;
using System.Collections.Generic;
using System;
using Android.Content;

namespace Product_Lookup
{
    [Activity(Label = "MainActivity", Theme = "@style/Theme.AppCompat.Light.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {

        Button btn_SearchProducts;
        Button btn_Camera;
        ListView list_Products;
        EditText editText;
        string queryString;

        ITescoAPI tescoAPI;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            btn_SearchProducts = FindViewById<Button>(Resource.Id.btn_SearchProduct);
            btn_Camera = FindViewById<Button>(Resource.Id.btn_Camera);

            list_Products = FindViewById<ListView>(Resource.Id.list_Products);

            editText = FindViewById<EditText>(Resource.Id.queryInput);
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

                    RootObject results = await tescoAPI.GetUsers(queryString, 0, 10);
                    List<string> result_name = new List<string>();
                    
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

            btn_Camera.Click += (s, e) =>
            {
                Intent cameraActivity = new Intent(this, typeof(CameraActivity));
                StartActivity(cameraActivity);
            };
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
