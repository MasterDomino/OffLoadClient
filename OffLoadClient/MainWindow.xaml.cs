using log4net;
using OffLoad.Core;
using OffLoad.Core.Services;
using System;
using System.Windows;
using System.Windows.Forms;

namespace OffLoadClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Instantiation

        public MainWindow()
        {
            log4net.Config.XmlConfigurator.Configure();
            Logger.InitializeLogger(LogManager.GetLogger(nameof(MainWindow)));
            InitializeComponent();
        }

        #endregion

        #region Methods

        private void Download(object sender, RoutedEventArgs e)
        {
            using (new CursorWait())
            {
                System.Windows.Forms.Application.UseWaitCursor = true;
                MusicDownloadService MDS = new MusicDownloadService();
                string url = VideoURL.Text;
                string path = string.IsNullOrWhiteSpace(Path.Text) ? "music" : Path.Text;
                if (url.Contains("www.youtube.com/playlist"))
                {
                    MDS.DownloadPlaylistAsync(url, path);
                }
                else if (url.Contains("www."))
                {
                    MDS.DownloadAsync(url, path);
                }
                else
                {
                    System.Windows.MessageBox.Show("You've input the wrong URL", "Download unsuccessfull!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
        }

        #endregion

        private void PathButton_Click(object sender, RoutedEventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    Path.Text = fbd.SelectedPath;
                }
            }
        }

        public class CursorWait : IDisposable
        {
            public CursorWait(bool appStarting = false, bool applicationCursor = false)
            {
                // Wait
                System.Windows.Forms.Cursor.Current = appStarting ? Cursors.AppStarting : Cursors.WaitCursor;
                System.Windows.Forms.Application.UseWaitCursor |= applicationCursor;
            }

            public void Dispose()
            {
                // Reset
                System.Windows.Forms.Cursor.Current = Cursors.Default;
                System.Windows.Forms.Application.UseWaitCursor = false;
            }
        }
    }
}