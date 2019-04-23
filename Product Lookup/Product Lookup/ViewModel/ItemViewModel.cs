using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmHelpers;
using Product_Lookup.Model;
using Product_Lookup.Services;
using Product_Lookup.Helpers;

namespace Product_Lookup.ViewModel
{
    //MOSTLY IGNORE THIS CLASS
    public class ItemViewModel : BaseViewModel
    {
        public ObservableRangeCollection<Item> Items { get; } = new ObservableRangeCollection<Item>();
        public ObservableRangeCollection<Grouping<string, Item>> ItemsGrouped { get; } = new ObservableRangeCollection<Grouping<string, Item>>();

        AzureService azureService = new AzureService();

        public ItemViewModel()
        {
            //azureService = DependencyService.Get<AzureService>();
        }
        

        string loadingMessage;
        public string LoadingMessage
        {
            get { return loadingMessage; }
            set { SetProperty(ref loadingMessage, value); }
        }


        string name;
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }


        double price;
        public double Price
        {
            get => price;
            set => SetProperty(ref price, value);
        }

        
        /*  FORMS SPECIFIC
        ICommand loadItemsCommand;
        public ICommand LoadItemsCommand =>
            loadItemsCommand ?? (loadItemsCommand = new Command(async () => await ExecuteLoadItemsCommandAsync()));
        */

        /*
        async Task ExecuteLoadItemsCommandAsync()
        {
            if (IsBusy || !(await LoginAsync()))
                return;

            try
            {
                LoadingMessage = "Loading Items...";
                IsBusy = true;
                var items = await azureService.GetItems();
                Items.ReplaceRange(items);

                SortItems();
            }
            catch (Exception ex)
            {
                Console.WriteLine("problem loading items " + ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
        

        void SortItems()
        {
            var groups = from item in Items
                         orderby item.Id descending
                         group item by Items.FirstOrDefault()
                into itemGroup
                         select new Grouping<string, Item>($"{itemGroup.Key} ({itemGroup.Count()})", itemGroup);


            ItemsGrouped.ReplaceRange(groups);
        }
        */



        /*  FORMS SPECIFIC
        ICommand addItemCommand;
        public ICommand AddItemCommand =>
            addItemCommand ?? (addItemCommand = new Command(async () => await ExecuteAddItemCommandAsync()));
        */


        /*
        async Task ExecuteAddItemCommandAsync()
        {
            if (IsBusy || !(await LoginAsync()))
                return;

            try
            {

                if (string.IsNullOrWhiteSpace(Name))
                    return;

                LoadingMessage = "Adding item...";
                IsBusy = true;

     
                var item = await azureService.AddItem(Name, Price);
                Name = string.Empty;
                Price = 0.00;
                Items.Add(item);
                SortItems();
            }
            catch (Exception ex)
            {
                Console.WriteLine("OH NO!" + ex);
            }
            finally
            {
                LoadingMessage = string.Empty;
                IsBusy = false;
            }

        }
        */


        /*
        //  FOR AUTHENTICATION
        public Task<bool> LoginAsync()
        {
            if (Settings.IsLoggedIn)
                return Task.FromResult(true);


            return azureService.LoginAsync();
        }
        */
    }
}