namespace OffLoad.Core.Services.Interfaces
{
    public interface ILoggingService
    {
        #region Methods

        void Dispose();

        void LogUndownloaded(string[] undownloads);

        #endregion
    }
}