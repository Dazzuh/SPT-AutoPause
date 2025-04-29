using BepInEx;
using BepInEx.Logging;
using BepInEx.Configuration;

namespace AutoPause;
[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class AutoPause : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;

    private static ConfigEntry<bool> Test;
    public static bool isPaused = false;
    private void Awake()
    {
        Logger = base.Logger;

        Logger.LogInfo($"Loading {MyPluginInfo.PLUGIN_GUID} - v{MyPluginInfo.PLUGIN_VERSION}");

        Logger.LogInfo("Loading patches...");
        LoadPatches();
        Logger.LogInfo("Patches loaded.");

        // initConfig();

        Logger.LogInfo($"Done loading {MyPluginInfo.PLUGIN_GUID} - v{MyPluginInfo.PLUGIN_VERSION}");
    }

    // private void initConfig()
    // {
    // }

    private void LoadPatches()
    {
        new OnGameStartedPatch().Enable();
        new OnGameEndedPatch().Enable();
    }

    private void OnDestroy()
    {
        Logger.LogInfo($"Unloading {MyPluginInfo.PLUGIN_GUID} - v{MyPluginInfo.PLUGIN_VERSION}");
    }
}
