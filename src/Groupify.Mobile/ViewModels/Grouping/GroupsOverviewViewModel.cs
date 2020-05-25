using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using DIPS.Xamarin.UI.Commands;
using Groupify.Mobile.Abstractions;
using Groupify.Mobile.Models;
using Groupify.Mobile.Services;
using Groupify.Mobile.ViewModels.Grouping.Abstractions;
using Xamarin.Forms;

namespace Groupify.Mobile.ViewModels.Grouping
{
    public class GroupsOverviewViewModel : IGroupingState
    {
        private readonly IDeviceDataBase m_deviceDataBase;
        private readonly ILogService m_logService;

        public GroupsOverviewViewModel(IDeviceDataBase deviceDataBase, ILogService logService, INavigationService navigationService)
        {
            DoneCommand = new AsyncCommand(async () =>
            {
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
                                ;
                            }
                        }
                    }

                    await navigationService.GoBack();
                }
                catch (Exception exception)
                {
                    m_logService.Log(exception);
                }

            });
            m_deviceDataBase = deviceDataBase;
            m_logService = logService;
        }

        public ICommand DoneCommand { get; }

        public ObservableCollection<GroupedIndividuals> GroupedGroups { get; } = new ObservableCollection<GroupedIndividuals>();

        public void Initialize(IGroupingStateMachine groupingStateMachine)
        {
        }

        internal void Prepare(List<GroupedIndividuals> groupedGroups)
        {
            GroupedGroups.Clear();
            groupedGroups.ForEach(g => GroupedGroups.Add(g));
        }
    }
}