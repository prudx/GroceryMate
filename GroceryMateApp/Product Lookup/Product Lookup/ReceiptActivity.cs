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
    [Activity(Label = "@string/app_name", Theme = "@style/Theme.AppCompat.Light.DarkActionBar", MainLauncher = false)]
    public class ReceiptActivity : Activity
    {
        ListView Receipts;
        ListView Items;
        ICollection<Receipt> ReceiptsCollection;
        ICollection<Item> ItemsForReceipt;
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

        
        public async void ReceiptViewBuilder()
        {
            ReceiptsCollection = await azureService.GetReceiptsForUser();

            foreach (Receipt r in ReceiptsCollection)
                ReceiptIds.Add(r.ReceiptId);

            //calculate our totals
            var totals = await azureService.GetTotalsForReceipts(ReceiptIds);

            var adapter = new ListViewReceipt_Adapter(this, ReceiptsCollection, totals); //CameraActivity.CapturedItems
            Receipts.Adapter = adapter;

            Receipts.ItemClick += ReceiptClick;
            Receipts.ItemLongClick += ReceiptLongClick;

            //not sure what to implement for this method
            //Items.ItemClick += ItemClick;    
            Items.ItemLongClick += ItemLongClick;
        }

        private async void ItemViewBuilder(int receiptId)
        {
            FindViewById<TextView>(Resource.Id.textStore).Visibility = ViewStates.Gone;   //change headers
            FindViewById<TextView>(Resource.Id.textDate).Visibility = ViewStates.Gone;
            FindViewById<TextView>(Resource.Id.textItem).Visibility = ViewStates.Visible;

            ItemsForReceipt = await azureService.GetItemsForReceipt(receiptId);

            if (ItemsForReceipt.Count != 0)
            {
                Items.Visibility = ViewStates.Visible;
                var adapter = new ListViewItem_Adapter(this, ItemsForReceipt);
                Items.Adapter = adapter;
            } else
            {
                await azureService.DeleteReceipt(receiptId);
                OnBackPressed();
            }      
        }

        private void ReceiptClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Receipts.Visibility = ViewStates.Gone;

            var receiptId = ReceiptsCollection.ElementAt(e.Position).ReceiptId;

            ItemViewBuilder(receiptId);
        }

        private void ReceiptLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            PopupMenu menu = new PopupMenu(this, Receipts.GetChildAt(e.Position));
            menu.MenuInflater.Inflate(Resource.Menu.popup_menu, menu.Menu);
            menu.MenuItemClick += async (s, a) =>
            {
                switch (a.Item.ItemId)
                {
                    case Resource.Id.popup_edit:
                        menu.Dismiss();
                        
                        break;
                    case Resource.Id.popup_delete:
                        menu.Dismiss();
                        await azureService.DeleteReceipt(ReceiptsCollection.ElementAt(e.Position).ReceiptId);
                        ReceiptViewBuilder();
                        break;
                }
            };
            menu.Show();
        }

        /*
        private void ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            PopupMenu menu = new PopupMenu(this, Items);
            menu.MenuInflater.Inflate(Resource.Menu.popup_menu, menu.Menu);
        }*/

        private void ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            PopupMenu menu = new PopupMenu(this, Items.GetChildAt(e.Position));
            menu.MenuInflater.Inflate(Resource.Menu.popup_menu, menu.Menu);
            menu.MenuItemClick += async (s, a) =>
            {
                switch (a.Item.ItemId)
                {
                    case Resource.Id.popup_edit:

                        break;
                    case Resource.Id.popup_delete:
                        menu.Dismiss();
                        var item = ItemsForReceipt.ElementAt(e.Position);
                        await azureService.DeleteItem(item.ItemId);
                        ItemViewBuilder(item.ReceiptId);    //remake view
                        break;
                }
            };
            menu.Show();
        }

        public override void OnBackPressed()
        {
            if(Items.Visibility == ViewStates.Visible)
            {
                Items.Visibility = ViewStates.Gone;
                ReceiptViewBuilder();
                Receipts.Visibility = ViewStates.Visible;
                FindViewById<TextView>(Resource.Id.textStore).Visibility = ViewStates.Visible;   //change headers
                FindViewById<TextView>(Resource.Id.textDate).Visibility = ViewStates.Visible;
                FindViewById<TextView>(Resource.Id.textItem).Visibility = ViewStates.Gone;
            }
            else
            {
                Finish();
            }
        }
    }
}