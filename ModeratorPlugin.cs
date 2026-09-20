using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Collections.Generic;

namespace Landoria.Moderator
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    public sealed class ModeratorPlugin : BaseUnityPlugin
    {
        private static readonly HashSet<string> CommandsRequiringModerator =
            new HashSet<string>
            {
                "exploremap", "goto", "itemset", "playerlist", "summon",
                "resetmap", "spawn", "event", "stopevent", "env", "resetenv",
                "nextday"
            };

        internal static ManualLogSource ModLogger { get; private set; }
        private const string PluginGuid = "Landoria.Moderator";
        private const string PluginName = "Landoria.Moderator";
        private const string PluginVersion = "1.0.11";


        private Harmony _harmony;

        private void Awake()
        {
            ModLogger = Logger;
            Logger.LogInfo($"AssemblyVersion: {GetType().Assembly.GetName().Version}.");
            _harmony = new Harmony(PluginGuid);
            _harmony.PatchAll();
            RegisterCommands();
            ModLogger.LogInfo($"{PluginName} {PluginVersion} is loaded.");
        }

        private void OnDestroy()
        {
            ModeratorMapSharing.Disable();
            ModLogger?.LogInfo($"{PluginName} {PluginVersion} is unloaded.");
            _harmony?.UnpatchSelf();
            _harmony = null;
            ModLogger = null;
        }

        private void Update()
        {
            PlayerPositionRpc.Update();
            ModeratorMapSharing.Update();
        }

        internal static void RegisterCommands()
        {
            ModeratorModeCommand.Register();
            ExploreMapCommand.Register();
            GotoCommand.Register();
            ItemSetCommand.Register();
            PlayerListCommand.Register();
            SummonCommand.Register();
            ResetMapCommand.Register();
            SpawnCommand.Register();
            EventCommands.Register();
            WeatherCommands.Register();
            NextDayCommand.Register();
        }

        internal static bool RequiresEnabledModerator(string command)
        {
            return CommandsRequiringModerator.Contains(command);
        }
    }
}
