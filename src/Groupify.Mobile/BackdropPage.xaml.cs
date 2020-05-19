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
        private double m_confirmationPopupFramePosition;
        private TaskCompletionSource<bool> m_confirmationTaskCompletetionSource;

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

        public Task<bool> ConfirmDeletion()
        {
            m_confirmationTaskCompletetionSource = new TaskCompletionSource<bool>();
            ConfirmationPopupFrame.TranslateTo(0, m_confirmationPopupFramePosition, easing: Easing.SinOut);
            Overlay.FadeTo(0.4);
            Overlay.InputTransparent = false;
            return m_confirmationTaskCompletetionSource.Task;
        }

        public async Task SetView(ContentView view)
        {
            if (!(view is BackdropMainView backdropMainView))
            {
                throw new Exception($"The view has to be of type {typeof(BackdropMainView)}");
            }

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

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            RemoveConfirmationPopup();
        }

        private void AnimateBackButton(BackdropMainView? previousView)
        {
            if (previousView == null)
            {
                return;
            }

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

        private void AnimateToolbarItemButton(BackdropMainView current, BackdropMainView? previous)
        {
            if (current == null)
            {
                return;
            }

            if (current.ToolbarItemCommand != null && previous == null)
            {
                toolbarItemButton.Opacity = 0;
                toolbarItemButton.TranslationY = navigateBackButton.TranslationY - 20;
                toolbarItemButton.TranslateTo(0, 0);
                toolbarItemButton.FadeTo(1);
            }

            if (current.ToolbarItemCommand != null && previous?.ToolbarItemCommand != null)
            {
                return;
            }

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

        private async void CancelConfirmation(object sender, EventArgs e)
        {
            await Task.WhenAll(
                ConfirmationPopupFrame.TranslateTo(0, Height, easing: Easing.SinIn),
                Overlay.FadeTo(0)
            );
            Overlay.InputTransparent = true;
            m_confirmationTaskCompletetionSource.SetResult(false);
        }

        private async void ConfirmConfirmation(object sender, EventArgs e)
        {
            await Task.WhenAll(
                ConfirmationPopupFrame.TranslateTo(0, Height, easing: Easing.SinIn),
                Overlay.FadeTo(0)
            );
            Overlay.InputTransparent = true;
            m_confirmationTaskCompletetionSource.SetResult(true);
        }

        private void OnBackClicked(object sender, EventArgs e)
        {
            Back();
        }

        private void RemoveConfirmationPopup()
        {
            m_confirmationPopupFramePosition = ConfirmationPopupFrame.TranslationY;
            ConfirmationPopupFrame.TranslationY = Height;
        }

        protected override bool OnBackButtonPressed()
        {
            return Back();
        }

        private bool Back()
        {
            if (m_navigationService.Stack.Count == 1)
            {
                if (m_backdropMainView.CanInvokeBackClicked())
                {
                    m_backdropMainView.InvokeBackClicked();
                }
                return true;
            }

            if (m_backdropMainView.CanInvokeBackClicked())
            {
                m_backdropMainView.InvokeBackClicked();
                return true;
            }
            else
            {
                m_navigationService.GoBack();
                return true;
            }
        }
    }
}