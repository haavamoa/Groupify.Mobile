using System.Collections.Generic;
using Groupify.Mobile.Models;

namespace Groupify.Mobile.ViewModels.Grouping.Abstractions
{
    public interface IGroupingStateMachine
    {
        void GoToIndividualSelectorState();
        void GoToGroupSelectorState(List<Individual> selectedIndividuals);
        void GoToGroupsOverViewState();
    }
}