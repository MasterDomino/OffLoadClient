using OffLoad.Core.Services;
using OffLoad.Core.Services.Interfaces;
using System;
using System.Configuration;
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
        #region Members

        private readonly IMusicDownloadService _mds;

        #endregion

        #region Instantiation

        public MainWindow()
        {
            _mds = new MusicDownloadService()
            {
                MaxDownloadTasks = Convert.ToInt32(ConfigurationManager.AppSettings["MaxDownloadTasks"])
            };
            InitializeComponent();
        }

        #endregion

        #region Methods

        private void Download(object sender, RoutedEventArgs e)
        {
            string url = URLBox.Text;
            if (!string.IsNullOrWhiteSpace(url))
            {
                string path = string.IsNullOrWhiteSpace(Path.Text) ? "music" : Path.Text;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                _mds.DownloadAsync(url, path);
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