using System;
using System.Linq;
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

        private async void TryAddIndividual(object sender, EventArgs e)
        {
            var vm = ((RegisterViewModel)BindingContext);
            if (string.IsNullOrEmpty(vm.NewIndividualName) || vm.NewIndividualName.All(c => char.IsWhiteSpace(c)))
            {
                await Task.Delay(100);
                IndividualNameEntry.Shake();
                IndividualNameEntry.Focus();
            }
            else
            {
                vm.AddIndividualCommand.Execute(null);
            }
        }

        private void TryAddIndividualsGroup(object sender, EventArgs e)
        {
            var vm = ((RegisterViewModel)BindingContext);
            if (string.IsNullOrEmpty(vm.NewGroupName) || vm.NewGroupName.All(c => char.IsWhiteSpace(c)))
            {
                NewGroupNameEntry.Shake();
                NewGroupNameEntry.Focus();
                return;
            }

            if (vm.Individuals.Count == 0)
            {
                IndividualsCountLabel.Shake();
                if(!string.IsNullOrEmpty(vm.NewGroupName))
                {
                    IndividualNameEntry.Focus();
                    return;
                }
            }
            vm.AddIndividualsGroupCommand.Execute(null);
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            var vm = ((RegisterViewModel)BindingContext);
            if (!(string.IsNullOrEmpty(vm.NewGroupName)) || vm.Individuals.Count != 0)
            {
                var shouldGoBack = await ((BackdropPage)App.Current.MainPage).Confirm("Er du sikker på at du vil avslutte registreringen og miste endringene?", "Avslutt");
                if (shouldGoBack)
                {
                    ((BackdropPage)App.Current.MainPage).GoBack();
                }
            }
            else
            {
                ((BackdropPage)App.Current.MainPage).GoBack();
            }
        }
    }
}