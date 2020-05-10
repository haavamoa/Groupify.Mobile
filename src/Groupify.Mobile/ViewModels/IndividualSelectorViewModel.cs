using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Groupify.Mobile.Abstractions;
using Groupify.Mobile.Services;

namespace Groupify.Mobile.ViewModels
{
    class IndividualSelectorViewModel : IViewModel
    {
        public IndividualSelectorViewModel(INavigationService navigationService)
        {
            
        }
#nullable disable
        public event PropertyChangedEventHandler PropertyChanged;
#nullable restore
        public void Setup(ViewModelConfiguration configuration) { }
    }
}
