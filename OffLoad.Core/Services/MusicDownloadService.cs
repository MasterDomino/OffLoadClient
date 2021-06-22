using OffLoad.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using YoutubeExplode;
using YoutubeExplode.Converter;
using YoutubeExplode.Models;
using YoutubeExplode.Models.MediaStreams;

namespace OffLoad.Core.Services
{
    public class MusicDownloadService
    {
        #region Members

        public event EventHandler<DownloadUpdateEventArgs> DownloadProgressUpdate;

        private readonly List<string> _undownloaded = new List<string>();

        #endregion

        #region Properties

        public int MaxDownloadTasks { get; set; }

        #endregion

        #region Methods

        public async void DownloadAsync(string url, string path, bool isVideo)
        {
            if (YoutubeClient.TryParseVideoId(url, out string videoId))
            {
                YoutubeClient client = new YoutubeClient();
                YoutubeConverter converter = new YoutubeConverter(client);
                Video video = null;
                try
                {
                    video = await client.GetVideoAsync(videoId).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace, "Exception encoutered.", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                if (video != null)
                {
                    bool task = await DownloadItemAsync(path, video, client, converter, isVideo).ConfigureAwait(false);
                    if (task)
                    {
                        DownloadProgressUpdate?.Invoke(this, new DownloadUpdateEventArgs(10000));
                        MessageBox.Show("File download successfull!", "Download successfull!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                    else
                    {
                        DownloadProgressUpdate?.Invoke(this, new DownloadUpdateEventArgs(696969));
                        MessageBox.Show("I encountered an exception when downloading.\n" + _undownloaded[0] + "\n", "Exception encoutered.", MessageBoxButton.OK, MessageBoxImage.Error);
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
                YoutubeConverter converter = new YoutubeConverter(client);
                Playlist playlist = null;
                try
                {
                    playlist = await client.GetPlaylistAsync(playlistId).ConfigureAwait(false);
                }
                catch
                {
                }
                if (playlist != null)
                {
                    bool[] tasks = new bool[playlist.Videos.Count];
                    ParallelOptions options = new ParallelOptions { MaxDegreeOfParallelism = MaxDownloadTasks };
                    Parallel.For(0, playlist.Videos.Count, options, async v => tasks[v] = await DownloadItemAsync(path, playlist.Videos[v], client, converter, isVideo).ConfigureAwait(false));
                    if (tasks.All(s => s))
                    {
                        DownloadProgressUpdate?.Invoke(this, new DownloadUpdateEventArgs(10000));
                        MessageBox.Show("Playlist download successful!", "Download successfull!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                    else
                    {
                        DownloadProgressUpdate?.Invoke(this, new DownloadUpdateEventArgs(696969));
                        MessageBox.Show("I encountered an exception when downloading one of the playlist titles.", "Exception encoutered.", MessageBoxButton.OK, MessageBoxImage.Error);
                        ILoggingService undownloaded = new LoggingService(path + "\\Undownloaded.list");
                        undownloaded.LogUndownloaded(_undownloaded.ToArray());
                        undownloaded.Dispose();
                    }
                }
                else
                {
                    MessageBox.Show("The playlist object was null.", "Exception encoutered.", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("The url you've used is wrong!\n" + url, "Download unsuccessfull!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private static string RemoveIllegalCharacters(string input) => string.Concat(input.Split(Path.GetInvalidFileNameChars()));

        private async Task<bool> DownloadItemAsync(string path, Video video, YoutubeClient client, YoutubeConverter converter, bool isVideo)
        {
            string fullPath = path + "/" + RemoveIllegalCharacters(video.Title) + (isVideo ? ".mp4" : ".m4a");
            if (File.Exists(fullPath))
            {
                return true;
            }
            try
            {
                MediaStreamInfoSet streamInfoSet = await client.GetVideoMediaStreamInfosAsync(video.Id).ConfigureAwait(false);
                if (isVideo)
                {
                    AudioStreamInfo audioStreamInfo = streamInfoSet?.Audio.Where(a => a.Container == Container.Mp4).WithHighestBitrate();
                    VideoStreamInfo videoStreamInfo = streamInfoSet?.Video.Where(a => a.Container == Container.Mp4).WithHighestVideoQuality();
                    if (audioStreamInfo != null && videoStreamInfo != null)
                    {
                        MediaStreamInfo[] mediaStreamInfos = new MediaStreamInfo[] { audioStreamInfo, videoStreamInfo };
                        await converter.DownloadAndProcessMediaStreamsAsync(mediaStreamInfos, fullPath, "mp4");
                        return true;
                    }
                }
                else
                {
                    AudioStreamInfo streamInfo = streamInfoSet?.Audio.Where(a => a.Container == Container.Mp4).WithHighestBitrate();
                    if (streamInfo != null)
                    {
                        await client.DownloadMediaStreamAsync(streamInfo, fullPath).ConfigureAwait(false);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                _undownloaded.Add(video.Title + " | " + video.GetShortUrl() + " | Reason: " + ex.Message);
                return false;
            }
            return false;
        }

        #endregion
    }
}