using log4net;
using OffLoad.Core;
using OffLoad.Core.Services;
using OffLoad.Core.Services.Interfaces;
using System.IO;
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
            IMusicDownloadService MDS = new MusicDownloadService();
            string url = URLBox.Text;
            string path = string.IsNullOrWhiteSpace(Path.Text) ? "music" : Path.Text;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (url.Contains("www.youtube.com/playlist"))
            {
                MDS.DownloadPlaylistAsync(url, path);
            }
            else
            {
                MDS.DownloadAsync(url, path);
            }
        }

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

        #endregion
    }
}