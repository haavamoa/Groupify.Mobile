using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using DIPS.Xamarin.UI.Commands;
using Groupify.Mobile.Abstractions;
using Groupify.Mobile.Models;

namespace Groupify.Mobile.ViewModels
{
    public class RegisterViewModel : IViewModel
    {
        private readonly IDeviceDataBase m_database;
#nullable disable
        private Group m_newIndividualsGroup;
#nullable restore

        public RegisterViewModel(IDeviceDataBase database)
        {
            m_database = database;

            AddIndividualsGroupCommand = new AsyncCommand(AddIndividualsGroup);
        }

        private async Task AddIndividualsGroup() => await m_database.Save(m_newIndividualsGroup);
        public void Setup(ViewModelConfiguration configuration) => configuration.InitializeMethod = Initialize;

        private Task Initialize()
        {
            m_newIndividualsGroup = new Group();
            return Task.CompletedTask;
        }

        private string m_newName;

        public string NewName
        {
            get => m_newName;
            set
            {
                m_newName = value;
                m_newIndividualsGroup.Name = m_newName;
            }
        }


        public ICommand AddIndividualsGroupCommand { get; }

#nullable disable
        public event PropertyChangedEventHandler PropertyChanged;
#nullable restore
    }
}