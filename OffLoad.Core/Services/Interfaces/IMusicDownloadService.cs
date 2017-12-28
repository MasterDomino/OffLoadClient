namespace OffLoad.Core.Services.Interfaces
{
    public interface IMusicDownloadService
    {
        #region Methods

        void DownloadAsync(string url, string path);

        void DownloadPlaylistAsync(string url, string path);

        #endregion
    }
}