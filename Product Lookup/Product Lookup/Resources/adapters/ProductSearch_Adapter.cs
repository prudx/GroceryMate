using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Icu.Text;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Product_Lookup.Model;

namespace Product_Lookup.Resources.adapters
{
    class ProductSearch_Adapter : BaseAdapter
    {
        class ViewHolder : Java.Lang.Object
        {
            public ImageView ProductSearchImg { get; set; }
            public TextView ProductSearchItemName { get; set; }
            public TextView ProductSearchItemPrice { get; set; }
        }

        private Activity activity;
        private List<Item> listProductSearchItems;

        public ProductSearch_Adapter(Activity ac, List<Item> i)
        {
            this.activity = ac;
            this.listProductSearchItems = i;
        }

        public override int Count
        {
            get
            {
                return listProductSearchItems.Count;
            }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }

        public override long GetItemId(int position)
        {
            return listProductSearchItems[position].Id;
        }

        private Bitmap GetImageBitmapFromUrl(string url)
        {
            Bitmap imageBitmap = null;

            using (var webClient = new WebClient())
            {
                var imageBytes = webClient.DownloadData(url);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
            }

            return imageBitmap;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? activity.LayoutInflater.Inflate(Resource.Layout.ProductSearchResult, parent, false);

            
            var ProductSearchImg = view.FindViewById<ImageView>(Resource.Id.productSearchImg);
            var ProductSearchItemName = view.FindViewById<TextView>(Resource.Id.productSearchItemName);
            var ProductSearchItemPrice = view.FindViewById<TextView>(Resource.Id.productSearchItemPrice);

            var imageBitmap = GetImageBitmapFromUrl(listProductSearchItems[position].Image);

            ProductSearchImg.SetImageBitmap(imageBitmap);
            ProductSearchItemName.Text = listProductSearchItems[position].Name;
            //ProductSearchItemPrice.Text = "€" + listProductSearchItems[position].Price; 
            ProductSearchItemPrice.Text 
                = listProductSearchItems[position].Price.ToString("€0.00", CultureInfo.InvariantCulture);

            return view;
        }
    }
}