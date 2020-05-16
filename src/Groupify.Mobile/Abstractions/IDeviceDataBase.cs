using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Groupify.Mobile.Models;

namespace Groupify.Mobile.Abstractions
{
    public interface IDeviceDataBase
    {
        Task<Individual> GetIndividual(int id); 
        Task Save(Individual group);
        Task Delete(Group group);
        Task Delete(Individual individual);
        Task<Group> GetGroup(int id);
        Task<List<Group>> GetAllGroups();
        Task Save(Group group);
        Task Initialize();
        Task DeleteAllIndividualGroups(Group group);
        Task<List<Individual>> GetAllIndividuals();
    }
}