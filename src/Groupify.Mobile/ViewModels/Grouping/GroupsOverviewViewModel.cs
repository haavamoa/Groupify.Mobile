using System.ComponentModel;
using System.Windows.Input;
using Groupify.Mobile.Abstractions;
using Groupify.Mobile.Services;
using Groupify.Mobile.ViewModels.Grouping.Abstractions;
using Xamarin.Forms;

namespace Groupify.Mobile.ViewModels.Grouping
{
    public class GroupsOverviewViewModel : IGroupingState
    {
        private IGroupingStateMachine m_groupingStateMachine;

        public GroupsOverviewViewModel()
        {
            DoneCommand = new Command(() => { });
        }

        public void Initialize(IGroupingStateMachine groupingStateMachine)
        {
            m_groupingStateMachine = groupingStateMachine;
        }

        public ICommand DoneCommand { get; }
    }
}