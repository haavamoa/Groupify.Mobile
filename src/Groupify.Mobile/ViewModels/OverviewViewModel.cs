using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using DIPS.Xamarin.UI.Commands;
using DIPS.Xamarin.UI.Extensions;
using Groupify.Mobile.Abstractions;
using Groupify.Mobile.Models;

namespace Groupify.Mobile.ViewModels
{
    public class OverviewViewModel : IViewModel
    {
        private readonly IDeviceDataBase m_database;
        private bool m_isRefreshing;
        public OverviewViewModel(INavigationService navigationService, IDeviceDataBase database)
        {
            NavigateToGroupingCommand = new AsyncCommand(navigationService.Push<IndividualSelectorViewModel>);
            RegisterIndividualsGroupCommand = new AsyncCommand(navigationService.Push<RegisterViewModel>);
            RefreshCommand = new AsyncCommand(Refresh);
            m_database = database;
        }
#nullable disable
        public event PropertyChangedEventHandler PropertyChanged;
#nullable restore

        public ObservableCollection<Group> Groups { get; } = new ObservableCollection<Group>();

        public bool IsRefreshing
        {
            get => m_isRefreshing;
            set => PropertyChanged.RaiseWhenSet(ref m_isRefreshing, value);
        }

        public IAsyncCommand NavigateToGroupingCommand { get; }

        public ICommand RefreshCommand { get; }

        public IAsyncCommand RegisterIndividualsGroupCommand { get; }

        public async Task Initialize()
        {
            var groups = await m_database.GetAllGroups();
            groups.ForEach(g => Groups.Add(g));
        }

        public void Setup(ViewModelConfiguration configuration)
        {
            configuration.InitializeMethod = Initialize;
            configuration.RefreshingMethod = () =>
            {
                IsRefreshing = true;
                return Task.CompletedTask;
            };
        }

        private async Task Refresh()
        {
            var groups = await m_database.GetAllGroups();
            Groups.Clear();
            groups.ForEach(g => Groups.Add(g));
            IsRefreshing = false;
        }
    }
}