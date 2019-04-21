using System;
using System.Collections.Generic;
using System.IO;
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
        MobileServiceClient Client { get; set; } = null;
        IMobileServiceSyncTable<User> userTable;
        IMobileServiceSyncTable<Receipt> receiptTable;
        IMobileServiceSyncTable<Item> itemTable;



        public async Task Initialize()
        {
            if (Client?.SyncContext?.IsInitialized ?? false)
                return;

            var appUrl = "https://grocerymate1.azurewebsites.net";

            Client = new MobileServiceClient(appUrl);

            var fileName = "groceryMateNew.db";                //MobileServiceClient.DefaultDatabasePath()
            fileName = Path.Combine(MobileServiceClient.DefaultDatabasePath, fileName);
    
            var store = new MobileServiceSQLiteStore(fileName);


            //define as many tables as you want
            store.DefineTable<User>();
            store.DefineTable<Receipt>();
            store.DefineTable<Item>();

            await Client.SyncContext.InitializeAsync(store);

            userTable = Client.GetSyncTable<User>();
            receiptTable = Client.GetSyncTable<Receipt>();
            itemTable = Client.GetSyncTable<Item>();
        }

        //change to sync tables?
        public async Task SyncTables()
        {
            await Initialize();

            try
            {
                if (!CrossConnectivity.Current.IsConnected)
                    return;

                await Client.SyncContext.PushAsync();

                await userTable.PullAsync("allItems", itemTable.CreateQuery());
                await receiptTable.PullAsync("allItems", itemTable.CreateQuery());
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
            await SyncTables();

            var data = await itemTable
                .OrderBy(i => i.ItemId)
                .ToEnumerableAsync();

            //different to james code
            return data;
        }


        //take in SID when creating user
        public async Task<User> AddUser(string name)
        {
            await Initialize();

            var user = new User
            {
                Id = "10",
                UserId = "10",
                Name = name
            };

            await userTable.InsertAsync(user);

            await SyncTables();

            return user;
        }

        public async Task<Receipt> AddReceipt(string storeName, List<Item> items)
        {
            await Initialize();

            var receipt = new Receipt
            {
                UserId = "10",
                ReceiptId = "5",
                StoreName = storeName,
                Items = items
            };

            await receiptTable.InsertAsync(receipt);

            await SyncTables();

            return receipt;
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

            await SyncTables();

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