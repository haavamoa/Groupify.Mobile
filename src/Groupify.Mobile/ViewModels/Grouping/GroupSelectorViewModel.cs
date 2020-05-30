using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using DIPS.Xamarin.UI.Commands;
using Groupify.Mobile.Abstractions;
using Groupify.Mobile.Extensions;
using Groupify.Mobile.Models;
using Groupify.Mobile.Services;
using Groupify.Mobile.ViewModels.Grouping.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Groupify.Mobile.ViewModels.Grouping
{
    public class GroupSelectorViewModel : IGroupingState
    {
        private IGroupingStateMachine m_groupingStateMachine;
        private MoveableIndividual m_highLightedIndividual;
        private List<Individual> m_selectedIndividuals;
        private readonly IDeviceDataBase m_deviceDataBase;
        private readonly ILogService m_logService;

        public GroupSelectorViewModel(IDeviceDataBase deviceDataBase, ILogService logService)
        {
            ApproveCommand = new AsyncCommand(async () =>
            {
                m_groupingStateMachine.GoToGroupsOverViewState(GroupedGroups.Where(g => g.Any()).ToList());
                try
                {
                    foreach (var groupedGroup in GroupedGroups)
                    {
                        foreach (var individual in groupedGroup)
                        {
                            var otherIndividualsInGroup = groupedGroup.Where(individualInGroup => individualInGroup.GetIndividual().Id != individual.GetIndividual().Id);
                            foreach (var otherIndividual in otherIndividualsInGroup)
                            {
                                await m_deviceDataBase.Save(new IndividualGroupings() { IndividualId = individual.GetIndividual().Id, OtherIndividualId = otherIndividual.GetIndividual().Id });
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    m_logService.Log(exception);
                }
            });

            GroupCommand = new Command<int>(numberOfIndividualsInGroup =>
                {
                    Group(numberOfIndividualsInGroup);
                });

            ChangeSelectionCommand = new Command(() =>
            {
                m_groupingStateMachine.GoToIndividualSelectorState();
            });

            HighlightCommand = new AsyncCommand<MoveableIndividual>(async individual =>
             {

                 try
                 {
                     CancelMovementCommand.Execute(null);
                     m_highLightedIndividual = individual;
                     m_highLightedIndividual.IsHighligted = true;
                     var groupsToHighlight = GroupedGroups.Where(groupedIndividuals => !groupedIndividuals.Contains(individual));
                     groupsToHighlight.ForEach(g => g.IsHighlighted = true);

                     await DisplayNumberOfTimesWithOthers(individual);
                 }
                 catch (System.Exception exception)
                 {
                     m_logService.Log(exception);
                 }
             });

            CancelMovementCommand = new Command(() =>
            {
                GroupedGroups.ForEach(g => g.IsHighlighted = false);
                if (m_highLightedIndividual != null)
                {
                    m_highLightedIndividual.IsHighligted = false;
                }
                m_highLightedIndividual = null;

                GroupedGroups.ForEach(g => g.ForEach(i => i.NumberOfTimesGroupedWith = 0));
            });

            MoveIndividualCommand = new Command<GroupedIndividuals>(ig =>
            {
                var group = GroupedGroups.First(groupedIndividuals => groupedIndividuals.Contains(m_highLightedIndividual));
                group.Remove(m_highLightedIndividual);
                ig.Add(m_highLightedIndividual);
                CancelMovementCommand.Execute(null);
            });
            m_deviceDataBase = deviceDataBase;
            m_logService = logService;
        }

        private async Task DisplayNumberOfTimesWithOthers(MoveableIndividual theIndividual)
        {
            var otherIndividualsGroupedWith = await m_deviceDataBase.GetAllIndividualGroupingsForIndividual(theIndividual.GetIndividual());
            
            foreach (var groupedGroup in GroupedGroups)
            {
                foreach (var individual in groupedGroup)
                {
                    individual.NumberOfTimesGroupedWith = otherIndividualsGroupedWith.Count(ig => ig.OtherIndividualId == individual.GetIndividual().Id);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand ApproveCommand { get; }
        public ICommand CancelMovementCommand { get; }
        public ICommand ChangeSelectionCommand { get; }
        public ICommand GroupCommand { get; }
        public ObservableCollection<GroupedIndividuals> GroupedGroups { get; } = new ObservableCollection<GroupedIndividuals>();
        public ICommand HighlightCommand { get; }
        public ICommand MoveIndividualCommand { get; }

        public void Initialize(IGroupingStateMachine groupingStateMachine)
        {
            m_groupingStateMachine = groupingStateMachine;
        }

        internal void Prepare(List<Individual> selectedIndividuals, int numberOfIndividualsInGroup)
        {
            m_selectedIndividuals = selectedIndividuals;
            Group(numberOfIndividualsInGroup);
        }

        private void Group(int numberOfIndividualsInGroup)
        {
            m_selectedIndividuals.Shuffle();
            var shuffledListOfGroups = m_selectedIndividuals.ChunkBy(numberOfIndividualsInGroup);
            shuffledListOfGroups.Shuffle();
            GroupedGroups.Clear();
            foreach (var group in shuffledListOfGroups)
            {
                var groupedIndividuals = new GroupedIndividuals($"Gruppe {shuffledListOfGroups.IndexOf(group) + 1}");

                foreach (var individual in group)
                {
                    groupedIndividuals.Add(new MoveableIndividual(individual));
                }
                GroupedGroups.Add(groupedIndividuals);
            }
        }
    }
}
