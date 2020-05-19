using System.ComponentModel;
using Groupify.Mobile.Abstractions;
using Groupify.Mobile.Services;
using Groupify.Mobile.ViewModels.Grouping.Abstractions;

namespace Groupify.Mobile.ViewModels.Grouping
{
    public class GroupsOverviewViewModel : IGroupingState
    {
        private IGroupingStateMachine m_groupingStateMachine;

        public GroupsOverviewViewModel()
        {
        }

        public void Initialize(IGroupingStateMachine groupingStateMachine)
        {
            m_groupingStateMachine = groupingStateMachine;
        }
    }
}