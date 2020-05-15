using System;
using System.Threading.Tasks;

namespace Groupify.Mobile.Abstractions
{
    public class ViewModelConfiguration
    {
        /// <summary>
        /// This method get's run when the view model get's navigated to
        /// </summary>
        public Func<Task>? InitializeMethod { get; set; }

        /// <summary>
        /// This method get's run when the app has gotten a message that it should refresh through navigation
        /// </summary>
        public Func<Task>? RefreshingMethod { get; set; }
    }
}