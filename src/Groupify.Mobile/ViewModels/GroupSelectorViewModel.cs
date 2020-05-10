using System.ComponentModel;
using System.Runtime.CompilerServices;
using Groupify.Mobile.Abstractions;
using Groupify.Mobile.Services;

namespace Groupify.Mobile.ViewModels
{
    public class GroupSelectorViewModel : IViewModel
    {
        public GroupSelectorViewModel(INavigationService navigationService) { }
#nullable disable
        public event PropertyChangedEventHandler PropertyChanged;
#nullable restore
        public void Setup(ViewModelConfiguration configuration) { }
    }
}