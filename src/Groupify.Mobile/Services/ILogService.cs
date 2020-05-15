using System;

namespace Groupify.Mobile.Services
{
    public interface ILogService
    {
        void Log(Exception exception);
    }
}