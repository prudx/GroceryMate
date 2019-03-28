using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Plugin.Connectivity;
using Product_Lookup.Model;

namespace Product_Lookup.Services
{
    public class AzureService
    {
        MobileServiceClient client = null;

        IMobileServiceSyncTable<Item> itemTable;



        public async Task Initialize()
        {
            if (client?.SyncContext?.IsInitialized ?? false)
                return;

            var appUrl = "http://grocerypal.azurewebsites.net";

            client = new MobileServiceClient(appUrl);

            var fileName = "groceryMate.db";                //MobileServiceClient.DefaultDatabasePath()

            var store = new MobileServiceSQLiteStore(fileName);

            //define as many tables as you want
            store.DefineTable<Item>();

            await client.SyncContext.InitializeAsync(store);

            itemTable = client.GetSyncTable<Item>();
        }

        //change to sync tables?
        public async Task SyncItems()
        {
            await Initialize();

            try
            {
                if (!CrossConnectivity.Current.IsConnected)
                    return;

                await client.SyncContext.PushAsync();

                await itemTable.PullAsync("allItems", itemTable.CreateQuery());
            }
            catch(Exception ex)
            {
                Console.WriteLine("error syncing items " + ex);
            }
        }

        public async Task<IEnumerable<Item>> GetItems()
        {
            await Initialize();
            await SyncItems();

            var data = itemTable
                .OrderBy(i => i.Id)
                .ToEnumerableAsync();

            //different to james code
            return await data;
        }

        public async Task<Item> AddItem(string name, double price)
        {
            await Initialize();

            //not saving image
            var item = new Item
            {
                Name = name,
                Price = price
            };

            await itemTable.InsertAsync(item);

            await SyncItems();

            return item;
        }

        //not used yet
        public Task<bool> LoginAsync()
        {
            return null;
        }

        //not used yet
        public static bool UseAuth { get; set; } = false;
    }
}