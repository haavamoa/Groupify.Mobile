using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.WebSockets;
using System.Threading.Tasks;
using DIPS.Xamarin.UI.Extensions;
using Groupify.Mobile.Abstractions;
using Groupify.Mobile.ViewModels;
using Groupify.Mobile.Views;
using LightInject;
using Xamarin.Forms;

namespace Groupify.Mobile.Services
{
    public class NavigationService : INavigationService, INotifyPropertyChanged
    {
        private readonly Dictionary<Type, Func<ContentView>> m_lookup = new Dictionary<Type, Func<ContentView>>();
        private readonly IServiceFactory m_serviceFactory;
        private readonly ILogService m_logService;

        public NavigationService(IServiceFactory serviceFactory, ILogService logService)
        {
            m_serviceFactory = serviceFactory;
            m_logService = logService;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Stack<ContentView> Stack { get; } = new Stack<ContentView>();

        public async Task Initialize()
        {
            var overviewViewModel = m_serviceFactory.GetInstance<OverviewViewModel>();
            var overView = new Overview();
            await InternalPush(overviewViewModel, overView, vm => { });

            m_lookup.Add(typeof(OverviewViewModel), () => overView);
            m_lookup.Add(typeof(RegisterViewModel), () => new RegisterView());
            m_lookup.Add(typeof(GroupSelectorViewModel), () => new GroupSelectorView());
            m_lookup.Add(typeof(GroupsOverviewViewModel), () => new GroupsOverview());
            m_lookup.Add(typeof(IndividualDetailViewModel), () => new IndividualDetailView());
            m_lookup.Add(typeof(IndividualSelectorViewModel), () => new IndividualSelectorView());
        }

        public async Task Pop()
        {
            Stack.Pop();
            PropertyChanged.Raise(nameof(Stack));
            await ((BackdropPage)Application.Current.MainPage).SetView(Stack.Peek());
        }

        public async Task Push<TViewModel>(Action<TViewModel> beforeNavigation) where TViewModel : IViewModel
        {
            var viewmodel = m_serviceFactory.GetInstance<TViewModel>();

            var view = InternalGetView<TViewModel>();

            await InternalPush(viewmodel, view, beforeNavigation);
        }

        public async Task Push<TViewModel>() where TViewModel : IViewModel
        {
            await Push<TViewModel>(viewModel => { });
        }
        private ContentView InternalGetView<TViewModel>() where TViewModel : IViewModel
        {
            if (!m_lookup.TryGetValue(typeof(TViewModel), out var factory))
            {
                throw new Exception("View not found when navigating");
            }

            return factory();
        }

        private async Task InternalPush<TViewModel>(TViewModel viewmodel, ContentView view, Action<TViewModel> beforeNavigation)
            where TViewModel : IViewModel
        {
            try
            {
                var config = new ViewModelConfiguration();
                viewmodel.Setup(config);

                Stack.Push(view);
                PropertyChanged.Raise(nameof(Stack));
                view.BindingContext = viewmodel;
                beforeNavigation?.Invoke(viewmodel);

                if(config.InitializeMethod != null)
                {
                    _ = config.InitializeMethod();
                }
                
                await ((BackdropPage)Application.Current.MainPage).SetView(view);
            }
            catch (Exception e)
            {
                m_logService.Log(e);
            }
        }
    }
}