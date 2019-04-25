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
using GroceryMate.Model;
using GroceryMate.Helpers;
using static GroceryMate.Helpers.Helper;
using GroceryMate.Authentication;

namespace GroceryMate.Services
{
    public class AzureService
    {
        

        public MobileServiceClient Client { get; set; } = null;
        MobileServiceUser User { get; set; } = null;
        IMobileServiceSyncTable<User> userTable;
        IMobileServiceSyncTable<Receipt> receiptTable;
        IMobileServiceSyncTable<Item> itemTable;

        public static bool UseAuth { get; set; } = true;

        public async Task Initialize()
        {
            if (Client?.SyncContext?.IsInitialized ?? false)
                return;

            var appUrl = "https://grocerymate1.azurewebsites.net";


            Client = new MobileServiceClient(appUrl, new AuthHandler());

            if (!string.IsNullOrWhiteSpace(Settings.AuthToken) && !string.IsNullOrWhiteSpace(Settings.UserSid))
            {
                Client.CurrentUser = new MobileServiceUser(Settings.UserSid);
                Client.CurrentUser.MobileServiceAuthenticationToken = Settings.AuthToken;
            }
            
            //unauthorized version
            //Client = new MobileServiceClient(appUrl);

            var fileName = "groceryMateLocal6.db";                //MobileServiceClient.DefaultDatabasePath()
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

                await userTable.PullAsync("allItems", userTable.CreateQuery());
                await receiptTable.PullAsync("allItems", receiptTable.CreateQuery());
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
                
            }
        }
        public async Task<bool> FindUser()
        {
            await Initialize();
            await SyncTables();

            var data = await userTable
                .Where(c => c.UserId == Settings.UserSid)
                .ToListAsync();
              
            foreach (var u in data)
            {
                Console.WriteLine(u.Id.ToString());
            }

            bool found = false;
            if(data.Count == 0)
            {
                found = false;
            } else if(data.First().UserId == Settings.UserSid)
            {
                found = true;
            }
            //different to james code
            return found;
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
        public async Task<User> AddUser()
        {
            await Initialize();

            var user = new User
            {
                Id = Settings.UserSid,
                UserId = Settings.UserSid            
            };
            
            await userTable.InsertAsync(user);

            await SyncTables();

            return user;
        }

        public async Task<int> CountReceipts()
        {
            await Initialize();
            await SyncTables();

            var data = await receiptTable
                .Where(c => c.Id != null)
                .ToListAsync();

            int count = 0;
            if (data.Count != 0)
                count = data.Count;

            return count;
        }

        public async Task<Receipt> AddReceipt(string storeName, ICollection<Item> items)
        {
            await Initialize();

            int receiptId = await CountReceipts();

            
            foreach(Item it in items)
            {
                await AddItem(it.Name, it.Price, receiptId);
            }
            

            var receipt = new Receipt
            {
                Id = "R"+receiptId,
                ReceiptId = receiptId,
                StoreName = storeName,
                //Items = items,
                UserId = Settings.UserSid                
            };

            await receiptTable.InsertAsync(receipt);

            await SyncTables();

            return receipt;
        }

        public async Task<int> CountItems()
        {
            await Initialize();
            await SyncTables();

            var data = await itemTable
                .Where(c => c.Id != null)
                .ToListAsync();

            int count = 0;
            if (data.Count != 0)
                count = data.Count;

            return count;
        }

        public async Task<Item> AddItem(string name, double price, int receiptId)
        {
            await Initialize();

            int itemId = await CountItems();

            var item = new Item
            {
                Id = "I"+itemId,
                ItemId = itemId,
                Name = name,
                Price = price,
                ReceiptId = receiptId
            };

            await itemTable.InsertAsync(item);

            await SyncTables();

            return item;
        }

        //not used yet
        /*
        public Task<bool> LoginAsync()
        {
            return null;
        }
        */    



        // Define an authenticated user.
        public async Task<bool> Authenticate()
        {
            Activity currentActivity = CrossCurrentActivity.Current.Activity; //get current activity
            await Initialize();

            var success = false;
            try
            {
                // Sign in with Facebook login using a server-managed flow.
                User = await Client.LoginAsync(currentActivity.ApplicationContext,
                    MobileServiceAuthenticationProvider.Google, "grocerymate");

                CreateAlert(AlertType.Info, string.Format("you are now logged in - {0}",
                    User.UserId), "Logged in!");

                success = true;
                
                //Set up the settings vars
                if (User == null)
                {
                    Settings.AuthToken = string.Empty;
                    Settings.UserSid = string.Empty;

                    success = false;
                }
                else
                {
                    Settings.AuthToken = User.MobileServiceAuthenticationToken;
                    Settings.UserSid = User.UserId;
                    
                    success = true;

                    //create new user if user doesn't exist in db
                    if (await FindUser() == false)
                        await AddUser();                   
                }
            }
            catch (Exception ex)
            {
                CreateAlert(AlertType.Error, ex.ToString(), "Authentication Error");
            }

            return success;
        }
        
        
    }
}