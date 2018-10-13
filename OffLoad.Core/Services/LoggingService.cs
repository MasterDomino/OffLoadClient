using OffLoad.Core.Services.Interfaces;
using System;
using System.IO;

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

        public void Dispose() => Dispose(true);

        public void LogUndownloaded(string[] undownloads)
        {
            if (undownloads.Length > 0)
            {
                for (int i = 0; i < undownloads.Length; i++)
                {
                    AppendToFile("[Undownloaded][OffLoadClient]: " + undownloads[i]);
                }
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