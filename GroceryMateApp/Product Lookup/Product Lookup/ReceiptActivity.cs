using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using GroceryMate;
using GroceryMate.Model;
using GroceryMate.Resources.adapters;
using GroceryMate.Services;

namespace GroceryMate
{
    [Activity(Label = "GroceryMate", Theme = "@style/Theme.AppCompat.Light.DarkActionBar", MainLauncher = false)]
    public class ReceiptActivity : Activity
    {
        ListView Receipts;
        ListView Items;
        ICollection<Receipt> ReceiptsCollection;
        ICollection<int> ReceiptIds = new Collection<int>();
        AzureService azureService = new AzureService();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_receipt);

            Receipts = FindViewById<ListView>(Resource.Id.listViewReceipt);

            Items = FindViewById<ListView>(Resource.Id.listViewItem);

            ReceiptViewBuilder();
        }

        
        public  async void ReceiptViewBuilder()
        {
            //try this using ProductSearch_Adapter
            //using get from azure
            ReceiptsCollection 
                = await azureService.GetReceiptsForUser();

            foreach (Receipt r in ReceiptsCollection)
                ReceiptIds.Add(r.ReceiptId);

            var totals
                = await azureService.GetTotalsForReceipts(ReceiptIds);

            var adapter = new ListViewReceipt_Adapter(this, ReceiptsCollection, totals); //CameraActivity.CapturedItems
            Receipts.Adapter = adapter;

            Receipts.ItemClick += ReceiptClick;
        }

        private async void ReceiptClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Receipts.Visibility = ViewStates.Gone;

            var receiptId = ReceiptsCollection.ElementAt(e.Position).ReceiptId;
            var items = await azureService.GetItemsForReceipt(receiptId);

            var adapter = new ListViewItem_Adapter(this, items);
            Items.Adapter = adapter;

            Console.WriteLine("normal");
        }
    }
}