using MauiBudgetApp.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiBudgetApp.Data
{
    public class PayItemItemDatabase
    {
        static SQLiteAsyncConnection Database;

        public static readonly AsyncLazy<PayItemItemDatabase> Instance =
            new AsyncLazy<PayItemItemDatabase>(async () =>
            
            {
                var instance = new PayItemItemDatabase();
                try
                {
                    CreateTableResult expense = await Database.CreateTableAsync<PayItem>();
                }
                catch (Exception ex)
                {
                    // Need to log this....
                    Console.WriteLine(ex.Message);
                }
                
                return instance;
            });

        public PayItemItemDatabase()
        {
            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        }

        public Task<List<TodoItem>> GetItemsAsync()
        {
            return Database.Table<TodoItem>().ToListAsync();
        }

        public Task<List<PayItem>> GetExpenseItems()
        {
            return Database.Table<PayItem>().Where(i => !i.IsIncome).ToListAsync();
        }

        public Task<List<PayItem>> GetIncomeItems()
        {
            return Database.Table<PayItem>().Where(i => i.IsIncome).ToListAsync();
        }

        public Task<PayItem> GetItemAsync(int id)
        {
            return Database.Table<PayItem>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveItemAsync(PayItem item)
        {
            if (item.ID != 0)
            {
                return Database.UpdateAsync(item);
            }
            else
            {
                item.IsVisible = true;
                return Database.InsertAsync(item);
            }
        }

        public Task<int> DeleteItemAsync(PayItem item)
        {
            return Database.DeleteAsync(item);
        }
    }
}
