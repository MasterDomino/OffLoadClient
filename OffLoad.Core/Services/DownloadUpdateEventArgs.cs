using System;

namespace OffLoad.Core.Services
{
    public class DownloadUpdateEventArgs : EventArgs
    {
        #region Instantiation

        public DownloadUpdateEventArgs(int progress) => Progress = progress;

        #endregion

        #region Properties

        public int Progress { get; }

        #endregion
    }
}