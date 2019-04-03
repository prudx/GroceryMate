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

namespace Product_Lookup.Resources.adapters
{
    class ListViewItem_Adapter : BaseAdapter
    {
        class ViewHolder : Java.Lang.Object
        {
            public TextView ItemName { get; set; }
            public TextView ItemPrice { get; set; }
        }

        private Activity activity;
        private List<Item> listReceiptItems;

        public ListViewItem_Adapter(Activity ac, List<Item> i)
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

        //not currently used so convert isn't bothering me
        public override long GetItemId(int position)
        {
            return Convert.ToInt64(listReceiptItems[position].ItemId);
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? activity.LayoutInflater.Inflate(Resource.Layout.ListViewReceipt, parent, false);

            var ItemName = view.FindViewById<TextView>(Resource.Id.itemName);
            var ItemPrice = view.FindViewById<TextView>(Resource.Id.itemPrice);
            

            ItemName.Text = listReceiptItems[position].Name;
            ItemPrice.Text = "" + listReceiptItems[position].Price;

            return view;
        }
    }
}