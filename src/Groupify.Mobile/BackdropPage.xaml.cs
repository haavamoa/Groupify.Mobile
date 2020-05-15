using System;
using System.Linq;
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

        public BackdropMainView BackdropMainView
        {
            get => m_backdropMainView;
            set
            {
                m_backdropMainView = value;
                OnPropertyChanged();
            }
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            await m_navigationService.GoBack();
        }

        public async Task SetView(ContentView view)
        {
            if (!(view is BackdropMainView backdropMainView)) throw new Exception($"The view has to be of type {typeof(BackdropMainView)}");
            backdropMainView.Opacity = 0;
            titleLabel.FadeTo(0);
            var previousView = BackdropMainView;
            //Fadeout previous view
            if (previousView != null)
            {
                AnimateToolbarItemButton(backdropMainView, previousView);
                await previousView.FadeTo(0);
            }

            BackdropMainView = backdropMainView;

            _ = titleLabel.FadeTo(1);

            _ = BackdropMainView.FadeTo(1);

            AnimateBackButton(previousView);
            AnimateToolbarItemButton(BackdropMainView, previousView);

            mainView.Content = view;
        }

        private void AnimateToolbarItemButton(BackdropMainView current, BackdropMainView? previous)
        {
            if (current == null) return;

            if (current.ToolbarItemCommand != null && previous == null)
            {
                toolbarItemButton.Opacity = 0;
                toolbarItemButton.TranslationY = navigateBackButton.TranslationY - 20;
                toolbarItemButton.TranslateTo(0, 0);
                toolbarItemButton.FadeTo(1);
            }

            if (current.ToolbarItemCommand != null && previous?.ToolbarItemCommand != null) return;

            if (current.ToolbarItemCommand != null && previous?.ToolbarItemCommand == null)
            {
                toolbarItemButton.Opacity = 0;
                toolbarItemButton.TranslationY = navigateBackButton.TranslationY - 20;
                toolbarItemButton.TranslateTo(0, 0);
                toolbarItemButton.FadeTo(1);
            }

            if (current.ToolbarItemCommand == null && previous?.ToolbarItemCommand != null)
            {
                toolbarItemButton.FadeTo(0);
                toolbarItemButton.TranslateTo(0, navigateBackButton.TranslationY - 20);
            }
        }

        private bool TryOpenToolbarItem(BackdropMainView view)
        {
            if (view.ToolbarItemCommand != null)
            {
                toolbarItemButton.Opacity = 0;
                toolbarItemButton.TranslationY = navigateBackButton.TranslationY - 20;
                toolbarItemButton.TranslateTo(0, 0);
                toolbarItemButton.FadeTo(1);
                return true;
            }

            return false;
        }

        private void AnimateBackButton(BackdropMainView? previousView)
        {
            if (previousView == null) return;

            if (BackdropMainView == m_navigationService.Stack.Last())
            {
                navigateBackButton.IsVisible = false;
                return;
            }

            navigateBackButton.IsVisible = true;
            if (previousView == m_navigationService.Stack.Last())
            {
                navigateBackButton.Opacity = 0;
                navigateBackButton.TranslationY = navigateBackButton.TranslationY - 20;
                navigateBackButton.TranslateTo(0, 0);
                navigateBackButton.FadeTo(1);
            }
        }
    }
}