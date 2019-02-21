using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System.Collections.Generic;
using AndroidDatabaseApp.Resources.Model;
using AndroidDatabaseApp.Resources.DataHelper;
using AndroidDatabaseApp.Resources;
using Android.Util;

namespace AndroidDatabaseApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        ListView listData;
        List<Person> FirstSouce = new List<Person>();
        Database db;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            //create db
            db = new Database();
            db.CreateDatabase();
            string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            Log.Info("DB_PATH", folder);


            listData = FindViewById<ListView>(Resource.Id.listView);

            var editName = FindViewById<EditText>(Resource.Id.editName);
            var editAge = FindViewById<EditText>(Resource.Id.editAge);
            var editEmail = FindViewById<EditText>(Resource.Id.editEmail);

            var btnAdd = FindViewById<Button>(Resource.Id.btnAdd);
            var btnEdit = FindViewById<Button>(Resource.Id.btnEdit);
            var btnDelete = FindViewById<Button>(Resource.Id.btnDelete);

            LoadData();


            btnAdd.Click += delegate
            {
                Person person = new Person()
                {
                    Name = editName.Text,
                    Age = int.Parse(editAge.Text),
                    Email = editEmail.Text
                };
                db.InsertIntoTablePerson(person);
                LoadData();
            };

            btnEdit.Click += delegate
            {
                Person person = new Person()
                {
                    Id = int.Parse(editName.Tag.ToString()),
                    Name = editName.Text,
                    Age = int.Parse(editAge.Text),
                    Email = editEmail.Text
                };
                db.UpdateTablePerson(person);
                LoadData();
            };

            btnDelete.Click += delegate
            {
                Person person = new Person()
                {
                    Id = int.Parse(editName.Tag.ToString()),
                    Name = editName.Text,
                    Age = int.Parse(editAge.Text),
                    Email = editEmail.Text
                };
                db.DeleteTablePerson(person);
                LoadData();
            };

            listData.ItemClick += (s, e) =>
            {
                for(int i = 0; i < listData.Count; i++)
                {
                    if (e.Position == i)
                        listData.GetChildAt(i).SetBackgroundColor(Android.Graphics.Color.DarkGray);
                    else
                        listData.GetChildAt(i).SetBackgroundColor(Android.Graphics.Color.Transparent);

                }

                var TxtName = e.View.FindViewById<TextView>(Resource.Id.textView1);
                var TxtAge = e.View.FindViewById<TextView>(Resource.Id.textView2);
                var TxtEmail = e.View.FindViewById<TextView>(Resource.Id.textView3);

                editName.Text = TxtName.Text;
                editName.Tag = e.Id;

                editAge.Text = TxtAge.Text;

                editEmail.Text = TxtEmail.Text;
            };
        }

        private void LoadData()
        {
            FirstSouce = db.SelectTablePerson();
            var adapter = new ListViewAdapter(this, FirstSouce);
            listData.Adapter = adapter;
        }
    }
}