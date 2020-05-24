using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using DIPS.Xamarin.UI.Extensions;
using Groupify.Mobile.Models;

namespace Groupify.Mobile.ViewModels.Grouping
{
    public class GroupedIndividuals : ObservableCollection<MoveableIndividual>, INotifyPropertyChanged
    {
        private bool m_isHighlighted;

        public GroupedIndividuals(string name)
        {
            Name = name;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsHighlighted
        {
            get => m_isHighlighted;
            set => PropertyChanged.RaiseWhenSet(ref m_isHighlighted, value);
        }

        public string Name { get; }
    }
}
