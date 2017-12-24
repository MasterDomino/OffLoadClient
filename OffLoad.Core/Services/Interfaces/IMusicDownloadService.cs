namespace OffLoad.Core.Services.Interfaces
{
    public interface IMusicDownloadService
    {
        #region Methods

        bool Download(string url, bool specialTitleProcessing, string path);

        #endregion
    }
}