using BepInEx;
using BepInEx.Logging;
using BepInEx.Configuration;
using System.Collections.Generic;

namespace AutoPause;
[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class AutoPause : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;

    public static ConfigEntry<string> Mode;
    public static ConfigEntry<bool> Resume;
    public static bool isPaused = false;
    private void Awake()
    {
        Logger = base.Logger;

        Logger.LogInfo($"Loading {MyPluginInfo.PLUGIN_GUID} - v{MyPluginInfo.PLUGIN_VERSION}");

        initConfig();

        Logger.LogInfo("Loading patches...");
        LoadPatches();
        Logger.LogInfo("Patches loaded.");

        Logger.LogInfo($"Done loading {MyPluginInfo.PLUGIN_GUID} - v{MyPluginInfo.PLUGIN_VERSION}");
    }

    private void initConfig()
    {
        var radioOptions = new[] { "Disabled", "Spotify", "Dumb" };
        var optionDescriptions = new Dictionary<string, string> {
            { "Disabled", "AutoPause is turned off." },
            { "Spotify", "Pauses only when Spotify window is detected and playing." },
            { "Dumb", "Will simply send the play/pause button at the beginning of raid. Warning: If you do not have any media playing, this will cause your last played media to resume." }
        };
        Mode = Config.Bind("General", "Mode of AutoPause", "Spotify", new ConfigDescription(
            "Select the mode for AutoPause",
            new AcceptableValueList<string>(radioOptions),
            new ConfigurationManagerAttributes {
                CustomDrawer = entry => {
                    var current = (string)entry.BoxedValue;
                    foreach (var opt in radioOptions) {
                        bool selected = current == opt;
                        var content = new UnityEngine.GUIContent(opt, optionDescriptions[opt]);
                        if (UnityEngine.GUILayout.Toggle(selected, content, UnityEngine.GUILayout.ExpandWidth(false))) {
                            if (!selected) entry.BoxedValue = opt;
                        }
                    }
                    UnityEngine.GUILayout.Label(UnityEngine.GUI.tooltip ?? "");
                }
            }
        ));
        Resume = Config.Bind("General", "Resume playback when exiting raid.", true);
    }

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
