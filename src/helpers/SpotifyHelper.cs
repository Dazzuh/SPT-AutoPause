using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Collections.Generic;
using BepInEx.Logging;

namespace AutoPause.Helpers
{
    public static class SpotifyHelper
    {
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        public static readonly string[] invalidTitles = { "No Spotify window found", "Spotify Premium", "Spotify Free" };

        public static List<string> GetSpotifyWindowTitles()
        {
            try
            {
                var titles = new List<string>();
                var processes = Process.GetProcessesByName("Spotify");
                foreach (var process in processes)
                {
                    if (process.MainWindowHandle != IntPtr.Zero)
                    {
                        int length = GetWindowTextLength(process.MainWindowHandle);
                        if (length > 0)
                        {
                            var sb = new StringBuilder(length + 1);
                            GetWindowText(process.MainWindowHandle, sb, sb.Capacity);
                            string windowTitle = sb.ToString();
                            if (!string.IsNullOrEmpty(windowTitle))
                            {
                                titles.Add(windowTitle);
                            }
                        }
                    }
                }
                return titles;
            }
            catch (Exception ex)
            {
                ManualLogSource logger = Logger.CreateLogSource("SpotifyHelper");
                logger.LogError(ex);
                return new List<string>();
            }
        }

        public static string GetSpotifyWindowTitle()
        {
            var titles = GetSpotifyWindowTitles();
            return titles.Count > 0 ? titles[0] : "No Spotify window found";
        }
    }
}
