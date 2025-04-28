using BepInEx;
using BepInEx.Logging;

namespace AutoPause;
[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;

    private void Awake()
    {
        Logger = base.Logger;
    
        Logger.LogInfo($"Loading {MyPluginInfo.PLUGIN_GUID} - v{MyPluginInfo.PLUGIN_VERSION}");

        // Execute RaidLoaded
        Logger.LogInfo("Loading patches...");
        LoadPatches();
        Logger.LogInfo("Patches loaded.");

        Logger.LogInfo($"Done loading {MyPluginInfo.PLUGIN_GUID} - v{MyPluginInfo.PLUGIN_VERSION}");
    }

    private void LoadPatches()
    {
        // Execute RaidLoaded
        new OnGameStartedClass().Enable();
    }

    private void OnDestroy()
    {
        Logger.LogInfo($"Unloading {MyPluginInfo.PLUGIN_GUID} - v{MyPluginInfo.PLUGIN_VERSION}");
    }
}
