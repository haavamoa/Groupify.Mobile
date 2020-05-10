using System;
using System.Threading.Tasks;

namespace Groupify.Mobile.Abstractions
{
    public class ViewModelConfiguration
    {
        public Func<Task> InitializeMethod { get; set; }
    }
}