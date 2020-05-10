using DIPS.Xamarin.UI;
using Groupify.Mobile.Abstractions;
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

        protected override async void OnStart()
        {
            await m_container.GetInstance<IDeviceDataBase>().Initialize(exception =>
            {
                MainPage = new ContentPage()
                {
                    Content = new Label() { Text = exception.Message }
                };
            });
            await NavigationService.Initialize();
        }

        public INavigationService NavigationService { get; }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

    }
}
