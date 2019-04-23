using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Plugin.CurrentActivity;
using Product_Lookup.Model;
using GroceryMate.Helpers;
using static GroceryMate.Helpers.Helper;


namespace Product_Lookup.Services
{
    public class AzureService
    {
        

        MobileServiceClient Client { get; set; } = null;
        MobileServiceUser User { get; set; } = null;
        IMobileServiceSyncTable<User> userTable;
        IMobileServiceSyncTable<Receipt> receiptTable;
        IMobileServiceSyncTable<Item> itemTable;


        public async Task Initialize()
        {
            if (Client?.SyncContext?.IsInitialized ?? false)
                return;

            var appUrl = "https://grocerymate1.azurewebsites.net";

            Client = new MobileServiceClient(appUrl);

            var fileName = "groceryMateLocal1.db";                //MobileServiceClient.DefaultDatabasePath()
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
            ReadOnlyCollection<MobileServiceTableOperationError> syncErrors = null;

            await Initialize();

            try
            {
                if (!CrossConnectivity.Current.IsConnected)
                    return;

                await userTable.PullAsync("allItems", itemTable.CreateQuery());
                await receiptTable.PullAsync("allItems", itemTable.CreateQuery());
                await itemTable.PullAsync("allItems", itemTable.CreateQuery());


                await Client.SyncContext.PushAsync();                
            }
            catch (MobileServicePushFailedException exc)
            {
                if (exc.PushResult != null)
                {
                    syncErrors = exc.PushResult.Errors;
                    foreach (var error in syncErrors)
                    {
                        Console.WriteLine(error.ToString());
                        Console.WriteLine(error.RawResult);
                        Console.WriteLine(error.OperationKind);
                        Console.WriteLine(error.TableName);
                        Console.WriteLine(error.Status);
                        Console.WriteLine(error.Item);

                    }
                }
                /*
                if (syncErrors != null)
                {
                    foreach (var error in syncErrors)
                    {
                        if (error.OperationKind == MobileServiceTableOperationKind.Update && error.Result != null)
                        {
                            //Update failed, reverting to server's copy.
                            await error.CancelAndUpdateItemAsync(error.Result);
                        }
                        else
                        {
                            // Discard local change.
                            await error.CancelAndDiscardItemAsync();
                        }

                        Console.WriteLine(@"Error executing sync operation. Item: {0} ({1}). Operation discarded.", error.TableName, error.Item["id"]);
                    }
                }
                */
            }
        }

        public async Task<IEnumerable<Item>> GetItems()
        {
            await Initialize();
            await SyncTables();

            var data = await itemTable
                .OrderBy(i => i.CreatedAt)
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
                //UserId = "10",
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
                UserId = 1,
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
                ReceiptId = 1, //works using longstring ID but not actual ReceiptId
                Name = "grape",
                Price = 1.00
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


        // Define an authenticated user.
        
        public async Task<bool> Authenticate()
        {
            Activity currentActivity = CrossCurrentActivity.Current.Activity; //get current activity

            var success = false;
            try
            {
                // Sign in with Facebook login using a server-managed flow.
                User = await Client.LoginAsync(currentActivity,
                    MobileServiceAuthenticationProvider.Google, "grocerymate");
                CreateAlert(AlertType.Info, string.Format("you are now logged in - {0}",
                    User.UserId), "Logged in!");

                success = true;
            }
            catch (Exception ex)
            {
                CreateAlert(AlertType.Error, ex.ToString(), "Authentication Error");
            }
            return success;
        }

        
    }
}