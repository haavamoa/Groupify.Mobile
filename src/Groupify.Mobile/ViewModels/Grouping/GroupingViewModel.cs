using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using DIPS.Xamarin.UI.Extensions;
using Groupify.Mobile.Abstractions;
using Groupify.Mobile.Models;
using Groupify.Mobile.Services;
using Groupify.Mobile.ViewModels.Grouping.Abstractions;

namespace Groupify.Mobile.ViewModels.Grouping
{
    public class GroupingViewModel : IViewModel, IGroupingStateMachine
    {
        private readonly IDeviceDataBase m_deviceDataBase;
        private readonly GroupSelectorViewModel m_groupSelectorViewModel;
        private readonly GroupsOverviewViewModel m_groupsOverviewViewModel;
        private readonly IndividualSelectorViewModel m_individualSelectorViewModel;
        private readonly ILogService m_logService;
        private IGroupingState m_currentState;
        private int m_numberOfIndividualsInGroup = 2;

        public GroupingViewModel(IDeviceDataBase deviceDataBase, ILogService logService, IndividualSelectorViewModel individualSelectorViewModel, GroupSelectorViewModel groupSelectorViewModel, GroupsOverviewViewModel groupsOverviewViewModel)
        {
            m_deviceDataBase = deviceDataBase;
            m_logService = logService;
            m_individualSelectorViewModel = individualSelectorViewModel;
            m_groupSelectorViewModel = groupSelectorViewModel;
            m_groupsOverviewViewModel = groupsOverviewViewModel;

        }

        public event PropertyChangedEventHandler PropertyChanged;

        public IGroupingState CurrentState
        {
            get => m_currentState;
            private set => PropertyChanged.RaiseWhenSet(ref m_currentState, value);
        }

        public Group Group { get; private set; }

        public int NumberOfIndividualsInGroup
        {
            get => m_numberOfIndividualsInGroup;
            set => PropertyChanged.RaiseWhenSet(ref m_numberOfIndividualsInGroup, value);
        }
        public void GoToGroupSelectorState(List<Individual> selectedIndividuals)
        {
            m_groupSelectorViewModel.Prepare(selectedIndividuals, NumberOfIndividualsInGroup);
            CurrentState = m_groupSelectorViewModel;
        }

        public void GoToGroupsOverViewState(List<GroupedIndividuals> groupedGroups)
        {
            m_groupsOverviewViewModel.Prepare(groupedGroups);
            CurrentState = m_groupsOverviewViewModel;
        }

        public void GoToIndividualSelectorState()
        {
            CurrentState = m_individualSelectorViewModel;
        }

        public void Prepare(Group selectedGroup)
        {
            Group = selectedGroup;
        }

        public void Setup(ViewModelConfiguration configuration) { configuration.InitializeMethod = Initialize; }

        private async Task Initialize()
        {
            try
            {
                ((IGroupingState)m_individualSelectorViewModel).Initialize(this);
                ((IGroupingState)m_groupSelectorViewModel).Initialize(this);
                ((IGroupingState)m_groupsOverviewViewModel).Initialize(this);

                GoToIndividualSelectorState();
                m_individualSelectorViewModel.Prepare(await m_deviceDataBase.GetIndividuals(Group.Id));
            }
            catch (Exception exception)
            {
                m_logService.Log(exception);
            }
        }
    }
}
