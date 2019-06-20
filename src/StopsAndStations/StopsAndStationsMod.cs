// <copyright file="StopsAndStationsMod.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

namespace StopsAndStations
{
    using System;
    using System.IO;
    using ColossalFramework;
    using ColossalFramework.Globalization;
    using ColossalFramework.IO;
    using ICities;
    using SkyTools.Configuration;
    using SkyTools.Storage;
    using SkyTools.Tools;

    /// <summary>The main class of the Stops and Stations mod.</summary>
    public sealed class StopsAndStationsMod : LoadingExtensionBase, IUserMod
    {
        private const ulong WorkshopId = 0;
        private const string NoWorkshopMessage = "Stops and Stations can only run when subscribed to in Steam Workshop";

        private readonly string modVersion = GitVersion.GetAssemblyVersion(typeof(StopsAndStationsMod).Assembly);
        private readonly string modPath = GetModPath();

        /// <summary>
        /// Initializes a new instance of the <see cref="StopsAndStationsMod"/> class.
        /// </summary>
        public StopsAndStationsMod()
        {
            ConfigProvider = new ConfigurationProvider<ModConfiguration>(ModConfiguration.StorageId, Name, () => new ModConfiguration());
        }

        /// <summary>Gets the name of this mod.</summary>
        public string Name => "Stops and Stations";

        /// <summary>Gets the description string of this mod.</summary>
        public string Description => "Prevents the stops and stations from being unrealistically overcrowded. Version: " + modVersion;

        /// <summary>
        /// Gets the configuration provider service.
        /// </summary>
        internal ConfigurationProvider<ModConfiguration> ConfigProvider { get; }

        /// <summary>Called when this mod is enabled.</summary>
        public void OnEnabled()
        {
            if (string.IsNullOrEmpty(modPath))
            {
                Log.Info($"The 'Stops and Stations' mod version {modVersion} cannot be started because of no Steam Workshop");
                return;
            }

            ConfigProvider.LoadDefaultConfiguration();
            Log.Info("The 'Stops and Stations' mod has been enabled, version: " + modVersion);
        }

        /// <summary>Called when this mod is disabled.</summary>
        public void OnDisabled()
        {
            if (string.IsNullOrEmpty(modPath))
            {
                return;
            }

            ConfigProvider.SaveDefaultConfiguration();
            Log.Info("The 'Stops and Stations' mod has been disabled.");
        }


        /// <summary>
        /// Called when a game level is loaded. If applicable, activates the Real Time mod for the loaded level.
        /// </summary>
        /// <param name="mode">The <see cref="LoadMode"/> a game level is loaded in.</param>
        public override void OnLevelLoaded(LoadMode mode)
        {
            if (string.IsNullOrEmpty(modPath))
            {
                MessageBox.Show("Sorry", NoWorkshopMessage);
                return;
            }

            switch (mode)
            {
                case LoadMode.LoadGame:
                case LoadMode.NewGame:
                case LoadMode.LoadScenario:
                case LoadMode.NewGameFromScenario:
                    break;

                default:
                    return;
            }

            Log.Info($"The 'Stops and Stations' mod starts, game mode {mode}.");
            StorageBase.CurrentLevelStorage.GameSaving += GameSaving;
            StorageBase.CurrentLevelStorage.Deserialize(ConfigProvider);
        }

        /// <summary>
        /// Called when a game level is about to be unloaded. If the Real Time mod was activated for this level,
        /// deactivates the mod for this level.
        /// </summary>
        public override void OnLevelUnloading()
        {
            if (!string.IsNullOrEmpty(modPath))
            {
                StorageBase.CurrentLevelStorage.GameSaving -= GameSaving;
                ConfigProvider.LoadDefaultConfiguration();
            }
        }

        private static string GetModPath()
        {
            /*PluginManager.PluginInfo pluginInfo = PluginManager.instance.GetPluginsInfo()
                .FirstOrDefault(pi => pi.publishedFileID.AsUInt64 == WorkshopId);

            return pluginInfo?.modPath;*/

            // TODO: enable Workshop mode
            return Path.Combine(DataLocation.modsPath,  "StopsAndStations");
        }

        private void GameSaving(object sender, EventArgs e) => StorageBase.CurrentLevelStorage.Serialize(ConfigProvider);
    }
}