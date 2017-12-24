using log4net;
using OffLoad.Core;
using OffLoad.Core.Services;
using System;
using System.IO;
using System.Net;
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
            bool downloaded = false;
            using (new CursorWait())
            {
                System.Windows.Forms.Application.UseWaitCursor = true;
                MusicDownloadService MDS = new MusicDownloadService();
                string url = VideoURL.Text;
                bool experimentalTitle = ExperimentalCheckBox.IsChecked ?? false;
                string path = string.IsNullOrWhiteSpace(Path.Text) ? "music" : Path.Text;
                if (url.Contains("www.youtube.com/playlist"))
                {
                    string response = new WebClient().DownloadString(new Uri(url));
                    foreach (int i in response.AllIndexesOf("data-video-id=\""))
                    {
                        downloaded = MDS.Download($"https://www.youtube.com/watch?v={response.Substring(i + 15, 11)}", experimentalTitle, path);
                    }
                }
                else if (url.Contains("www."))
                {
                    downloaded = MDS.Download(url, experimentalTitle, path);
                }
                else
                {
                    System.Windows.MessageBox.Show("You've input the wrong URL", "Download successfull!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }

            if (downloaded)
            {
                System.Windows.MessageBox.Show("The file was downloaded successfully!", "Download successfull!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                System.Windows.MessageBox.Show("I encountered an exception when downloading.", "Exception encoutered.", MessageBoxButton.OK, MessageBoxImage.Error);
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