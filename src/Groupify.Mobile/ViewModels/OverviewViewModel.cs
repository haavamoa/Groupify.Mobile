using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using DIPS.Xamarin.UI.Commands;
using DIPS.Xamarin.UI.Extensions;
using Groupify.Mobile.Abstractions;
using Groupify.Mobile.Models;
using Groupify.Mobile.Services;
using Groupify.Mobile.ViewModels.Grouping;
using Xamarin.Forms;

namespace Groupify.Mobile.ViewModels
{
    public class OverviewViewModel : IViewModel
    {
        private readonly IDeviceDataBase m_database;
        private readonly ILogService m_logService;
        private readonly INavigationService m_navigationService;
        private bool m_isRefreshing;
        public OverviewViewModel(INavigationService navigationService, IDeviceDataBase database, ILogService logService)
        {
            NavigateToGroupingCommand = new AsyncCommand<Group>(NavigateToGrouping);
            RegisterIndividualsGroupCommand = new AsyncCommand(navigationService.Push<RegisterViewModel>);
            RefreshCommand = new AsyncCommand(Refresh);
            DeleteCommand = new AsyncCommand<Group>(Delete);
            EditCommand = new AsyncCommand<Group>(NavigateToEditGroup);

            m_navigationService = navigationService;
            m_database = database;
            m_logService = logService;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public IAsyncCommand<Group> DeleteCommand { get; }

        public IAsyncCommand<Group> EditCommand { get; }

        public ObservableCollection<Group> Groups { get; } = new ObservableCollection<Group>();


        public bool IsRefreshing
        {
            get => m_isRefreshing;
            set => PropertyChanged.RaiseWhenSet(ref m_isRefreshing, value);
        }

        public IAsyncCommand<Group> NavigateToGroupingCommand { get; }

        public ICommand RefreshCommand { get; }

        public IAsyncCommand RegisterIndividualsGroupCommand { get; }

        public async Task Initialize()
        {
            try
            {
#if DEBUG
                //await GetAllFromDatabase(); //Use this to debug
#endif

                var groups = await m_database.GetAllGroups();
                AddGroups(groups);
            }
            catch (Exception exception)
            {

                m_logService.Log(exception);
            }
        }

        public bool HasAnyGroups => Groups.Any();


        private void AddGroups(List<Group> groups)
        {
            Groups.Clear();
            groups.ForEach(g => Groups.Add(g));
            PropertyChanged.Raise(nameof(HasAnyGroups));
        }

        private async Task GetAllFromDatabase()
        {
            var allIndividuals = await m_database.GetAllIndividuals();
            var allGroups = await m_database.GetAllGroups();
            var individualGrouping = await m_database.GetAllIndividualGroupings();
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


        private async Task Delete(Group groupToDelete)
        {
            
            try
            {
                var shouldRemove = await ((BackdropPage)Application.Current.MainPage).Confirm($"Er du sikker på at du vil slette {groupToDelete.Name}?", "Slett");
                if(shouldRemove)
                {
                    await m_database.DeleteAllIndividualGroups(groupToDelete);
                    Groups.Remove(groupToDelete);
                    PropertyChanged.Raise(nameof(HasAnyGroups));
                }
            }
            catch (Exception exception)
            {
                m_logService.Log(exception);
            }
        }

        private Task NavigateToEditGroup(Group groupToEdit)
        {
            return m_navigationService.Push<RegisterViewModel>(registerviewModel => registerviewModel.PrepareEditingGroup(groupToEdit));
        }
        private Task NavigateToGrouping(Group selectedGroup)
        {
            return m_navigationService.Push<GroupingViewModel>(groupingViewModel => groupingViewModel.Prepare(selectedGroup));
        }

        private async Task Refresh()
        {
            try
            {
                var groups = await m_database.GetAllGroups();
                AddGroups(groups);
                IsRefreshing = false;
            }
            catch (Exception exception)
            {
                m_logService.Log(exception);
            }
        }
    }
}