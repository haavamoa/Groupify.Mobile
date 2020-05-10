using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Groupify.Mobile.Abstractions;
using Groupify.Mobile.Extensions;
using Groupify.Mobile.Models;
using SQLite;

namespace Groupify.Mobile.Repository
{
    public class DeviceDatabase : IDeviceDataBase
    {
        private static readonly Lazy<SQLiteAsyncConnection> s_lazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
        {
            return new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        });

        private static SQLiteAsyncConnection Database => s_lazyInitializer.Value;

        private static bool s_initialized = false;
        private Action<Exception> m_onException;

        public DeviceDatabase(Action<Exception> onException)
        {
            m_onException = onException;
            InitializeAsync().SafeFireAndForget(false, m_onException);
        }

        private async Task InitializeAsync()
        {
            if (!s_initialized)
            {

                if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(IndividualsGroup).Name))
                {
                    await Database.CreateTablesAsync(CreateFlags.None, typeof(IndividualsGroup)).ConfigureAwait(false);
                    
                    s_initialized = true;
                }
            }
        }

        public Task Reset() => Database.DeleteAllAsync<IndividualsGroup>();

        public async Task<List<IndividualsGroup>> GetIndividualsGroups()
        {
            var individualGroups = new List<IndividualsGroup>();
            try
            {
                individualGroups = await Database.Table<IndividualsGroup>().ToListAsync();
            }
            catch (Exception exception)
            {
                m_onException.Invoke(exception);
            }
            return individualGroups;
        }

        public Task<List<IndividualsGroup>> GetItemsNotDoneAsync() =>
            // SQL queries are also possible
            Database.QueryAsync<IndividualsGroup>("SELECT * FROM [TodoItem] WHERE [Done] = 0");

        public Task<IndividualsGroup> GetIndividualsGroup(int id) => Database.Table<IndividualsGroup>().Where(i => i.ID == id).FirstOrDefaultAsync();

        public async Task<int> Save(IndividualsGroup group)
        {
            var id = 0;
            try
            {
                if (group.ID != 0)
                {
                    id = await Database.UpdateAsync(group);
                }
                else
                {
                    id = await Database.InsertAsync(group);
                }
            }
            catch (Exception exception)
            {
                m_onException.Invoke(exception);
            }
            return id;
        }

        public async Task<int> DeleteItemAsync(IndividualsGroup item)
        {
            var id = 0;
            try
            {
                id = await Database.DeleteAsync(item);
            }
            catch (Exception exception)
            {

                m_onException(exception);
            }
            return id;
        }
    }
}
