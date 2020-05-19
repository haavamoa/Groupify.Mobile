using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Groupify.Mobile.Extensions;
using Groupify.Mobile.ViewModels.Grouping;
using Groupify.Mobile.ViewModels.Grouping.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Groupify.Mobile.Views.Grouping
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GroupingView : BackdropMainView
    {
        private IGroupingState m_currentState;

        public GroupingView()
        {
            InitializeComponent();
        }

        private void OnBigButtonPressed(object sender, EventArgs e)
        {
            if(m_currentState is IndividualSelectorViewModel)
            {
                if (((GroupingViewModel)BindingContext).NumberOfIndividualsInGroup == 0)
                {
                    NumberOfIndividualsInGroupFrame.Shake();
                    NumberOfIndividualsInGroupEntry.Focus();
                    NumberOfIndividualsInGroupEntry.CursorPosition = 1;
                    NumberOfIndividualsInGroupEntry.SelectionLength = 1;
                }
            }
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            ((GroupingViewModel)BindingContext).PropertyChanged += OnGroupingViewModelPropertyChanged;
        }

        private void OnGroupingViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName.Equals(nameof(GroupingViewModel.CurrentState)))
            {
                m_currentState = ((GroupingViewModel)BindingContext).CurrentState;
                
                if(m_currentState is IndividualSelectorViewModel)
                {
                    CurrentStateContentView.Content = new IndividualSelectorView();
                }

                if (m_currentState is GroupSelectorViewModel)
                {
                    CurrentStateContentView.Content = new GroupSelectorView();
                }

                if (m_currentState is GroupsOverviewViewModel)
                {
                    CurrentStateContentView.Content = new GroupOverviewView();
                }

                CurrentStateContentView.Content.BindingContext = m_currentState;
            }
        }
    }
}