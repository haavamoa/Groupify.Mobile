using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using DIPS.Xamarin.UI.Extensions;
using Groupify.Mobile.Models;
using Xamarin.Forms;

namespace Groupify.Mobile.ViewModels.Grouping
{
    public class SelectableIndividualViewModel : INotifyPropertyChanged
    {
        private bool m_isSelected;

        public SelectableIndividualViewModel(Individual individual, Action onSelectionChangedAction)
        {
            m_individual = individual;
            m_onSelectedCallback = onSelectionChangedAction;
            SelectCommand = new Command(() => IsSelected = !IsSelected);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public int Id => m_individual.Id;
        public bool IsSelected
        {
            get => m_isSelected;
            set
            {
                PropertyChanged.RaiseWhenSet(ref m_isSelected, value);
                m_onSelectedCallback.Invoke();
            }
        }

        public string Name => m_individual.Name;

        private Individual m_individual;
        private readonly Action m_onSelectedCallback;

        public Individual GetIndividualModel() => m_individual;

        public ICommand SelectCommand { get; }
    }
}
