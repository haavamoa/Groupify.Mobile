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
        Task Save(IndividualGroupings individualGrouping);
        Task Delete(IndividualGroupings individualGrouping);
        Task<List<IndividualGroupings>> GetAllIndividualGroupingsForIndividual(Individual individual);
        Task<List<IndividualGroupings>> GetAllIndividualGroupings();
        Task Delete(Group group);
        Task Delete(Individual individual);
        Task<Group> GetGroup(int id);
        Task<List<Group>> GetAllGroups();
        Task Save(Group group);
        Task Initialize();
        Task DeleteAllIndividualGroups(Group group);
        Task<List<Individual>> GetAllIndividuals();
        Task<List<Individual>> GetIndividuals(int id);
        Task DeleteIndividualGroupings(Individual individual);
    }
}