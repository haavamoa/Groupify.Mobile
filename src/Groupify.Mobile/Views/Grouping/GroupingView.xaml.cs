using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Groupify.Mobile.ViewModels.Grouping;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Groupify.Mobile.Views.Grouping
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GroupingView : BackdropMainView
    {
        public GroupingView()
        {
            InitializeComponent();
        }

        protected async override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            ((GroupingViewModel)BindingContext).PropertyChanged += OnGroupingViewModelPropertyChanged;
            ObserveEntryTextChangedAndAnimate();
        }

        private async void ObserveEntryTextChangedAndAnimate()
        {
            while (string.IsNullOrEmpty(NumberOfIndividualsInGroupEntry.Text) && !NumberOfIndividualsInGroupEntry.IsFocused)
            {
                await EntryLine.FadeTo(0, length: 800);
                await EntryLine.FadeTo(1, length: 800);
            }
        }

        private void OnGroupingViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals(nameof(GroupingViewModel.CurrentState)))
            {
                var currentState = ((GroupingViewModel)BindingContext).CurrentState;
            }
        }

        private void NumberOfPeopleInGroupFrameTapped(object sender, EventArgs e)
        {
            NumberOfIndividualsInGroupEntry.Focus();
        }

        private void NumberOfIndividualsInGroupEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            ObserveEntryTextChangedAndAnimate();
        }

        private void NumberOfIndividualsInGroupEntry_FocusChanged(object sender, FocusEventArgs e)
        {
            ObserveEntryTextChangedAndAnimate();
        }
    }
}