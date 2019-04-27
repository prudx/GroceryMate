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
using GroceryMate.Helpers;
using GroceryMate.Model;
using GroceryMate.Resources.adapters;
using GroceryMate.Services;

namespace GroceryMate
{
    [Activity(Label = "@string/app_name", MainLauncher = false)]
    public class ReceiptActivity : Activity
    {
        ListView Receipts;
        ListView Items;
        EditText EditItemName;
        EditText EditItemPrice;
        Button EditItemSave;
        int EditItemID;
        ICollection<Receipt> ReceiptsCollection;
        ICollection<Item> ItemsForReceipt;
        AzureService azureService = new AzureService();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_receipt);

            Receipts = FindViewById<ListView>(Resource.Id.listViewReceipt);

            Items = FindViewById<ListView>(Resource.Id.listViewItem);

            EditItemName = FindViewById<EditText>(Resource.Id.editItem);
            EditItemPrice = FindViewById<EditText>(Resource.Id.editPrice);
            EditItemSave = FindViewById<Button>(Resource.Id.editSave);

            EditItemSave.Click += SaveClick;
            /*
            EditItemName.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) => {
                EditItemName.Text = e.Text.ToString();
            };
            EditItemPrice.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) => {
                queryString = e.Text.ToString();
            };*/

            ReceiptViewBuilder();
        }

        private async void SaveClick(object sender, EventArgs e)
        {
            Helper.CloseKeyboard();
            var item = EditItemName.Text;
            var price = Convert.ToDouble(EditItemPrice.Text);
            EditItemName.Text = "";
            EditItemPrice.Text = "";

            //hide editing from view
            FindViewById<EditText>(Resource.Id.editItem).Visibility = ViewStates.Gone;
            FindViewById<EditText>(Resource.Id.editPrice).Visibility = ViewStates.Gone;
            FindViewById<Button>(Resource.Id.editSave).Visibility = ViewStates.Gone;
            var updatedItem = await azureService.UpdateItem(item, price, EditItemID);
            EditItemID = new int();

            ItemViewBuilder(updatedItem.ReceiptId); //rebuild view
        }

        public async void ReceiptViewBuilder()
        {
            ReceiptsCollection = await azureService.GetReceiptsForUser();
            var receiptIds = new Collection<int>();

            foreach (Receipt r in ReceiptsCollection)
                receiptIds.Add(r.ReceiptId);

            //calculate our totals
            var totals = await azureService.GetTotalsForReceipts(receiptIds);

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
            menu.Show();
            menu.MenuItemClick += async (s, a) =>
            {
                switch (a.Item.ItemId)
                {
                    case Resource.Id.popup_edit:
                        menu.Menu.Dispose();
                        Toast.MakeText(this, "Please click into a receipt to edit.", ToastLength.Long).Show();
                        break;
                    case Resource.Id.popup_delete:
                        menu.Menu.Dispose();
                        await azureService.DeleteReceipt(ReceiptsCollection.ElementAt(e.Position).ReceiptId);
                        ReceiptViewBuilder();
                        break;
                }
            };
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
            menu.Show();
            menu.MenuItemClick += async (s, a) =>
            {
                switch (a.Item.ItemId)
                {
                    case Resource.Id.popup_edit:
                        menu.Menu.Dispose();
                        var itemEdit = ItemsForReceipt.ElementAt(e.Position);
                        ItemEdit(itemEdit.ItemId);
                        break;
                    case Resource.Id.popup_delete:
                        menu.Menu.Dispose();
                        var itemDelete = ItemsForReceipt.ElementAt(e.Position);
                        await azureService.DeleteItem(itemDelete.ItemId);
                        ItemViewBuilder(itemDelete.ReceiptId);    //remake view
                        break;
                }
            };
        }

        private void ItemEdit(int itemId)
        {
            FindViewById<EditText>(Resource.Id.editItem).Visibility = ViewStates.Visible;   //change headers
            FindViewById<EditText>(Resource.Id.editPrice).Visibility = ViewStates.Visible;
            FindViewById<Button>(Resource.Id.editSave).Visibility = ViewStates.Visible;

            EditItemID = itemId;
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
                FindViewById<EditText>(Resource.Id.editItem).Visibility = ViewStates.Gone;   //change headers
                FindViewById<EditText>(Resource.Id.editPrice).Visibility = ViewStates.Gone;
                FindViewById<Button>(Resource.Id.editSave).Visibility = ViewStates.Gone;
            }
            else
            {
                Finish();
            }
        }
    }
}