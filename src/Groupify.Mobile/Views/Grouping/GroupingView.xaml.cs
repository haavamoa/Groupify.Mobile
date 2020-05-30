using System;
using System.ComponentModel;
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
            if (m_currentState is IndividualSelectorViewModel individualSelectorViewModel)
            {
                if (((GroupingViewModel)BindingContext).NumberOfIndividualsInGroup == 0 || (!(int.TryParse(NumberOfIndividualsInGroupEntry.Text, out var number))))
                {
                    NumberOfIndividualsInGroupFrame.Shake();
                    NumberOfIndividualsInGroupEntry.Focus();
                    NumberOfIndividualsInGroupEntry.CursorPosition = 1;
                    NumberOfIndividualsInGroupEntry.SelectionLength = 1;
                    return;
                }
                individualSelectorViewModel.GroupCommand.Execute(((GroupingViewModel)BindingContext).NumberOfIndividualsInGroup);
            }
            else if (m_currentState is GroupSelectorViewModel groupSelectorViewModel)
            {
                groupSelectorViewModel.ApproveCommand.Execute(null);
            }
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            ((GroupingViewModel)BindingContext).PropertyChanged += OnGroupingViewModelPropertyChanged;
        }

        private void OnGroupingViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals(nameof(GroupingViewModel.CurrentState)))
            {
                //Cleanup
                if (m_currentState is GroupSelectorViewModel)
                {
                    ExtraButtonsGrid.IsVisible = false;
                }

                //Initialize new state
                m_currentState = ((GroupingViewModel)BindingContext).CurrentState;

                if (m_currentState is IndividualSelectorViewModel)
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

        private async void OnBackClicked(object sender, EventArgs e)
        {
            if (m_currentState is GroupSelectorViewModel)
            {
                var shouldClose = await ((BackdropPage)Application.Current.MainPage).ConfirmClosingGrouping();
                if (!(shouldClose))
                {
                    return;
                }
            }
            ((BackdropPage)Application.Current.MainPage).GoBack();
        }
    }
}