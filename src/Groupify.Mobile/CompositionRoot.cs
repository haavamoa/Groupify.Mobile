using System;
using Groupify.Mobile.Abstractions;
using Groupify.Mobile.Services;
using Groupify.Mobile.ViewModels;
using LightInject;

namespace Groupify.Mobile
{
    public class CompositionRoot : ICompositionRoot
    {
        public void Compose(IServiceRegistry serviceRegistry)
        {
            serviceRegistry.Register<INavigationService>(factory => new NavigationService(factory), new PerContainerLifetime());
            RegisterViewModels(serviceRegistry);
        }

        private void RegisterViewModels(IServiceRegistry serviceRegistry)
        {
            serviceRegistry.Register<OverviewViewModel>();
            serviceRegistry.Register<RegisterViewModel>();
            serviceRegistry.Register<GroupSelectorViewModel>();
            serviceRegistry.Register<GroupsOverviewViewModel>();
            serviceRegistry.Register<IndividualDetailViewModel>();
            serviceRegistry.Register<IndividualSelectorViewModel>();
        }
    }
}