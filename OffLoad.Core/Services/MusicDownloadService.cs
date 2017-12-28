using OffLoad.Core.Services.Interfaces;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using YoutubeExplode;
using YoutubeExplode.Models;
using YoutubeExplode.Models.MediaStreams;

namespace OffLoad.Core.Services
{
    public class MusicDownloadService : IMusicDownloadService
    {
        #region Methods

        public async void DownloadAsync(string url, string path)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                if (YoutubeClient.TryParseVideoId(url, out string videoId))
                {
                    YoutubeClient client = new YoutubeClient();
                    Video video = await client.GetVideoAsync(videoId).ConfigureAwait(false);

                    if (File.Exists($"{path}\\{video.Title}.m4a") || video == null)
                    {
                        return;
                    }

                    MediaStreamInfoSet streamInfoSet = await client.GetVideoMediaStreamInfosAsync(videoId).ConfigureAwait(false);
                    MediaStreamInfo streamInfo = streamInfoSet?.Audio.Where(a => a.Container == Container.M4A).WithHighestBitrate();
                    if (streamInfo != null)
                    {
                        await client.DownloadMediaStreamAsync(streamInfo, $"{path}\\{video.Title}.m4a").ConfigureAwait(false);
                    }
                    else
                    {
                        MessageBox.Show("I encountered an exception when downloading.", "Exception encoutered.", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    MessageBox.Show("The file was downloaded successfully!", "Download successfull!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                else
                {
                    Logger.Warn($"[Wrong VideoId]VideoId: {url}");
                    MessageBox.Show("The url you've used is wrong!", "Download unsuccessfull!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                MessageBox.Show($"I encountered an exception when downloading. {ex}", "Exception encoutered.", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async void DownloadPlaylistAsync(string url, string path)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                if (YoutubeClient.TryParsePlaylistId(url, out string playlistId))
                {
                    YoutubeClient client = new YoutubeClient();
                    Playlist playlist = await client.GetPlaylistAsync(playlistId).ConfigureAwait(false);

                    foreach (Video video in playlist.Videos)
                    {
                        if (File.Exists($"{path}\\{video.Title}.m4a") || playlist == null)
                        {
                            return;
                        }

                        MediaStreamInfoSet streamInfoSet = await client.GetVideoMediaStreamInfosAsync(video.Id).ConfigureAwait(false);
                        MediaStreamInfo streamInfo = streamInfoSet?.Audio.Where(a => a.Container == Container.M4A).WithHighestBitrate();
                        if (streamInfo != null)
                        {
                            await client.DownloadMediaStreamAsync(streamInfo, $"{path}\\{video.Title}.m4a").ConfigureAwait(false);
                            MessageBox.Show("The file was downloaded successfully!", "Download successfull!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        }
                        else
                        {
                            MessageBox.Show("I encountered an exception when downloading.", "Exception encoutered.", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                else
                {
                    Logger.Warn($"[Wrong VideoId]VideoId: {url}");
                    MessageBox.Show("The url you've used is wrong!", "Download unsuccessfull!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                MessageBox.Show($"I encountered an exception when downloading. {ex}", "Exception encoutered.", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion
    }
}