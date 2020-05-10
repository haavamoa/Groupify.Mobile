using System.Collections.Generic;
using System.Threading.Tasks;
using Groupify.Mobile.Models;

namespace Groupify.Mobile.Abstractions
{
    public interface IDeviceDataBase
    {
        Task<IndividualsGroup> GetIndividualsGroup(int id);
        Task<List<IndividualsGroup>> GetIndividualsGroups();
        Task<int> Save(IndividualsGroup group);
    }
}