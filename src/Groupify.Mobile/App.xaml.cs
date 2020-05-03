using DIPS.Xamarin.UI;
using Groupify.Mobile.Abstractions;
using Groupify.Mobile.Services;
using LightInject;
using Xamarin.Forms;

namespace Groupify.Mobile
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            Library.Initialize();
            var container = new ServiceContainer(new ContainerOptions { EnablePropertyInjection = false });
            container.RegisterFrom<CompositionRoot>();
            NavigationService = container.GetInstance<INavigationService>();

            MainPage = new BackdropPage(NavigationService);
        }

        protected override async void OnStart()
        {
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
