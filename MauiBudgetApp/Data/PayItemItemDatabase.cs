using MauiBudgetApp.Models;
using SQLite;

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
                    CreateTableResult expenseTable = await Database.CreateTableAsync<ExpenseItem>();
                    CreateTableResult incomeTable = await Database.CreateTableAsync<IncomeItem>();
                }
                catch (Exception ex)
                {
                    // Need to log the error and tell user about it
                    Console.WriteLine(ex.Message);
                }
                
                return instance;
            });

        public PayItemItemDatabase()
        {
            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        }

        /// <summary>
        /// Gets all pay items.
        /// </summary>
        /// <returns>List of pay items.</returns>
        public async Task<List<PayItem>> GetAllItemsAsync()
        {
            var incomeItems = await Database.Table<IncomeItem>().ToListAsync();
            var expenseItems = await Database.Table<ExpenseItem>().ToListAsync();

            List<PayItem> payItems = new List<PayItem>();
            foreach (var item in incomeItems)
            {
                payItems.Add(item);
            }

            foreach (var expenseItem in expenseItems)
            {
                payItems.Add(expenseItem);
            }

            return payItems;
        }

        /// <summary>
        /// Gets all Income items
        /// </summary>
        /// <returns>List of Income items.</returns>
        public Task<List<IncomeItem>> GetIncomeItemsAsync()
        {
            return Database.Table<IncomeItem>().ToListAsync();
        }

        /// <summary>
        /// Gets all Expense items
        /// </summary>
        /// <returns>List of expense items</returns>
        public Task<List<ExpenseItem>> GetExpenseItemsAsync()
        {
            return Database.Table<ExpenseItem>().ToListAsync();
        }

        /// <summary>
        /// Gets specific Income item based on ID.
        /// </summary>
        /// <param name="id">ID of the income item to find.</param>
        /// <returns>Income item</returns>
        public Task<IncomeItem> GetIncomeItemAsync(int id)
        {
            return Database.Table<IncomeItem>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Gets specific Expense item based on ID.
        /// </summary>
        /// <param name="id">ID of the expense item to find.</param>
        /// <returns>Expense item</returns>
        public Task<ExpenseItem> GetExpenseItemAsync(int id)
        {
            return Database.Table<ExpenseItem>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Saves the payitem.
        /// </summary>
        /// <param name="item">The payitem to save (income or expense)</param>
        /// <returns>Int indicating if it save/updated record.</returns>
        public async Task<int> SaveItemAsync(PayItem item)
        {
            if (item is ExpenseItem)
            {
                if (item.ID != 0)
                {
                    return await Database.UpdateAsync((ExpenseItem)item);
                }
                else
                {
                    item.IsVisible = true;
                    var res = await Database.InsertAsync((ExpenseItem)item);
                    return res;
                }
            }
            else
            {
                if (item.ID != 0)
                {
                    return await Database.UpdateAsync((IncomeItem)item);
                }
                else
                {
                    item.IsVisible = true;
                    var res = await Database.InsertAsync((IncomeItem)item);
                    return res;
                }
            }
            
        }

        /// <summary>
        /// Deteles specific pay item
        /// </summary>
        /// <param name="item">Pay item to delete</param>
        /// <returns>int indicating how many records where deleted.</returns>
        public async Task<int> DeleteItemAsync(PayItem item)
        {
            try
            {
                if (item is ExpenseItem)
                {
                    return await Database.DeleteAsync((ExpenseItem)item);
                }
                else
                {
                    return await Database.DeleteAsync((IncomeItem)item);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await Task.CompletedTask;
                return 0;
            }

        }

        private async Task<List<PayItem>> ReturnEmptyList()
        {
            await Task.CompletedTask;
            List<PayItem> empty = new List<PayItem>();
            return empty;
        }

        internal Task GetIncomeItemAsync()
        {
            throw new NotImplementedException();
        }
    }
}
