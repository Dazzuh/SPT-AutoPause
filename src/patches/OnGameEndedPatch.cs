using System.Reflection;
using EFT;
using SPT.Reflection.Patching;
using AutoPause.Helpers;
using System.Linq;

namespace AutoPause
{
    public class OnGameEndedPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod() => typeof(GameWorld).GetMethod(nameof(GameWorld.UnregisterPlayer));

        [PatchPostfix]
        private static void PostFix()
        {
            if (AutoPause.Mode.Value == "Disabled" || !AutoPause.isPaused) return;
            if (AutoPause.Mode.Value == "Spotify")
            {
                var title = SpotifyHelper.GetSpotifyWindowTitle();
                bool isInvalidTitle = SpotifyHelper.invalidTitles.Contains(title) || string.IsNullOrEmpty(title);
                if (!isInvalidTitle)
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
            else if (AutoPause.Mode.Value == "Dumb")
            {
                MediaKeyHelper.SendPlayPause();
                AutoPause.isPaused = false;
                Logger.LogInfo("Play/Pause key sent.");
            }
            else
            {
                Logger.LogError($"Invalid mode: {AutoPause.Mode.Value}");
            }
        }
    }
}