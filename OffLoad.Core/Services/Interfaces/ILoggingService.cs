using System;
using System.Runtime.CompilerServices;

namespace OffLoad.Core.Services.Interfaces
{
    public interface ILoggingService
    {
        #region Methods

        void Debug(string message, [CallerMemberName]string caller = "");

        void Dispose();

        void Error(string message, Exception ex = null, [CallerMemberName]string caller = "");

        void Fatal(string message, Exception ex = null, [CallerMemberName]string caller = "");

        void Info(string message, [CallerMemberName]string caller = "");

        void LogUndownloaded(string[] undownloads);

        void Warning(string message, Exception ex = null, [CallerMemberName]string caller = "");

        #endregion
    }
}