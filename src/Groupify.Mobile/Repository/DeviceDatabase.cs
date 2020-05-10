using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Groupify.Mobile.Abstractions;
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

        private Action<Exception>? m_onException;

        public async Task Initialize(Action<Exception> onException)
        {
            m_onException = onException;
            var tasks = new List<Task>();
            if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(IndividualsGroup).Name))
            {
                tasks.Add(Database.CreateTablesAsync(CreateFlags.None, typeof(Individual)));
            }
            if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(Group).Name))
            {
                tasks.Add(Database.CreateTablesAsync(CreateFlags.None, typeof(Group)));
            }
            if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(IndividualsGroup).Name))
            {
                tasks.Add(Database.CreateTablesAsync(CreateFlags.None, typeof(IndividualsGroup)));
            }

            await Task.WhenAll(tasks);
        }

        public Task<Group> GetGroup(int id) => Get<Group>(g => g.Id == id);
        public Task<IndividualsGroup> GetIndividualsGroup(int id) => Get<IndividualsGroup>(ig => ig.Id == id);
        public Task<Individual> GetIndividual(int id) => Get<Individual>(g => g.Id == id);
        public Task<List<IndividualsGroup>> GetAllIndividualsGroups() => GetAll<IndividualsGroup>();
        public Task<List<Group>> GetAllGroups() => GetAll<Group>();
        public Task<List<Individual>> GetAllIndividuals() => GetAll<Individual>();
        public Task<int> Save(Individual individual) => Save(individual, individual.Id);
        public Task<int> Save(Group group) => Save(group, group.Id);
        public Task<int> Save(IndividualsGroup group) => Save<IndividualsGroup>(group, group.Id);

        private async Task<List<T>> GetAll<T>() where T : new()
        {
            var items = new List<T>();
            try
            {
                items = await Database.Table<T>().ToListAsync();
            }
            catch (Exception exception)
            {
                m_onException?.Invoke(exception);
            }
            return items;
        }

        private async Task<T> Get<T>(Expression<Func<T, bool>> expression) where T : new()
        {
            var item = default(T);
            try
            {
                item = await Database.Table<T>().Where(expression).FirstOrDefaultAsync();
            }
            catch (Exception exception)
            {
                m_onException?.Invoke(exception);
            }

            return item;
        }

        private async Task<int> Delete<T>(T item) where T : new()
        {
            var id = 0;
            try
            {
                id = await Database.DeleteAsync(item);
            }
            catch (Exception exception)
            {

                m_onException?.Invoke(exception);
            }
            return id;
        }

        private async Task<int> Save<T>(T item, int id) where T : new()
        {
            var newId = 0;
            try
            {
                if (id != 0)
                {
                    newId = await Database.UpdateAsync(item);
                }
                else
                {
                    newId = await Database.InsertAsync(item);
                }
            }
            catch (Exception exception)
            {
                m_onException?.Invoke(exception);
            }
            return newId;
        }
    }
}
