using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;

namespace OffLoad.Core
{
    public static class Extensions
    {
        #region Methods

        public static List<int> AllIndexesOf(this string str, string value)
        {
            List<int> indexes = new List<int>();
            if (string.IsNullOrEmpty(value))
            {
                return indexes;
            }
            for (int index = 0; ; index += value.Length)
            {
                index = str.IndexOf(value, index, StringComparison.CurrentCulture);
                if (index == -1)
                {
                    return indexes;
                }
                indexes.Add(index);
            }
        }

        public static string RemoveSpecialCharacters(this string str) => Regex.Replace(str, @"\u3010.*?\u3011|\u005B.*?\u005D|\u0028.*?\u0029|[^0-9a-zA-Z\s_\-+.,!@#$%^&*():?=;\\/|<>\u0022']", "", RegexOptions.Compiled);

        public static string GetWebsiteTitleSpecial(string url)
        {
            WebClient wc = new WebClient();
            wc.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/61.0.3163.100 Safari/537.36");
            wc.Encoding = System.Text.Encoding.UTF8;
            tryAgain:
            string response = wc.DownloadString(url);
            string match = Regex.Match(response, @"\<title\b[^>]*\>\s*(?<Title>[\s\S]*?)\</title\>", RegexOptions.IgnoreCase).Groups["Title"].Value.RemoveSpecialCharacters().Replace(" - YouTube", "").Replace("| Free Listening on SoundCloud", "").Replace(" HD", "");
            if (match.Equals("YouTube"))
            {
                Thread.Sleep(100);
                goto tryAgain;
            }
            string[] split = match.Split(' ');
            if (split.Length <= 2 && !match.Contains("-"))
            {
                match = $"{split[0]} - {split[1]}";
            }
            return match.TrimEnd(' ');
        }

        public static string GetWebsiteTitle(string url)
        {
            WebClient wc = new WebClient();
            wc.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/61.0.3163.100 Safari/537.36");
            wc.Encoding = System.Text.Encoding.UTF8;
            tryAgain:
            string response = wc.DownloadString(url);
            string match = Regex.Match(response, @"\<title\b[^>]*\>\s*(?<Title>[\s\S]*?)\</title\>", RegexOptions.IgnoreCase).Groups["Title"].Value.Replace(" - YouTube", "").Replace("| Free Listening on SoundCloud", "").Replace(" HD", "");
            if (match.Equals("YouTube"))
            {
                Thread.Sleep(100);
                goto tryAgain;
            }
            return match.TrimEnd(' ');
        }

        #endregion
    }
}