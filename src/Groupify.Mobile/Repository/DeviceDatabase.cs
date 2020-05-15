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


        public Task<int> Delete(Group group) => Delete<Group>(group);

        public Task<List<Group>> GetAllGroups()
        {
            return GetAll<Group>();
        }

        public Task<List<Individual>> GetAllIndividuals()
        {
            return GetAll<Individual>();
        }

        public Task<List<IndividualsGroup>> GetAllIndividualsGroups()
        {
            return GetAll<IndividualsGroup>();
        }

        public Task<Group> GetGroup(int id)
        {
            return Get<Group>(g => g.Id == id);
        }

        public Task<Individual> GetIndividual(int id)
        {
            return Get<Individual>(g => g.Id == id);
        }

        public Task<IndividualsGroup> GetIndividualsGroup(int id)
        {
            return Get<IndividualsGroup>(ig => ig.Id == id);
        }

        public async Task Initialize()
        {
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
        public Task<int> Save(Individual individual)
        {
            return Save(individual, individual.Id);
        }

        public Task<int> Save(Group group)
        {
            return Save(group, group.Id);
        }

        public Task<int> Save(IndividualsGroup group)
        {
            return Save<IndividualsGroup>(group, group.Id);
        }

        private async Task<int> Delete<T>(T item) where T : new()
        {
            return await Database.DeleteAsync(item);
        }

        private async Task<T> Get<T>(Expression<Func<T, bool>> expression) where T : new()
        {
            return await Database.Table<T>().Where(expression).FirstOrDefaultAsync();
        }

        private async Task<List<T>> GetAll<T>() where T : new()
        {
            return await Database.Table<T>().ToListAsync();
        }
        private async Task<int> Save<T>(T item, int id) where T : new()
        {
            if (id != 0)
            {
                return await Database.UpdateAsync(item);
            }
            else
            {
                return await Database.InsertAsync(item);
            }
        }
    }
}
