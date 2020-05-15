using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Groupify.Mobile.Services
{
    public class LogService : ILogService
    {
        public void Log(Exception exception)
        {
            var stackLayout = new StackLayout();
            stackLayout.Children.Add(new Label() { Text = exception.Message });
            stackLayout.Children.Add(new Label() { Text = exception.StackTrace });
            Application.Current.MainPage = new ContentPage() { Content = stackLayout };
        }
    }
}
