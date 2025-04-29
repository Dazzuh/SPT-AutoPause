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

            var title = SpotifyHelper.GetSpotifyWindowTitle();
            bool isInvalidTitle = SpotifyHelper.invalidTitles.Contains(title) || string.IsNullOrEmpty(title);
            if (AutoPause.Mode.Value == "Spotify" && isInvalidTitle && !string.IsNullOrEmpty(title))
            {
                MediaKeyHelper.SendPlayPause();
                AutoPause.isPaused = false;
                Logger.LogInfo("[Spotify mode] Previously paused by us, play key sent.");
            }
            else if (AutoPause.Mode.Value == "Dumb")
            {
                MediaKeyHelper.SendPlayPause();
                AutoPause.isPaused = false;
                Logger.LogInfo("[Dumb mode] Play/Pause key sent.");
            }
            else
            {
                Logger.LogError($"Invalid mode: {AutoPause.Mode.Value}");
            }
        }
    }
}