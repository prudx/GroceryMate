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
using GroceryMate.Services;
using Microcharts;
using Microcharts.Droid;
using GroceryMate.Helpers;
using GroceryMate.API;
using Refit;
using GroceryMate.JsonData;
using System.Threading.Tasks;
using GroceryMate.Model;

namespace GroceryMate
{
    [Activity(Label = "@string/app_name")]
    public class ChartActivity : Activity
    {
        List<Entry> Entries;
        ChartView ChartTop;
        Spinner StoreAnalysis;
        Spinner UserGroup;
        Spinner ChartType;
        List<string> Colors;

        ChartView ChartBottom;
        Spinner ItemAnalysis;
        EditText SearchProduct;

        ITescoAPI tescoAPI;

        AzureService azureService = new AzureService();
        NativeFont font = new NativeFont();

        bool ForUser;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_chart);

            Colors = new List<string>() {"#266489","#68B9C0","#90D585","#FF1493","#42f477","#c741f4","#f4f141","#2718f9","#f91717","#ff7b00"};
            Entries = new List<Entry>();

            var temp = new Entry(100) { Label = "Default" };
            var tempList = new List<Entry>() { temp };

            ChartTop = FindViewById<ChartView>(Resource.Id.chartView1);
            ChartTop.Chart = new DonutChart() { Entries = tempList};

            ChartBottom = FindViewById<ChartView>(Resource.Id.chartView2);
            ChartBottom.Chart = new BarChart() { Entries = tempList };

            StoreAnalysis = FindViewById<Spinner>(Resource.Id.spinnerStore);
            UserGroup = FindViewById<Spinner>(Resource.Id.spinnerUserGroup);
            ChartType = FindViewById<Spinner>(Resource.Id.spinnerChart);

            ItemAnalysis = FindViewById<Spinner>(Resource.Id.spinnerItem);
            SearchProduct = FindViewById<EditText>(Resource.Id.searchProductChart);

            UserGroup.ItemSelected += UserGroupSelected;

