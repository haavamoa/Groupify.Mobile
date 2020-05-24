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

            if(m_currentState is IndividualSelectorViewModel individualSelectorViewModel)
            {
                individualSelectorViewModel.GroupCommand.Execute(((GroupingViewModel)BindingContext).NumberOfIndividualsInGroup);
            }
            else if(m_currentState is GroupSelectorViewModel groupSelectorViewModel)
            {
                groupSelectorViewModel.ApproveCommand.Execute(null);
            }
            else if(m_currentState is GroupsOverviewViewModel groupsOverviewViewModel)
            {
                groupsOverviewViewModel.DoneCommand.Execute(null);
            }
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            ((GroupingViewModel)BindingContext).PropertyChanged += OnGroupingViewModelPropertyChanged;
        }

        private void OnGroupingViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //Cleanup
            if(m_currentState is GroupSelectorViewModel)
            {
                ExtraButtonsGrid.IsVisible = false;
            }


            //Initialize new state
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
                    ExtraButtonsGrid.IsVisible = true;
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