using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Groupify.Mobile.Models;

namespace Groupify.Mobile.Abstractions
{
    public interface IDeviceDataBase
    {
        Task<Individual> GetIndividual(int id); 
        Task<List<Individual>> GetAllIndividuals();
        Task<int> Save(Individual group);
        Task<Group> GetGroup(int id);
        Task<List<Group>> GetAllGroups();
        Task<int> Save(Group group);
        Task<IndividualsGroup> GetIndividualsGroup(int id);
        Task<List<IndividualsGroup>> GetAllIndividualsGroups();
        Task<int> Save(IndividualsGroup group);
        Task Initialize(Action<Exception> onException);
    }
}