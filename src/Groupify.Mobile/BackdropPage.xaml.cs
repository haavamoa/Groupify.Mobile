using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Groupify.Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [ContentProperty(nameof(MainContent))]
    public partial class BackdropPage : ContentPage
    {
        public BackdropPage()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty MainContentProperty = BindableProperty.Create(nameof(MainContent), typeof(View), typeof(BackdropPage));

        public View MainContent
        {
            get => (View)GetValue(MainContentProperty);
            set => SetValue(MainContentProperty, value);
        }
    }
}