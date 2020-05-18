using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DIPS.Xamarin.UI.Extensions;
using Groupify.Mobile.Abstractions;
using Groupify.Mobile.Models;
using Groupify.Mobile.ViewModels.Grouping.Abstractions;
using Xamarin.Forms;

namespace Groupify.Mobile.ViewModels.Grouping
{
    public class GroupingViewModel : IViewModel
    {
        private readonly IndividualSelectorViewModel m_individualSelectorViewModel;
        private readonly GroupSelectorViewModel m_groupSelectorViewModel;
        private readonly GroupsOverviewViewModel m_groupsOverviewViewModel;

        public GroupingViewModel(IndividualSelectorViewModel individualSelectorViewModel, GroupSelectorViewModel groupSelectorViewModel, GroupsOverviewViewModel groupsOverviewViewModel)
        {
            m_individualSelectorViewModel = individualSelectorViewModel;
            m_groupSelectorViewModel = groupSelectorViewModel;
            m_groupsOverviewViewModel = groupsOverviewViewModel;
            GroupCommand = new Command(() => m_groupSelectorViewModel.RandomizeGroup(NumberOfIndividualsInGroup, Group));
        }

        public Group Group{ get; private set; }

        public int NumberOfIndividualsInGroup { get; set; }

        public ICommand GroupCommand { get; }

        public void Prepare(Group selectedGroup)
        {
            Group = selectedGroup;
        }

        public void Setup(ViewModelConfiguration configuration) { configuration.InitializeMethod = Initialize; }

        private Task Initialize()
        {
            CurrentState = m_individualSelectorViewModel;
            return Task.CompletedTask;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private IGroupState m_currentState;

        public IGroupState CurrentState
        {
            get => m_currentState;
            set => PropertyChanged.RaiseWhenSet(ref m_currentState, value);
        }

    }
}
