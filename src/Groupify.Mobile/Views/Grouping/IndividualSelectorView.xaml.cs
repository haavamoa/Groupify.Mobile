using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Groupify.Mobile.ViewModels.Grouping;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Groupify.Mobile.Views.Grouping
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IndividualSelectorView : ContentView
    {
        public IndividualSelectorView()
        {
            InitializeComponent();
        }

        private void ToggleIsAllSelected(object sender, EventArgs e)
        {
            ((IndividualSelectorViewModel)BindingContext).IsAllSelected = !((IndividualSelectorViewModel)BindingContext).IsAllSelected;
        }
    }
}