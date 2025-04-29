using System.Reflection;
using EFT;
using SPT.Reflection.Patching;
using AutoPause.Helpers;

namespace AutoPause
{
    public class OnGameEndedPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod() => typeof(GameWorld).GetMethod(nameof(GameWorld.UnregisterPlayer));

        [PatchPostfix]
        private static void PostFix()
        {
            if (!AutoPause.isPaused) return;
            var title = SpotifyHelper.GetSpotifyWindowTitle();
            if (!string.IsNullOrEmpty(title) && title != "No Spotify window found" && title == "Spotify Premium" || title == "Spotify Free")
            {
                MediaKeyHelper.SendPlayPause();
                AutoPause.isPaused = false;
                Logger.LogInfo($"Spotify window title: {title}");
                Logger.LogInfo("Play key sent.");
            }
            else
            {
                Logger.LogDebug("No Spotify window found or title is invalid.");
            }
        }
    }
}