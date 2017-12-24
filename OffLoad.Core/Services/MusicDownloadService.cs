using OffLoad.Core.Services.Interfaces;
using System;
using System.Diagnostics;
using System.IO;

namespace OffLoad.Core.Services
{
    public class MusicDownloadService : IMusicDownloadService
    {
        #region Methods

        public bool Download(string url, bool specialTitleProcessing, string path)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string fileName = Extensions.GetWebsiteTitle(url);
                if (specialTitleProcessing)
                {
                    fileName = Extensions.GetWebsiteTitleSpecial(url);
                }
                if (File.Exists($"{path}\\{fileName}.m4a"))
                {
                    return true;
                }

                Logger.Debug("Stream download started.", "MDM");
                Stopwatch watch = new Stopwatch();
                watch.Start();

                // download and wait until process finishes
                YoutubeDL(url, path);

                watch.Stop();
                Logger.Debug($"Stream download finished, elapsed time: {watch.ElapsedMilliseconds}ms", "MDM");
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return false;
            }
        }

        private void YoutubeDL(string url, string path)
        {
            ProcessStartInfo youtubeDL = new ProcessStartInfo
            {
                FileName = "youtube-dl",
                Arguments = $"-f \"bestaudio[ext=m4a]\" -o \"{path}\\%(title)s.%(ext)s\" {url} -q --no-warnings",
                UseShellExecute = false,
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true
            };
            Process.Start(youtubeDL).WaitForExit();
        }

        #endregion
    }
}