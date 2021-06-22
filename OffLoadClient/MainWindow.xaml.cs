using OffLoad.Core.Services;
using System;
using System.Configuration;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Threading;

namespace OffLoadClient
{
    public partial class MainWindow : Window
    {
        #region Members

        private readonly MusicDownloadService _mds;

        #endregion

        #region Instantiation

        public MainWindow()
        {
            _mds = new MusicDownloadService()
            {
                MaxDownloadTasks = Convert.ToInt32(ConfigurationManager.AppSettings["MaxDownloadTasks"])
            };
            _mds.DownloadProgressUpdate += OnDownloadProgressUpdate;
            InitializeComponent();
            DownloadProgress.Maximum = 10000;
            DownloadProgress.Value = 0;
        }

        #endregion

        #region Methods

        private void Download(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(URLBox.Text) && IsVideo.IsChecked.HasValue)
            {
                string path = string.IsNullOrWhiteSpace(Path.Text) ? "music" : Path.Text;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                _mds.DownloadAsync(URLBox.Text, path, IsVideo.IsChecked.Value);
            }
        }

        private void OnDownloadProgressUpdate(object sender, DownloadUpdateEventArgs e)
        {
            if (e.Progress <= 10000)
            {
                Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => DownloadProgress.Value = e.Progress));
            }
            else if (e.Progress == 696969)
            {
                Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => {
                    DownloadProgress.Foreground = Brushes.Red;
                    DownloadProgress.Value = 10000;
                }));
            }
        }

        private void PathButton_Click(object sender, RoutedEventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    Path.Text = fbd.SelectedPath.Replace('\\','/');
                }
            }
        }

        #endregion
    }
}