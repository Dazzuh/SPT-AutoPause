using System.Reflection;
using EFT;
using SPT.Reflection.Patching;
using AutoPause.Helpers;
using System.Linq;

namespace AutoPause
{
    public class OnGameStartedPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod() => typeof(GameWorld).GetMethod("OnGameStarted", BindingFlags.Public | BindingFlags.Instance);

        [PatchPostfix]
        private static void PostFix()
        {
            if (AutoPause.Mode.Value == "Disabled") return;
            if (AutoPause.Mode.Value == "Spotify")
            {
                var title = SpotifyHelper.GetSpotifyWindowTitle();
                bool isInvalidTitle = SpotifyHelper.invalidTitles.Contains(title) || string.IsNullOrEmpty(title);
                if (!isInvalidTitle)
                {
                    MediaKeyHelper.SendPlayPause();
                    AutoPause.isPaused = true;
                    Logger.LogInfo($"[Spotify mode] Spotify window title: {title}");
                    Logger.LogInfo("[Spotify mode] Pause key sent.");
                }
                else
                {
                    Logger.LogDebug("[Spotify mode] No Spotify window found or title is invalid.");
                }
            } 
            else if (AutoPause.Mode.Value == "Dumb")
            {
                MediaKeyHelper.SendPlayPause();
                AutoPause.isPaused = true;
                Logger.LogInfo("[Dumb mode] Play/Pause key sent.");
            }
            else
            {
                Logger.LogError($"Invalid mode: {AutoPause.Mode.Value}");
            }
        }
    }
}