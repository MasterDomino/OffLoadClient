using OffLoad.Core.Services.Interfaces;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using YoutubeExplode;
using YoutubeExplode.Models;
using YoutubeExplode.Models.MediaStreams;

namespace OffLoad.Core.Services
{
    public class MusicDownloadService : IMusicDownloadService
    {
        #region Properties

        public int MaxDownloadTasks { get; set; }

        #endregion

        #region Methods

        public async void DownloadAsync(string url, string path)
        {
            if (YoutubeClient.TryParseVideoId(url, out string videoId))
            {
                YoutubeClient client = new YoutubeClient();
                Video video = await client.GetVideoAsync(videoId).ConfigureAwait(false);
                if (video != null)
                {
                    bool task = await DownloadItemAsync(path, video, client).ConfigureAwait(false);
                    if (task)
                    {
                        MessageBox.Show("File download successfull!", "Download successfull!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                    else
                    {
                        MessageBox.Show("I encountered an exception when downloading.", "Exception encoutered.", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("The video object was null.", "Exception encoutered.", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else if (YoutubeClient.TryParsePlaylistId(url, out string playlistId))
            {
                YoutubeClient client = new YoutubeClient();
                Playlist playlist = await client.GetPlaylistAsync(playlistId).ConfigureAwait(false);
                if (playlist != null)
                {
                    bool[] tasks = new bool[playlist.Videos.Count];
                    ParallelOptions options = new ParallelOptions { MaxDegreeOfParallelism = MaxDownloadTasks };
                    Parallel.ForEach(playlist.Videos, options, v => tasks.Append(DownloadItemAsync(path, v, client).Result));
                    if (tasks.All(s => true))
                    {
                        MessageBox.Show("Playlist download successful!", "Download successfull!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                    else
                    {
                        MessageBox.Show("I encountered an exception when downloading one of the playlist titles.", "Exception encoutered.", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("The playlist object was null.", "Exception encoutered.", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show($"The url you've used is wrong!\n{url}", "Download unsuccessfull!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task<bool> DownloadItemAsync(string path, Video video, YoutubeClient client)
        {
            string fullPath = $"{path}\\{video.Title}.m4a";
            if (File.Exists(fullPath))
            {
                return true;
            }
            MediaStreamInfoSet streamInfoSet = await client.GetVideoMediaStreamInfosAsync(video.Id).ConfigureAwait(false);
            MediaStreamInfo streamInfo = streamInfoSet?.Audio.Where(a => a.Container == Container.M4A).WithHighestBitrate();
            if (streamInfo != null)
            {
                await client.DownloadMediaStreamAsync(streamInfo, fullPath).ConfigureAwait(false);
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion
    }
}