#define AUTH
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
#if AUTH
        public static bool UseAuth { get; set; }
#else
        public static bool UseAuth { get; set; } = false;
#endif
        public MobileServiceClient Client { get; set; } = null;
        MobileServiceUser User { get; set; } = null;
        IMobileServiceSyncTable<User> userTable;
        IMobileServiceSyncTable<Receipt> receiptTable;
        IMobileServiceSyncTable<Item> itemTable;

        //INITIALIZATION

        public async Task Initialize()
        {
            if (Client?.SyncContext?.IsInitialized ?? false)
                return;

            var appUrl = "https://grocerymate1.azurewebsites.net";

#if AUTH
            Client = new MobileServiceClient(appUrl, new AuthHandler());

            if (!string.IsNullOrWhiteSpace(Settings.AuthToken) && !string.IsNullOrWhiteSpace(Settings.UserSid))
            {
                Client.CurrentUser = new MobileServiceUser(Settings.UserSid);
                Client.CurrentUser.MobileServiceAuthenticationToken = Settings.AuthToken;
            }
#else
            //unauthorized version
            Client = new MobileServiceClient(appUrl);
#endif
            var fileName = "groceryMateLocal7.db";                //MobileServiceClient.DefaultDatabasePath()
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

        //ONLINE OFFLINE SYNCRONIZATION

        //Syncronize our tables to azure
        public async Task SyncTables()
        {
            await Initialize();
            ReadOnlyCollection<MobileServiceTableOperationError> syncErrors = null;
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

        //USERS

        public async Task<bool> FindUser()
        {
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
            return found;
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


        //RECEIPTS

        public async Task<Receipt> GetReceipt(int receiptId)
        {
            await SyncTables();

            var data = await receiptTable
                .Where(c => c.ReceiptId == receiptId)
                .ToCollectionAsync();

            return data.Single();
        }

        public async Task<ICollection<Receipt>> GetReceiptsForUser()
        {
            await SyncTables();

            var data = await receiptTable
                .Where(c => c.UserId == Settings.UserSid)
                .OrderByDescending(c => c.CreatedAt)
                .ToCollectionAsync();

            return data;
        }

        public async Task<ICollection<double>> GetTotalsForReceipts(ICollection<int> receiptIds)
        {
            await SyncTables();
            Collection<double> totals = new Collection<double>();

            foreach (int id in receiptIds)
            {
                double receiptTotal = 0;
                var data = await itemTable
                .Where(c => c.ReceiptId == id)
                .ToCollectionAsync();

                foreach (Item i in data)
                    receiptTotal += i.Price;

                totals.Add(receiptTotal);
            }
            return totals;
        }

        public async Task<int> AllocatedReceiptId()
        {
            await SyncTables();

            var data = await receiptTable
                .Where(c => c.Id != null)
                .ToListAsync();
            
            int id = data.Count;
            //avoid ID troubles later
            foreach(Receipt r in data)
                if(r.ReceiptId == id)
                    id += 10000;
            
            return id;
        }

        public async Task<Receipt> AddReceipt(string storeName, ICollection<Item> items)
        {
            await Initialize();
            int receiptId = await AllocatedReceiptId();
            
            foreach(Item it in items)
                await AddItem(it.Name, it.Price, receiptId);
            
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

        public async Task<Receipt> DeleteReceipt(int receiptId)
        {
            await Initialize();

            var receipt = await GetReceipt(receiptId);

            //delete items belonging to receipt first.
            var items = await GetItemsForReceipt(receiptId);
            foreach (Item i in items)
                await DeleteItem(i.ItemId);

            //then delete receipt
            await receiptTable.DeleteAsync(receipt);

            await SyncTables();

            return receipt;
        }


        //ITEMS

        public async Task<int> CountItems()
        {
            await SyncTables();

            var data = await itemTable
                .Where(c => c.Id != null)
                .ToListAsync();

            int id = data.Count;
            //avoid ID troubles later
            foreach (Item r in data)
                if (r.ItemId == id)
                    id += 10000;

            return id;
        }

        public async Task<IEnumerable<Item>> GetItems()
        {
            await SyncTables();

            var data = await itemTable
                .OrderBy(i => i.CreatedAt)
                .ToEnumerableAsync();

            return data;
        }

        public async Task<ICollection<Item>> GetItemsForReceipt(int receiptId)
        {
            await SyncTables();

            var data = await itemTable
                .Where(i => i.ReceiptId == receiptId)
                .OrderBy(i => i.CreatedAt)
                .ToCollectionAsync();

            return data;
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

        public async Task<Item> DeleteItem(int itemId)
        {
            var data = await itemTable
                .Where(c => c.ItemId == itemId)
                .ToListAsync();

            await itemTable.DeleteAsync(data.Single());

            await SyncTables();

            return data.Single();
        }

        //AUTHENTICATION

        // Define an authenticated user.
        public async Task<bool> Authenticate()
        {
            Activity currentActivity = CrossCurrentActivity.Current.Activity; //get current activity
            await Initialize();

            var success = false;
            try
            {
                if (!CrossConnectivity.Current.IsConnected)
                    return false;

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
                    UseAuth = true;

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