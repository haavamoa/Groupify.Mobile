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