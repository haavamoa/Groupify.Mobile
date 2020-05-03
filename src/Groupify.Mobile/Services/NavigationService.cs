using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Groupify.Mobile.Abstractions;
using Groupify.Mobile.ViewModels;
using Groupify.Mobile.Views;
using LightInject;
using Xamarin.Forms;

namespace Groupify.Mobile.Services
{
    public class NavigationService : INavigationService, INotifyPropertyChanged
    {
        private readonly Dictionary<IViewModel, ContentView> m_lookup = new Dictionary<IViewModel, ContentView>();
        private readonly IServiceFactory m_serviceFactory;

        public NavigationService(IServiceFactory serviceFactory)
        {
            m_serviceFactory = serviceFactory;
        }

        public async Task Initialize()
        {
            var overviewViewModel = m_serviceFactory.GetInstance<OverviewViewModel>();
            var overView = new Overview();
            await InternalPush(overviewViewModel, overView, async viewmodel => await viewmodel.Initialize());

            m_lookup.Add(overviewViewModel, overView);
            m_lookup.Add(m_serviceFactory.GetInstance<RegisterViewModel>(), new RegisterView());
            m_lookup.Add(m_serviceFactory.GetInstance<GroupSelectorViewModel>(), new GroupSelectorView());
            m_lookup.Add(m_serviceFactory.GetInstance<GroupsOverviewViewModel>(), new GroupsOverview());
            m_lookup.Add(m_serviceFactory.GetInstance<IndividualDetailViewModel>(), new IndividualDetailView());
            m_lookup.Add(m_serviceFactory.GetInstance<IndividualSelectorViewModel>(), new IndividualSelectorView());
        }

        public async Task Pop()
        {
            var viewModelToPop = Stack.Pop();
            var view = InternalGetView(viewModelToPop);

            await ((BackdropPage)Application.Current.MainPage).RemoveView(view);
            await ((BackdropPage)Application.Current.MainPage).SetView(InternalGetView(Stack.Peek()));
        }

        public async Task Push<TViewModel>(Action<TViewModel> beforeNavigation) where TViewModel : IViewModel
        {
            var viewmodel = m_serviceFactory.GetInstance<TViewModel>();

            var view = InternalGetView(viewmodel);

            await InternalPush(viewmodel, view, beforeNavigation);
        }

        public async Task Push<TViewModel>() where TViewModel : IViewModel
        {
            await Push<TViewModel>(viewModel => { });
        }

        public Stack<IViewModel> Stack { get; } = new Stack<IViewModel>();
        public event PropertyChangedEventHandler PropertyChanged;

        private ContentView InternalGetView<TViewModel>(TViewModel viewmodel) where TViewModel : IViewModel
        {
            if (!m_lookup.TryGetValue(viewmodel, out var view)) throw new Exception("View not found when navigating");

            return view;
        }

        private async Task InternalPush<TViewModel>(TViewModel viewmodel, ContentView view, Action<TViewModel> beforeNavigation)
            where TViewModel : IViewModel
        {
            Stack.Push(viewmodel);
            view.BindingContext = viewmodel;
            beforeNavigation?.Invoke(viewmodel);
            await ((BackdropPage)Application.Current.MainPage).SetView(view);
        }
    }
}