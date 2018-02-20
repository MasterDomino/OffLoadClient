using OffLoad.Core.Enums;
using OffLoad.Core.Services.Interfaces;
using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace OffLoad.Core.Services
{
    public class LoggingService : ILoggingService, IDisposable
    {
        #region Members

        public const string LogFilePath = "log.log";

        private readonly StreamWriter _stringWriter;

        private bool _disposed;

        #endregion

        #region Instantiation

        public LoggingService() => _stringWriter = new StreamWriter(LogFilePath, true);

        #endregion

        #region Properties

        public LoggingLevel LoggingLevel { get; set; }

        #endregion

        #region Methods

        public void Debug(string message, [CallerMemberName]string caller = "") => AppendToFile($"[{DateTime.Now}][Debug][{caller}]: {message}");

        public void Dispose() => Dispose(true);

        public void Error(string message, Exception ex = null, [CallerMemberName]string caller = "")
        {
            if (ex == null)
            {
                AppendToFile($"[{DateTime.Now}][Error][{caller}]: {message}");
            }
            else
            {
                AppendToFile($"[{DateTime.Now}][Error][{caller}]: {message} {ex.Message} {ex.InnerException}\r\n{ex.StackTrace}");
            }
        }

        public void Fatal(string message, Exception ex = null, [CallerMemberName]string caller = "")
        {
            if (ex == null)
            {
                AppendToFile($"[{DateTime.Now}][Fatal][{caller}]: {message}");
            }
            else
            {
                AppendToFile($"[{DateTime.Now}][Fatal][{caller}]: {message} {ex.Message} {ex.InnerException}\r\n{ex.StackTrace}");
            }
        }

        public void Info(string message, [CallerMemberName]string caller = "") => AppendToFile($"[{DateTime.Now}][Info][{caller}]: {message}");

        public void Warning(string message, Exception ex = null, [CallerMemberName]string caller = "")
        {
            if (ex == null)
            {
                AppendToFile($"[{DateTime.Now}][Warning][{caller}]: {message}");
            }
            else
            {
                AppendToFile($"[{DateTime.Now}][Warning][{caller}]: {message} {ex.Message} {ex.InnerException}\r\n{ex.StackTrace}");
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _stringWriter.Dispose();
                }
                _disposed = true;
            }
        }

        private void AppendToFile(string message) => _stringWriter.WriteLine(message);

        #endregion
    }
}