using System;
using System.Threading.Tasks;

namespace Groupify.Mobile.Abstractions
{
    public class ViewModelConfiguration
    {
        /// <summary>
        /// This method get's run when the navigationservice initializes
        /// </summary>
        public Func<Task>? InitializeMethod { get; set; }
    }
}