using System;
using Groupify.Mobile.Abstractions;
using Groupify.Mobile.Repository;
using Groupify.Mobile.Services;
using Groupify.Mobile.ViewModels;
using LightInject;

namespace Groupify.Mobile
{
    public class CompositionRoot : ICompositionRoot
    {
        public static Action<Exception> OnDatabaseExceptions { get; internal set; }

        public void Compose(IServiceRegistry serviceRegistry)
        {
            serviceRegistry.Register<INavigationService>(factory => new NavigationService(factory), new PerContainerLifetime());
            serviceRegistry.RegisterViewModels();
            serviceRegistry.Register<IDeviceDataBase>(fact => new DeviceDatabase(OnDatabaseExceptions), lifetime: new PerContainerLifetime());
        }
    }

    public static class CompositionRootExtensions
    {
        public static void RegisterViewModels(this IServiceRegistry serviceRegistry)
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

