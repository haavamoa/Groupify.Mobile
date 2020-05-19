using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Groupify.Mobile.Abstractions;
using Groupify.Mobile.Models;
using Groupify.Mobile.ViewModels.Grouping.Abstractions;
using Xamarin.Forms;

namespace Groupify.Mobile.ViewModels.Grouping
{
    public class GroupSelectorViewModel : IGroupingState
    {
        private IGroupingStateMachine m_groupingStateMachine;

        public event PropertyChangedEventHandler PropertyChanged;

        public GroupSelectorViewModel()
        {
            ApproveCommand = new Command(() =>
            {
                //Send the randomized group to overview
                m_groupingStateMachine.GoToGroupsOverViewState();
            });
        }

        internal void Prepare(List<Individual> selectedIndividuals, int numberOfIndividualsInGroup)
        {

        }

        public void Initialize(IGroupingStateMachine groupingStateMachine)
        {
            m_groupingStateMachine = groupingStateMachine;
        }

        public ICommand ApproveCommand { get; }
    }
}
