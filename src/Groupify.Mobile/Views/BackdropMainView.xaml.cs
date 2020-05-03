using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}