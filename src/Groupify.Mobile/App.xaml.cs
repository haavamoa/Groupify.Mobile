using System;
using DIPS.Xamarin.UI;
using Groupify.Mobile.Abstractions;
using Groupify.Mobile.Services;
using LightInject;
using Xamarin.Forms;

namespace Groupify.Mobile
{
    public partial class App : Application
    {
        private readonly ServiceContainer m_container;

        public App()
        {
            InitializeComponent();
            Library.Initialize();
            m_container = new ServiceContainer(new ContainerOptions { EnablePropertyInjection = false });

            m_container.RegisterFrom<CompositionRoot>();
            NavigationService = m_container.GetInstance<INavigationService>();

            MainPage = new BackdropPage(NavigationService);
        }

        public INavigationService NavigationService { get; }

        protected override void OnResume()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override async void OnStart()
        {
            try
            {
                await m_container.GetInstance<IDeviceDataBase>().Initialize();
                await NavigationService.Initialize();
            }
            catch (Exception exception)
            {

                m_container.GetInstance<ILogService>().Log(exception);
            }
        }
    }
}
