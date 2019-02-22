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
using Product_Lookup.Model;
using Product_Lookup.Resources.adapters;

namespace Product_Lookup
{
    [Activity(Label = "ReceiptActivity")]
    public class ReceiptActivity : Activity
    {
        ListView ReceiptItems;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_receipt);

            ReceiptItems = FindViewById<ListView>(Resource.Id.listViewReceipt);

            ReceiptBuilder();
            // Create your application here
        }

        
        public void ReceiptBuilder()
        {
            var adapter = new ListViewItem_Adapter(this, CameraActivity.CapturedItems);
            ReceiptItems.Adapter = adapter;
        }
        
    }
}