using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DIPS.Xamarin.UI.Controls.Popup;
using DIPS.Xamarin.UI.Extensions;
using Groupify.Mobile.Abstractions;
using Groupify.Mobile.Extensions;
using Groupify.Mobile.Views;
using Xamarin.Forms;
using Xamarin.Forms.Markup;
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



        public string ConfirmTitle
        {
            get => (string)GetValue(ConfirmTitleProperty);
            set => SetValue(ConfirmTitleProperty, value);
        }

        public static readonly BindableProperty ConfirmTitleProperty = BindableProperty.Create(nameof(ConfirmTitle), typeof(string), typeof(BackdropPage));



        public string ConfirmMessage
        {
            get => (string)GetValue(ConfirmMessageProperty);
            set => SetValue(ConfirmMessageProperty, value);
        }

        public static readonly BindableProperty ConfirmMessageProperty = BindableProperty.Create(nameof(ConfirmMessage), typeof(string), typeof(BackdropPage));


        public Task<bool> Confirm(string confirmationTitleText, string confirmationText)
        {
            m_confirmationTaskCompletetionSource = new TaskCompletionSource<bool>();
            ConfirmationTitleLabel.Text = confirmationTitleText;
            ConfirmConfirmationButton.Text = confirmationText;
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
            //Fadeout previous view and dispose events
            if (previousView != null)
            {
                AnimateToolbarItemButton(backdropMainView, previousView);
                await previousView.FadeTo(0);

                if(!m_navigationService.Stack.Contains(previousView))
                {
                    DisposeBackdropMainView(previousView);
                }
                
            }

            BackdropMainView = backdropMainView;
            InitializeBackdropMainView(BackdropMainView);

            _ = titleLabel.FadeTo(1);

            _ = BackdropMainView.FadeTo(1);

            AnimateBackButton(previousView);
            AnimateToolbarItemButton(BackdropMainView, previousView);

            mainView.Content = backdropMainView;
        }

        private CancellationTokenSource m_highlightToolbarItemCancellationToken = new CancellationTokenSource();

        private void InitializeBackdropMainView(BackdropMainView mainView)
        {
           mainView.HighlightToolbarItemChanged += OnBackdropMainViewPropertyChanged;
           if(mainView.HighlightToolbarItem)
            {
                BounceToolBarItem();
            }
        }

        private void DisposeBackdropMainView(BackdropMainView mainView)
        {
            mainView.HighlightToolbarItemChanged -= OnBackdropMainViewPropertyChanged;
        }

        private void OnBackdropMainViewPropertyChanged(object sender, bool isHighlightingToolbarItem)
        {
            if(!(isHighlightingToolbarItem))
            {
                m_highlightToolbarItemCancellationToken.Cancel();
                m_highlightToolbarItemCancellationToken = new CancellationTokenSource();
            }
            else
            {
                BounceToolBarItem();
            }
        }

        private void BounceToolBarItem()
        {
            toolbarItemButton.Bounce(m_highlightToolbarItemCancellationToken.Token);
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            RemoveConfirmationPopups();
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

        private void RemoveConfirmationPopups()
        {
            if(m_confirmationTaskCompletetionSource?.Task != null) //Because setting the text will result in the view getting resized, we need to check if we are about to move the frame to not hide it
            {
                if (!m_confirmationTaskCompletetionSource.Task.IsCompleted)
                    return;
            }
            
            m_confirmationPopupFramePosition = ConfirmationPopupFrame.TranslationY;
            ConfirmationPopupFrame.TranslationY = Height;
        }

        protected override bool OnBackButtonPressed()
        {
            return Back();
        }

        public void GoBack()
        {
            m_navigationService.GoBack();
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
                GoBack();
                return true;
            }
        }



    }
}