using System;
using Groupify.Mobile.Abstractions;
using Groupify.Mobile.Repository;
using Groupify.Mobile.Services;
using Groupify.Mobile.ViewModels;
using Groupify.Mobile.ViewModels.Grouping;
using LightInject;

namespace Groupify.Mobile
{
    public class CompositionRoot : ICompositionRoot
    {
        public static Action<Exception> OnDatabaseExceptions { get; internal set; }

        public void Compose(IServiceRegistry serviceRegistry)
        {
            
            serviceRegistry.Register<INavigationService>(factory => new NavigationService(factory, factory.GetInstance<ILogService>()), new PerContainerLifetime());
            serviceRegistry.RegisterServices();
            serviceRegistry.RegisterViewModels();
            serviceRegistry.Register<IDeviceDataBase, DeviceDatabase>(new PerContainerLifetime());
        }
    }

    public static class CompositionRootExtensions
    {
        public static void RegisterViewModels(this IServiceRegistry serviceRegistry)
        {
            serviceRegistry.Register<OverviewViewModel>();
            serviceRegistry.Register<RegisterViewModel>();
            serviceRegistry.Register<GroupingViewModel>();
            serviceRegistry.Register<GroupSelectorViewModel>();
            serviceRegistry.Register<GroupsOverviewViewModel>();
            serviceRegistry.Register<IndividualSelectorViewModel>();
        }

        public static void RegisterServices(this IServiceRegistry serviceRegistry)
        {
            serviceRegistry.Register<ILogService, LogService>(new PerContainerLifetime());
        }
    }
}

