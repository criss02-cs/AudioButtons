using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AudioButtons.Models;
using SQLite;

namespace AudioButtons
{
    public class Database
    {
        public SQLiteAsyncConnection DB { get; set; }

        public Database()
        {
            Task.Run(Init);
        }

        public async Task Init()
        {
            DB ??= new SQLiteAsyncConnection(DbConstants.DatabasePath, DbConstants.Flags);
            var result = await DB.CreateTableAsync<ButtonAudio>();
            ;
        }

        public async Task<List<ButtonAudio>> GetAllButtons()
        {
            await Init();
            return await DB.Table<ButtonAudio>().ToListAsync();
        }

        public async Task<int> InsertItemAsync(ButtonAudio item)
        {
            await Init();
            return await DB.InsertAsync(item);
        }

        public async Task<int> UpdateItemAsync(ButtonAudio item)
        {
            await Init();
            return await DB.UpdateAsync(item);
        }

        public async Task<int> DeleteItemAsync(ButtonAudio item)
        {
            await Init();
            return await DB.DeleteAsync(item);
        }
    }
}
