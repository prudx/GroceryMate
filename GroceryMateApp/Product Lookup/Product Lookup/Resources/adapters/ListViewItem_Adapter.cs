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
using GroceryMate;
using GroceryMate.Model;

namespace GroceryMate.Resources.adapters
{
    class ListViewItem_Adapter : BaseAdapter
    {
        class ViewHolder : Java.Lang.Object
        {
            public TextView ItemName { get; set; }
            public TextView ItemPrice { get; set; }
        }

        private Activity activity;
        private ICollection<Item> listReceiptItems;

        public ListViewItem_Adapter(Activity ac, ICollection<Item> i)
        {
            this.activity = ac;
            this.listReceiptItems = i;
        }

        public override int Count
        {
            get
            {
                return listReceiptItems.Count;
            }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }

        public override long GetItemId(int position)
        {
            return listReceiptItems.ElementAt(position).ItemId;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? activity.LayoutInflater.Inflate(Resource.Layout.ListViewItem, parent, false);

            var ItemName = view.FindViewById<TextView>(Resource.Id.itemName);
            var ItemPrice = view.FindViewById<TextView>(Resource.Id.itemPrice);
            
            
            ItemName.Text = listReceiptItems.ElementAt(position).Name;
            ItemPrice.Text = "" + listReceiptItems.ElementAt(position).Price;

            return view;
        }
    }
}