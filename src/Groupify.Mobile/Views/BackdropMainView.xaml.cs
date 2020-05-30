using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Groupify.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BackdropMainView : ContentView
    {
        public BackdropMainView()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(BackdropMainView));

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public static readonly BindableProperty ToolbarItemCommandProperty = BindableProperty.Create(nameof(ToolbarItemCommand), typeof(ICommand), typeof(BackdropMainView));

        public ICommand ToolbarItemCommand
        {
            get => (ICommand)GetValue(ToolbarItemCommandProperty);
            set => SetValue(ToolbarItemCommandProperty, value);
        }

        public static readonly BindableProperty ToolbarItemImageSourceProperty = BindableProperty.Create(nameof(ToolbarItemImageSource), typeof(ImageSource), typeof(BackdropMainView));

        public ImageSource ToolbarItemImageSource
        {
            get => (ImageSource)GetValue(ToolbarItemImageSourceProperty);
            set => SetValue(ToolbarItemImageSourceProperty, value);
        }


        public bool CanInvokeBackClicked()
        {
            return BackClicked?.GetInvocationList().Length > 0;
        }

        public void InvokeBackClicked()
        {
            BackClicked?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler BackClicked;
        public event EventHandler<bool> HighlightToolbarItemChanged;


        public bool HighlightToolbarItem
        {
            get => (bool)GetValue(HighlightToolbarItemProperty);
            set => SetValue(HighlightToolbarItemProperty, value);
        }

        public static readonly BindableProperty HighlightToolbarItemProperty = BindableProperty.Create(nameof(HighlightToolbarItem), typeof(bool), typeof(BackdropPage), propertyChanged:asd);

        private static void asd(BindableObject bindable, object oldValue, object newValue)
        {
            if(!(bindable is BackdropMainView backdropMainView)) return;
            backdropMainView.HighlightToolbarItemChanged?.Invoke(bindable, (bool)newValue);
        }
    }
}