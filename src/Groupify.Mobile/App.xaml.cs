﻿using DIPS.Xamarin.UI;
using Groupify.Mobile.Abstractions;
using Groupify.Mobile.Services;
using LightInject;
using Xamarin.Forms;

namespace Groupify.Mobile
{
    public partial class App : Application
    {
        private INavigationService m_navigationService;

        public App()
        {
            InitializeComponent();
            Library.Initialize();
            var container = new ServiceContainer(new ContainerOptions { EnablePropertyInjection = false });
            container.RegisterFrom<CompositionRoot>();
            m_navigationService = container.GetInstance<INavigationService>();

            MainPage = new BackdropPage(m_navigationService);
        }

        protected override async void OnStart()
        {
            await m_navigationService.Initialize();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}