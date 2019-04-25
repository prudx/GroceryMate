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
using GroceryMate.Model;

namespace GroceryMate.Resources.adapters
{
    class ListViewReceipt_Adapter : BaseAdapter
    {
        class ViewHolder : Java.Lang.Object
        {
            public TextView ReceiptDate { get; set; }
            public TextView ReceiptName { get; set; }
            public TextView ReceiptTotal { get; set; }
        }

        private Activity activity;
        private ICollection<Receipt> listReceipt;
        private ICollection<double> total;

        public ListViewReceipt_Adapter(Activity ac, ICollection<Receipt> receipts, ICollection<double> totalPrice)
        {
            this.activity = ac;
            this.listReceipt = receipts;
            this.total = totalPrice;
        }

        public override int Count
        {
            get
            {
                return listReceipt.Count;
            }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }

        //not currently used so convert isn't bothering me
        public override long GetItemId(int position)
        {
            return listReceipt.ElementAt(position).ReceiptId;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? activity.LayoutInflater.Inflate(Resource.Layout.ListViewReceipt, parent, false);

            var ReceiptName = view.FindViewById<TextView>(Resource.Id.receiptName);
            var ReceiptDate = view.FindViewById<TextView>(Resource.Id.receiptDate);
            var ReceiptTotal = view.FindViewById<TextView>(Resource.Id.receiptTotal);

            double totalPrice = total.ElementAt(position);

            ReceiptName.Text = listReceipt.ElementAt(position).StoreName;
            ReceiptDate.Text = listReceipt.ElementAt(position).CreatedAt.UtcDateTime.ToShortDateString();

            if (!totalPrice.ToString().Contains("."))
                ReceiptTotal.Text = totalPrice+".00";
            else
                ReceiptTotal.Text = totalPrice.ToString();

            //ItemPrice.Text = "" + listReceipt.ElementAt(position).;

            return view;
        }
    }
}