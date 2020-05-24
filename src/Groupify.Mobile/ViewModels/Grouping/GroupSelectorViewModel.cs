using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using DIPS.Xamarin.UI.Extensions;
using Groupify.Mobile.Abstractions;
using Groupify.Mobile.Extensions;
using Groupify.Mobile.Models;
using Groupify.Mobile.ViewModels.Grouping.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Groupify.Mobile.ViewModels.Grouping
{
    public class GroupSelectorViewModel : IGroupingState
    {
        private IGroupingStateMachine m_groupingStateMachine;
        private List<Individual> m_selectedIndividuals;
        private ObservableCollection<GroupedIndividuals> m_groupedGroups;

        public event PropertyChangedEventHandler PropertyChanged;

        public GroupSelectorViewModel()
        {
            ApproveCommand = new Command(() =>
            {
                //Send the randomized group to overview
                m_groupingStateMachine.GoToGroupsOverViewState();
            });

            GroupCommand = new Command<int>(numberOfIndividualsInGroup =>
                {
                    Group(numberOfIndividualsInGroup);
                });

            ChangeSelectionCommand = new Command(() =>
            {
                m_groupingStateMachine.GoToIndividualSelectorState();
            });
        }

        private void Group(int numberOfIndividualsInGroup)
        {
            m_selectedIndividuals.Shuffle();
            var shuffledListOfGroups = m_selectedIndividuals.ChunkBy(numberOfIndividualsInGroup);
            shuffledListOfGroups.Shuffle();
            var listOfGroupedGroups = new ObservableCollection<GroupedIndividuals>();
            GroupedGroups.Clear();
            foreach (var group in shuffledListOfGroups)
            {
                var groupedIndividuals = new GroupedIndividuals($"Gruppe {shuffledListOfGroups.IndexOf(group) + 1}");
                
                foreach(var individual in group)
                {
                    groupedIndividuals.Add(individual);
                }
                GroupedGroups.Add(groupedIndividuals);
            }
        }

        public ObservableCollection<GroupedIndividuals> GroupedGroups { get; } = new ObservableCollection<GroupedIndividuals>();

        public ICommand GroupCommand { get; }

        public ICommand ChangeSelectionCommand { get; }

        internal void Prepare(List<Individual> selectedIndividuals, int numberOfIndividualsInGroup)
        {
            m_selectedIndividuals = selectedIndividuals;
            Group(numberOfIndividualsInGroup);
        }

        public void Initialize(IGroupingStateMachine groupingStateMachine)
        {
            m_groupingStateMachine = groupingStateMachine;
        }

        public ICommand ApproveCommand { get; }
    }
}
