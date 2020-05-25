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

        public async Task DeleteAllIndividualGroups(Group group)
        {
            var individualsInGroup = await Database
                .Table<Individual>().Where(i => i.GroupId == group.Id).ToListAsync();
            individualsInGroup.ForEach(async individualInGroup => await Database.Table<Individual>().DeleteAsync(individual => individual.Id == individualInGroup.Id));
            await Delete(group);
        }

        public Task Delete(Group group) => Delete<Group>(group);

        public Task<List<Group>> GetAllGroups()
        {
            return GetAll<Group>();
        }

        public Task<List<Individual>> GetAllIndividuals()
        {
            return GetAll<Individual>();
        }

        public Task<Group> GetGroup(int id)
        {
            return Get<Group>(g => g.Id == id);
        }

        public Task<Individual> GetIndividual(int id)
        {
            return Get<Individual>(g => g.Id == id);
        }

        public async Task Initialize()
        {
            var tasks = new List<Task>();
            if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(Individual).Name))
            {
                tasks.Add(Database.CreateTablesAsync(CreateFlags.None, typeof(Individual)));
            }
            if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(Group).Name))
            {
                tasks.Add(Database.CreateTablesAsync(CreateFlags.None, typeof(Group)));
            }
            if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(IndividualGroupings).Name))
            {
                tasks.Add(Database.CreateTablesAsync(CreateFlags.None, typeof(IndividualGroupings)));
            }

            await Task.WhenAll(tasks);
        }
        public Task Save(Individual individual)
        {
            return Save(individual, individual.Id);
        }

        public Task Save(Group group)
        {
            return Save(group, group.Id);
        }

        private Task Delete<T>(T item) where T : new()
        {
            return Database.DeleteAsync(item);
        }

        private async Task<T> Get<T>(Expression<Func<T, bool>> expression) where T : new()
        {
            return await Database.Table<T>().Where(expression).FirstOrDefaultAsync();
        }

        private async Task<List<T>> GetAll<T>() where T : new()
        {
            return await Database.Table<T>().ToListAsync();
        }
        private Task Save<T>(T item, int id) where T : new()
        {
            if (id != 0)
            {
                return Database.UpdateAsync(item);
            }
            else
            {
                return Database.InsertAsync(item);
            }
        }

        public Task Delete(Individual individual) => Delete<Individual>(individual);
        public Task<List<Individual>> GetIndividuals(int groupId)
        {
            return Database.Table<Individual>().Where(individual => individual.GroupId == groupId).ToListAsync();
        }

        public Task Save(IndividualGroupings individualGrouping) => Save<IndividualGroupings>(individualGrouping, individualGrouping.Id);
        public Task Delete(IndividualGroupings individualGrouping) => Delete(individualGrouping);
        public Task<List<IndividualGroupings>> GetAllIndividualGroupings(Individual individual)
        {
            return Database.Table<IndividualGroupings>().Where(individualGrouping => individualGrouping.IndividualId == individual.Id).ToListAsync();
        }
    }
}
