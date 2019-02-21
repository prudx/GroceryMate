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
using AndroidDatabaseApp.Resources.Model;
using Java.Lang;

namespace AndroidDatabaseApp.Resources
{
    public class ViewHolder : Java.Lang.Object
    {
        public TextView TxtName { get; set; }
        public TextView TxtAge { get; set; }
        public TextView TxtEmail { get; set; }
    }

    public class ListViewAdapter : BaseAdapter
    {
        private Activity activity;
        private List<Person> listPerson;

        public ListViewAdapter(Activity ac, List<Person> p) {
            this.activity = ac;
            this.listPerson = p;
        }

        public override int Count
        {
            get
            {
                return listPerson.Count;
            }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }

        public override long GetItemId(int position)
        {
            return listPerson[position].Id;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? activity.LayoutInflater.Inflate(Resource.Layout.listView_DataTemplate, parent, false);

            var TxtName = view.FindViewById<TextView>(Resource.Id.textView1);
            var TxtAge = view.FindViewById<TextView>(Resource.Id.textView2);
            var TxtEmail = view.FindViewById<TextView>(Resource.Id.textView3);

            TxtName.Text = listPerson[position].Name;
            TxtAge.Text = ""+listPerson[position].Age;
            TxtEmail.Text = listPerson[position].Email;

            return view;
        }
    }
}