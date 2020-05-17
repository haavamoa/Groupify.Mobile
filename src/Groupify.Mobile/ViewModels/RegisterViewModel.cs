using System;
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
        private Group? m_newGroup;

        public bool IsEditing
        {
            get => m_isEditing; 
            private set => PropertyChanged.RaiseWhenSet(ref m_isEditing, value);
        }

        private Individual m_newIndividual = new Individual();
        private bool m_isEditing;

        public RegisterViewModel(IDeviceDataBase database, INavigationService navigationService, ILogService logService)
        {
            m_database = database;
            m_navigationService = navigationService;
            m_logService = logService;
            AddIndividualsGroupCommand = new AsyncCommand(AddIndividualsGroup);
            AddIndividualCommand = new Command(AddIndividual, () => !string.IsNullOrEmpty(NewIndividualName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand AddIndividualCommand { get; }
        public ICommand AddIndividualsGroupCommand { get; }
        public ObservableCollection<Individual> Individuals { get; } = new ObservableCollection<Individual>();

        public string NewGroupName
        {
            get => m_newGroup.Name;
            set => m_newGroup.Name = value;
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

        public void Setup(ViewModelConfiguration configuration)
        {
            configuration.InitializeMethod = Initialize;
        }

        private async Task Initialize()
        {
            if (m_newGroup == null)
            {
                m_newGroup = new Group();
            }
            else
            {
                var individuals = await m_database.GetIndividuals(m_newGroup.Id);
                individuals.ForEach(individual => Individuals.Add(individual));
                PropertyChanged.Raise(nameof(NewGroupName));
            }
        }

        public void PrepareEditingGroup(Group group)
        {
            m_newGroup = group;
            IsEditing = true;
        }

        private void AddIndividual()
        {
            Individuals.Add(m_newIndividual);
            m_newIndividual = new Individual();
            NewIndividualName = string.Empty;
        }

        private async Task AddIndividualsGroup()
        {
            if (string.IsNullOrEmpty(NewGroupName) || Individuals.Count == 0)
            {
                return;
            }

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