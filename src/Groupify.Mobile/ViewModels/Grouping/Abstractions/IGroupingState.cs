using System.Threading.Tasks;
using System.Windows.Input;
using Groupify.Mobile.Models;

namespace Groupify.Mobile.ViewModels.Grouping.Abstractions
{
    public interface IGroupingState
    {
        void Initialize(IGroupingStateMachine groupingStateMachine);
    }
}