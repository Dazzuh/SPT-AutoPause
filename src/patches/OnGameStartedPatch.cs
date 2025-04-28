using System.Reflection;
using EFT;
using SPT.Reflection.Patching;

namespace AutoPause
{
    public class OnGameStartedClass : ModulePatch
    {
        protected override MethodBase GetTargetMethod() => typeof(GameWorld).GetMethod("OnGameStarted", BindingFlags.Public | BindingFlags.Instance);

        [PatchPostfix]
        private static void PostFix()
        {
            Logger.LogInfo("Game has started.");
        }
    }
}