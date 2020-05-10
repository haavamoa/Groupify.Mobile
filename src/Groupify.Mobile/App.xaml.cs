using DIPS.Xamarin.UI;
using Groupify.Mobile.Abstractions;
using Groupify.Mobile.Repository;
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
            CompositionRoot.OnDatabaseExceptions = exception =>
            {
                App.Current.MainPage = new ContentPage()
                {
                    Content = new Label() { Text = exception.Message }
                };
            };

            container.RegisterFrom<CompositionRoot>();
            NavigationService = container.GetInstance<INavigationService>();

            MainPage = new BackdropPage(NavigationService);
        }

        protected override async void OnStart() => await NavigationService.Initialize();

        public INavigationService NavigationService { get; }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

    }
}
