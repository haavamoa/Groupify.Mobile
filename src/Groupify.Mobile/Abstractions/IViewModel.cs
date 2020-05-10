using System.ComponentModel;

namespace Groupify.Mobile.Abstractions
{
    public interface IViewModel : INotifyPropertyChanged
    {
        void Setup(ViewModelConfiguration configuration);
    }
}