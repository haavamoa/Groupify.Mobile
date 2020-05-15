using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using DIPS.Xamarin.UI.Commands;
using DIPS.Xamarin.UI.Extensions;
using Groupify.Mobile.Abstractions;

namespace Groupify.Mobile.ViewModels
{
    public class OverviewViewModel : IViewModel
    {
        private bool m_isRefreshing;
        private readonly IDeviceDataBase m_database;

        public OverviewViewModel(INavigationService navigationService, IDeviceDataBase database)
        {
            NavigateToGroupingCommand = new AsyncCommand(navigationService.Push<IndividualSelectorViewModel>);
            RegisterIndividualsGroupCommand = new AsyncCommand(navigationService.Push<RegisterViewModel>);
            RefreshCommand = new AsyncCommand(Refresh);
            m_database = database;
        }

        public void Setup(ViewModelConfiguration configuration)
        {
            configuration.InitializeMethod = Initialize;
        }

        public async Task Initialize()
        {
            var items = m_database.GetAllGroups();
        }


        private async Task Refresh()
        {
            await Initialize();
            IsRefreshing = false;
        }

#nullable disable
        public event PropertyChangedEventHandler PropertyChanged;
#nullable restore

        public IAsyncCommand NavigateToGroupingCommand { get; }

        public IAsyncCommand RegisterIndividualsGroupCommand { get; }

        public ICommand RefreshCommand { get; }
        public bool IsRefreshing
        {
            get => m_isRefreshing;
            set => PropertyChanged.RaiseWhenSet(ref m_isRefreshing, value);
        }
    }
}