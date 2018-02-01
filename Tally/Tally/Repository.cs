using System;
using System.Collections.Generic;
using System.Text;
using Tally.Models;
using SQLite;
using System.Threading.Tasks;
using System.Threading;
using FreshMvvm;
using Xamarin.Forms;

namespace Tally
{
    public class Repository
    {
        private readonly SQLiteAsyncConnection db;
        public SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);
        public string StatusMessage { get; set; }
        bool isInitialized;
        

        public Repository(string dbPath)
        {
            db = new SQLiteAsyncConnection(dbPath);
            db.CreateTableAsync<Item>().Wait();
        }

        public async Task CreateItem(Item item)
        {
            try
            {
                // Basic validation to ensure we have a item name.
                if (string.IsNullOrWhiteSpace(item.Name))
                    throw new Exception("Name is required");

                if (string.IsNullOrWhiteSpace(item.Cost))
                    throw new Exception("Cost is required");

                // Insert/update contact.
                var result = await db.InsertOrReplaceAsync(item).ConfigureAwait(continueOnCapturedContext: false);
                StatusMessage = $"{result} record(s) added [Item Name: {item.Name}])";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Failed to create item: {item.Name}. Error: {ex.Message}";
            }
        }

        public Task<List<Item>> GetAllItems()
        {
            // Return a list of bills saved to the Bill table in the database.
            return db.Table<Item>().ToListAsync();
        }

        internal Task<int> DeleteItemAsync(Item item)
        {
            return db.DeleteAsync(item);
        }

        internal async Task DeleteAll()
        {
            await Initialize().ConfigureAwait(false);

            await semaphoreSlim.WaitAsync().ConfigureAwait(false);

            try
            {
                await db.DropTableAsync<Item>();
                await db.CreateTableAsync<Item>();
            }
            finally
            {
                semaphoreSlim.Release();
            }

        }

        internal async Task Initialize()
        {
            if (isInitialized)
                return;

            await db.CreateTableAsync<Item>().ConfigureAwait(false);

            isInitialized = true;
        }
    }
}

