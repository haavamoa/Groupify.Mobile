using System.ComponentModel;
using System.Threading.Tasks;
using Groupify.Mobile.Abstractions;
using Groupify.Mobile.Services;

namespace Groupify.Mobile.ViewModels
{
    public class OverviewViewModel : IViewModel
    {
        public OverviewViewModel(INavigationService navigationService)
        {
            
        }
#nullable disable
        public event PropertyChangedEventHandler PropertyChanged;
#nullable restore

        public async Task Initialize()
        {
            
        }
    }
}