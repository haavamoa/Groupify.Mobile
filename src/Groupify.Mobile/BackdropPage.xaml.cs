using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Groupify.Mobile.Abstractions;
using Groupify.Mobile.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Groupify.Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BackdropPage : ContentPage
    {
        private readonly INavigationService m_navigationService;
        private BackdropMainView m_backdropMainView;

        public BackdropPage(INavigationService navigationService)
        {
            m_navigationService = navigationService;
            InitializeComponent();
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            await m_navigationService.Pop();
        }

        public BackdropMainView BackdropMainView
        {
            get => m_backdropMainView;
            set
            {
                m_backdropMainView = value; 
                OnPropertyChanged();
            }
        }

        public  async Task SetView(ContentView view)
        {
            if (!(view is BackdropMainView backdropMainView)) throw new Exception($"The view has to be of type {typeof(BackdropMainView)}");
            BackdropMainView = backdropMainView;

            AnimateBackButton();

            mainView.Content = view;
        }

        private void AnimateBackButton()
        {
            if (m_navigationService.Stack.Count > 1)
            {
                navigateBackButton.FadeTo(1.0);
            }
            else
            {
                navigateBackButton.FadeTo(0.0);
            }
        }
    }
}