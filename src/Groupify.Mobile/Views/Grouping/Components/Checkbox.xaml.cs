using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Groupify.Mobile.Views.Grouping.Components
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Checkbox : ContentView
    {
        public Checkbox()
        {
            InitializeComponent();
        }




        public new Color BackgroundColor
        {
            get => (Color)GetValue(BackgroundColorProperty);
            set => SetValue(BackgroundColorProperty, value);
        }

        public static readonly BindableProperty BackgroundColorProperty = BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(Checkbox));
        public bool IsChecked
        {
            get => (bool)GetValue(IsCheckedProperty);
            set => SetValue(IsCheckedProperty, value);
        }

        public static readonly BindableProperty IsCheckedProperty = BindableProperty.Create(nameof(IsChecked), typeof(bool), typeof(Checkbox), propertyChanged:IsCheckedPropertyChanged);

        private static void IsCheckedPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is Groupify.Mobile.Views.Grouping.Components.Checkbox checkBox)) return;
            checkBox.Toggle();
        }

        public void Toggle()
        {
            var length = (uint)200;
            if(IsChecked)
            {
                CheckboxImage.LayoutTo(new Rectangle(2, 2, 20, 20),length:length, easing:Easing.SpringOut);
            }
            else
            {
                CheckboxImage.LayoutTo(new Rectangle((CheckboxImage.Width / 2)+2, (CheckboxImage.Height / 2)+2, 0, 0), length: length, easing: Easing.SpringIn);
            }
        }
    }
}