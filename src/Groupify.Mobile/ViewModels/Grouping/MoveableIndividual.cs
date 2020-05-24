using System.ComponentModel;
using DIPS.Xamarin.UI.Extensions;
using Groupify.Mobile.Models;

namespace Groupify.Mobile.ViewModels.Grouping
{
    public class MoveableIndividual : INotifyPropertyChanged
    {
        private Individual m_individual;
        private bool m_isHighligted;

        public MoveableIndividual(Individual individual)
        {
            m_individual = individual;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsHighligted
        {
            get => m_isHighligted;
            set => PropertyChanged.RaiseWhenSet(ref m_isHighligted, value);
        }

        public string Name => m_individual.Name;
    }
}
