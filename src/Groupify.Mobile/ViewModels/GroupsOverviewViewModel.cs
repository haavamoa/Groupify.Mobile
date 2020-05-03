using System.ComponentModel;
using Groupify.Mobile.Abstractions;
using Groupify.Mobile.Services;

namespace Groupify.Mobile.ViewModels
{
    public class GroupsOverviewViewModel : IViewModel
    {
        public GroupsOverviewViewModel(INavigationService navigationService)
        {
        }
#nullable disable
        public event PropertyChangedEventHandler PropertyChanged;
#nullable restore

    }
}