using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Groupify.Mobile.Abstractions {
    public interface INavigationService {
        Task Pop();
        Stack<IViewModel> Stack { get; }
        Task Initialize();
        Task Push<TViewModel>(Action<TViewModel> beforeNavigation) where TViewModel : IViewModel;
        Task Push<TViewModel>() where TViewModel : IViewModel;
    }
}