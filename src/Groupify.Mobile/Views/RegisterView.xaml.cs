using System;
using System.Threading.Tasks;
using Groupify.Mobile.Extensions;
using Groupify.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Groupify.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterView
    {
        public RegisterView()
        {
            InitializeComponent();
        }

        private void FocusIndividualNameEntry(object sender, EventArgs e)
        {
            IndividualNameEntry.Focus();
        }

        private void ValidateCriteriaToRegister(object sender, EventArgs e)
        {
            var vm = ((RegisterViewModel)BindingContext);
            if (string.IsNullOrEmpty(vm.NewGroupName))
            {
                NewGroupNameEntry.Shake();
                NewGroupNameEntry.Focus();
            }

            if (vm.Individuals.Count == 0)
            {
                IndividualsCountLabel.Shake();
                if(!string.IsNullOrEmpty(vm.NewGroupName))
                {
                    IndividualNameEntry.Focus();
                }
            }
        }

       
    }
}