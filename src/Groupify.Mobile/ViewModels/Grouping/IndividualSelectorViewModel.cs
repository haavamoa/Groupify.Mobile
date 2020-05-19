﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using DIPS.Xamarin.UI.Extensions;
using Groupify.Mobile.Models;
using Groupify.Mobile.ViewModels.Grouping.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Groupify.Mobile.ViewModels.Grouping
{
    public class IndividualSelectorViewModel : IGroupingState, INotifyPropertyChanged
    {
        public IndividualSelectorViewModel()
        {
            GroupCommand = new Command<int>(numberOfIndividuals =>
            {
                if (numberOfIndividuals == 0)
                {
                    return;
                }

                var selectedIndividuals = SelectableIndividuals.Where(selectableIndividual => selectableIndividual.IsSelected).ToList();
                var individualModels = new List<Individual>();
                selectedIndividuals.ForEach(selectedIndividuals => individualModels.Add(selectedIndividuals.GetIndividualModel()));

                m_groupingStateMachine.GoToGroupSelectorState(individualModels);
            });
        }

        public ICommand GroupCommand { get; }
        private bool m_isAllSelected = true;
        private IGroupingStateMachine m_groupingStateMachine;
        private List<Individual> m_originIndividuals;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsAllSelected
        {
            get => m_isAllSelected;
            set
            {
                PropertyChanged.RaiseWhenSet(ref m_isAllSelected, value);
                SelectableIndividuals.ForEach(selectableIndividual => selectableIndividual.IsSelected = value);
            }
        }

        public ObservableCollection<SelectableIndividualViewModel> SelectableIndividuals { get; } = new ObservableCollection<SelectableIndividualViewModel>();

        public void Prepare(List<Individual> individuals)
        {
            individuals.ForEach(individual => SelectableIndividuals.Add(new SelectableIndividualViewModel(individual) { IsSelected = IsAllSelected }));
        }

        public void Initialize(IGroupingStateMachine groupingStateMachine)
        {
            m_groupingStateMachine = groupingStateMachine;
        }
    }
}
