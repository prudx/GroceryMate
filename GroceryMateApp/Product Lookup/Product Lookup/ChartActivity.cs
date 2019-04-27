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

namespace GroceryMate
{
    [Activity(Label = "@string/app_name")]
    public class ChartActivity : Activity
    {
        List<Entry> Entries;
        ChartView Chart;
        Spinner StoreAnalysis;
        Spinner UserGroup;
        Spinner ChartType;
        List<string> Colors; 

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

            Chart = FindViewById<ChartView>(Resource.Id.chartView1);
            Chart.Chart = new DonutChart() { Entries = tempList};
            

            StoreAnalysis = FindViewById<Spinner>(Resource.Id.spinnerStore);
            UserGroup = FindViewById<Spinner>(Resource.Id.spinnerUserGroup);
            ChartType = FindViewById<Spinner>(Resource.Id.spinnerChart);


            UserGroup.ItemSelected += UserGroupSelected;
            //StoreAnalysis.ItemSelected += AnalysisTypeSelected;
        }

        //Set var for azure querys
        private void UserGroupSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            if (UserGroup.SelectedItem.ToString() == GetString(Resource.String.you))
                ForUser = true;
            else if (UserGroup.SelectedItem.ToString() == GetString(Resource.String.all))
                ForUser = false;
        }

        /*Dont use, launch query from button
        private void AnalysisTypeSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            

        }
        */

        private async void AverageSpendQuery()
        {
            List<KeyValuePair<string, double>> TotalAveragesForUniqueStores = await azureService.AverageSpend(ForUser);

            foreach(var kvp in TotalAveragesForUniqueStores)
            {
                Console.WriteLine(kvp);
            }

            var entries = CreateEntries(TotalAveragesForUniqueStores);
            Entries = new List<Entry>();


            SelectChart(entries);
        }

        private List<Entry> CreateEntries(List<KeyValuePair<string, double>> kvpl)
        {
            int colorCounter = 0; //asigns up to 10 colours
            foreach (KeyValuePair<string, double> kvp in kvpl)
            {
                var label = kvp.Key;
                var number = kvp.Value;

                Entry e = new Entry((float)number)
                {
                    Label = label,
                    ValueLabel = String.Format("{0:0.00}", number),
                    Color = SkiaSharp.SKColor.Parse(Colors[colorCounter]),
                };

                colorCounter++;
                Entries.Add(e);
            }
            return Entries;
        }

        private void SelectChart(List<Entry> entries)
        {
            var chartType = ChartType.SelectedItem.ToString();

            if (chartType == GetString(Resource.String.donut))
                Chart.Chart = new DonutChart() { Entries = entries };
            else if (chartType == GetString(Resource.String.bar))
                Chart.Chart = new BarChart() { Entries = entries };
            else if (chartType == GetString(Resource.String.point))
                Chart.Chart = new PointChart() { Entries = entries };
            else if (chartType == GetString(Resource.String.line))
                Chart.Chart = new LineChart() { Entries = entries };
            else if (chartType == GetString(Resource.String.radial))
                Chart.Chart = new RadialGaugeChart() { Entries = entries };
            else if (chartType == GetString(Resource.String.radar))
                Chart.Chart = new RadarChart() { Entries = entries };

            Chart.Chart.LabelTextSize = font.GetNativeSize(10);
        }

  

        [Java.Interop.Export()]
        public void LaunchStoreAnalysis(View view)
        {
            if (StoreAnalysis.SelectedItem.ToString() == GetString(Resource.String.avgSpend))
                AverageSpendQuery();    
            //else if (StoreAnalysis.SelectedItem.ToString() == GetString(Resource.String.mostVisted))
        }       
    }
}