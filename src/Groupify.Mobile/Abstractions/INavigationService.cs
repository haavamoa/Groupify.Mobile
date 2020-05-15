using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Groupify.Mobile.Abstractions {
    public interface INavigationService {
        Task GoBack();
        Stack<ContentView> Stack { get; }
        Task Initialize();
        Task Push<TViewModel>(Action<TViewModel> beforeNavigation) where TViewModel : IViewModel;
        Task Push<TViewModel>() where TViewModel : IViewModel;
        Task GoBackAndRefresh();
    }
}