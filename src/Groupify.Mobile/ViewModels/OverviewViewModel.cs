using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using DIPS.Xamarin.UI.Commands;
using Groupify.Mobile.Abstractions;
using Groupify.Mobile.Services;

namespace Groupify.Mobile.ViewModels
{
    public class OverviewViewModel : IViewModel
    {
        public OverviewViewModel(INavigationService navigationService)
        {
            NavigateToGroupingCommand = new AsyncCommand(navigationService.Push<IndividualSelectorViewModel>, onException:OnException);
            RegisterIndividualsGroupCommand = new AsyncCommand(navigationService.Push<RegisterViewModel>);
        }

        private void OnException(Exception obj)
        {
            
        }
#nullable disable
        public event PropertyChangedEventHandler PropertyChanged;
#nullable restore

        public IAsyncCommand NavigateToGroupingCommand { get; }

        public IAsyncCommand RegisterIndividualsGroupCommand { get; }

        public async Task Initialize()
        {
            
        }
    }
}