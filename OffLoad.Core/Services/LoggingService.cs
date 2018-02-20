using OffLoad.Core.Services.Interfaces;
using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace OffLoad.Core.Services
{
    public class LoggingService : ILoggingService, IDisposable
    {
        #region Members

        public string FilePath = "log.log";

        private readonly StreamWriter _stream;

        private bool _disposed;

        #endregion

        #region Instantiation

        public LoggingService() => _stream = new StreamWriter(FilePath, true);

        public LoggingService(string filePath)
        {
            FilePath = filePath;
            _stream = new StreamWriter(FilePath, true);
        }

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

        public void LogUndownloaded(string[] undownloads)
        {
            if (undownloads.Length > 0)
            {
                for (int i = 0; i < undownloads.Length; i++)
                {
                    AppendToFile($"[Undownloaded][OffLoadClient]: {undownloads[i]}");
                }
            }
        }

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
                    _stream.Close();
                    _stream.Dispose();
                }
                _disposed = true;
            }
        }

        private void AppendToFile(string message)
        {
            _stream.WriteLine(message);
            _stream.Flush();
        }

        #endregion
    }
}