            LaunchStoreAnalysis(ChartTop);
        }

        //Set var for azure querys
        private void UserGroupSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            if (UserGroup.SelectedItem.ToString() == GetString(Resource.String.you))
                ForUser = true;
            else if (UserGroup.SelectedItem.ToString() == GetString(Resource.String.all))
                ForUser = false;
        }

        private async void AverageSpendQuery()
        {
            List<KeyValuePair<string, double>> TotalAveragesForUniqueStores = await azureService.AverageSpend(ForUser);

            var entries = CreateEntries(TotalAveragesForUniqueStores);
            Entries = new List<Entry>();

            SelectChartTop(entries);
        }

        private async void MostVisitedQuery()
        {
            List<KeyValuePair<string, double>> TotalVisitsForUniqueStores = await azureService.MostVisited(ForUser);

            foreach (var kvp in TotalVisitsForUniqueStores)
            {
                Console.WriteLine(kvp);
            }

            var entries = CreateEntries(TotalVisitsForUniqueStores);
            Entries = new List<Entry>();


            SelectChartTop(entries);
        }

        
        private List<Entry> CreateEntries(List<KeyValuePair<string, double>> kvpl)
        {
            int colorCounter = 0; //asigns up to 10 colours
            foreach (KeyValuePair<string, double> kvp in kvpl)
            {
                var label = kvp.Key;
                var number = kvp.Value;
                string valLabel;

                //check if we want currency, or whole numbers displayed
                if (StoreAnalysis.SelectedItem.ToString() == GetString(Resource.String.mostVisted))
                    valLabel = String.Format("{0:0}", number);
                else
                    valLabel = String.Format("{0:0.00}", number);

                Entry e = new Entry((float)number)
                {
                    Label = label,
                    ValueLabel = valLabel,
                    Color = SkiaSharp.SKColor.Parse(Colors[colorCounter]),
                };

                colorCounter++;
                Entries.Add(e);
            }
            return Entries;
        }

        private void SelectChartTop(List<Entry> entries)
        {
            var chartType = ChartType.SelectedItem.ToString();

            if (chartType == GetString(Resource.String.donut))
                ChartTop.Chart = new DonutChart() { Entries = entries };
            else if (chartType == GetString(Resource.String.bar))
                ChartTop.Chart = new BarChart() { Entries = entries };
            else if (chartType == GetString(Resource.String.point))
                ChartTop.Chart = new PointChart() { Entries = entries };
            else if (chartType == GetString(Resource.String.line))
                ChartTop.Chart = new LineChart() { Entries = entries };
            else if (chartType == GetString(Resource.String.radial))
                ChartTop.Chart = new RadialGaugeChart() { Entries = entries };
            else if (chartType == GetString(Resource.String.radar))
                ChartTop.Chart = new RadarChart() { Entries = entries };

            ChartTop.Chart.LabelTextSize = font.GetNativeSize(10);
        }

        [Java.Interop.Export()]
        public void LaunchStoreAnalysis(View view)
        {
            if (StoreAnalysis.SelectedItem.ToString() == GetString(Resource.String.avgSpend))
                AverageSpendQuery();
            else if (StoreAnalysis.SelectedItem.ToString() == GetString(Resource.String.mostVisted))
                MostVisitedQuery();
        }

        [Java.Interop.Export()]
        public void LaunchItemAnalysis(View view)
        {
            if (ItemAnalysis.SelectedItem.ToString() == GetString(Resource.String.cheapest))
                CheapestQuery();
            Helper.CloseKeyboard();
        }

        // TESCO QUERIES
        private async void CheapestQuery()
        {
            string itemName = SearchProduct.Text.ToUpper();

            if (itemName == "")
                itemName = "CHOCOLATE";                

            KeyValuePair<string, double> CheapestAtStore = await azureService.Cheapest(itemName);

            Console.WriteLine(CheapestAtStore);

            KeyValuePair<string, double> CheapestAtTesco = await TescoQuery(itemName);

            var entries = CreateEntriesAgainstTesco(CheapestAtStore, CheapestAtTesco);
            Entries = new List<Entry>();    //empty the entries

            SelectChartBottom(entries);
        }

        private async Task<KeyValuePair<string, double>> TescoQuery(string itemName)
        {
            double price = 0;

            try
            {   
                tescoAPI = RestService.For<ITescoAPI>("https://dev.tescolabs.com");
                RootObject results = await tescoAPI.GetItems(itemName, 0, 1);

                if (results != null)
                    price = results.Uk.Ghs.Products.Results.ElementAt(0).Price;
                else
                    price = 0;                
            }
            catch(Exception ex)
            {
                Toast.MakeText(this, ex.ToString(), ToastLength.Long);
            }

            var CheapestAtTesco = new KeyValuePair<string, double>("TESCO", price);
            return CheapestAtTesco;
        }

        private List<Entry> CreateEntriesAgainstTesco(KeyValuePair<string, double> kvp, KeyValuePair<string, double> tescoKvp)
        {
            var label = kvp.Key;
            var number = kvp.Value;
            var T_label = tescoKvp.Key;
            var T_number = tescoKvp.Value;

            string T_Color, kvpColor;

            if (number > T_number)
                { T_Color = "#42f45c"; kvpColor = "#db0d0d"; } 
            else if (number == T_number)
                { T_Color = "#0dd7db"; kvpColor = "#0dd7db"; }
            else
            { T_Color = "#db0d0d"; kvpColor = "#42f45c"; }

            var e = new List<Entry>()
            {
                new Entry((float)number)
                {
                    Label = label,
                    ValueLabel = String.Format("{0:0.00}", number),
                    Color = SkiaSharp.SKColor.Parse(kvpColor),
                },
                new Entry((float)T_number)
                {
                    Label = T_label,
                    ValueLabel = String.Format("{0:0.00}", T_number),
                    Color = SkiaSharp.SKColor.Parse(T_Color),
                }
            };

            Entries = e;         
            return Entries;
        }

        private void SelectChartBottom(List<Entry> entries)
        {
            var chartType = ChartType.SelectedItem.ToString();

            if (chartType == GetString(Resource.String.donut))
                ChartBottom.Chart = new DonutChart() { Entries = entries };
            else if (chartType == GetString(Resource.String.bar))
                ChartBottom.Chart = new BarChart() { Entries = entries };
            else if (chartType == GetString(Resource.String.point))
                ChartBottom.Chart = new PointChart() { Entries = entries };
            else if (chartType == GetString(Resource.String.line))
                ChartBottom.Chart = new LineChart() { Entries = entries };
            else if (chartType == GetString(Resource.String.radial))
                ChartBottom.Chart = new RadialGaugeChart() { Entries = entries };
            else if (chartType == GetString(Resource.String.radar))
                ChartBottom.Chart = new RadarChart() { Entries = entries };

            ChartBottom.Visibility = ViewStates.Visible;
            ChartBottom.Chart.LabelTextSize = font.GetNativeSize(10);
        }
    }
}