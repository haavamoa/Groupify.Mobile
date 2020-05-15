using System;
using System.ComponentModel;
using System.Threading.Tasks;
using DIPS.Xamarin.UI.Commands;
using DIPS.Xamarin.UI.Extensions;
using Groupify.Mobile.Abstractions;
using Groupify.Mobile.Models;
using Groupify.Mobile.Services;

namespace Groupify.Mobile.ViewModels
{
    internal class IndividualSelectorViewModel : IViewModel
    {
        private Group m_group;
        private readonly IDeviceDataBase m_deviceDataBase;
        private readonly INavigationService m_navigationService;
        private readonly ILogService m_logService;

        public IndividualSelectorViewModel(IDeviceDataBase deviceDataBase, INavigationService navigationService, ILogService logService)
        {
            DeleteGroupCommand = new AsyncCommand(DeleteGroup);
            m_deviceDataBase = deviceDataBase;
            m_navigationService = navigationService;
            m_logService = logService;
        }

        private async Task DeleteGroup()
        {
            try
            {
                await m_deviceDataBase.Delete(Group);

                await m_navigationService.GoBackAndRefresh();
            }
            catch (Exception exception)
            {
                m_logService.Log(exception);
            }
        }

#nullable disable
        public event PropertyChangedEventHandler PropertyChanged;
#nullable restore
        public Group Group
        {
            get => m_group;
            set => PropertyChanged.RaiseWhenSet(ref m_group, value);
        }

        public IAsyncCommand DeleteGroupCommand { get; }

        public void Setup(ViewModelConfiguration configuration) { }

        internal void Prepare(Group selectedGroup)
        {
            Group = selectedGroup;
        }
    }
}
