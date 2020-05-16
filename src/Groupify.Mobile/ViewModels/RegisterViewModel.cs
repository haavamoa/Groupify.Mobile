using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using DIPS.Xamarin.UI.Commands;
using DIPS.Xamarin.UI.Extensions;
using Groupify.Mobile.Abstractions;
using Groupify.Mobile.Models;
using Groupify.Mobile.Services;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Groupify.Mobile.ViewModels
{
    public class RegisterViewModel : IViewModel
    {
        private readonly IDeviceDataBase m_database;
        private readonly ILogService m_logService;
        private readonly INavigationService m_navigationService;
        private readonly Group m_newGroup = new Group();
        private Individual m_newIndividual = new Individual();

        public RegisterViewModel(IDeviceDataBase database, INavigationService navigationService, ILogService logService)
        {
            m_database = database;
            m_navigationService = navigationService;
            m_logService = logService;
            AddIndividualsGroupCommand = new AsyncCommand(AddIndividualsGroup, () => !string.IsNullOrEmpty(NewGroupName) && Individuals.Count > 0);
            AddIndividualCommand = new Command(AddIndividual, () => !string.IsNullOrEmpty(NewIndividualName));
        }

#nullable disable
        public event PropertyChangedEventHandler PropertyChanged;
#nullable restore

        public ICommand AddIndividualCommand { get; }
        public ICommand AddIndividualsGroupCommand { get; }
        public ObservableCollection<Individual> Individuals { get; } = new ObservableCollection<Individual>();

        public string NewGroupName
        {
            get => m_newGroup.Name;
            set
            {
                m_newGroup.Name = value;
                ((AsyncCommand)AddIndividualsGroupCommand).RaiseCanExecuteChanged();
            }
        }
        public string NewIndividualName
        {
            get => m_newIndividual.Name;
            set
            {
                m_newIndividual.Name = value;
                ((Command)AddIndividualCommand).ChangeCanExecute();
                PropertyChanged.Raise();
            }
        }

        public void Setup(ViewModelConfiguration configuration) { }

        private void AddIndividual()
        {
            Individuals.Add(m_newIndividual);
            m_newIndividual = new Individual();
            NewIndividualName = string.Empty;
            ((AsyncCommand)AddIndividualsGroupCommand).RaiseCanExecuteChanged();
        }

        private async Task AddIndividualsGroup()
        {
            try
            {
                m_newGroup.Count = Individuals.Count;

                //Save group
                await m_database.Save(m_newGroup);

                //Save all individuals
                Individuals.ForEach(async individual =>
                {
                    individual.GroupId = m_newGroup.Id;
                    await m_database.Save(individual);
                });

                await m_navigationService.GoBackAndRefresh();
            }
            catch (Exception exception)
            {
                m_logService.Log(exception);
            }
        }
    }
}