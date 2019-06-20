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
    using SkyTools.Localization;
    using SkyTools.Storage;
    using SkyTools.Tools;
    using SkyTools.UI;

    /// <summary>The main class of the Stops and Stations mod.</summary>
    public sealed class StopsAndStationsMod : LoadingExtensionBase, IUserMod
    {
        private const ulong WorkshopId = 0;
        private const string NoWorkshopMessage = "Stops and Stations can only run when subscribed to in Steam Workshop";

        private readonly string modVersion = GitVersion.GetAssemblyVersion(typeof(StopsAndStationsMod).Assembly);
        private readonly string modPath = GetModPath();
        private LocalizationProvider localizationProvider;
        private ConfigUI configUI;

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

            localizationProvider = new LocalizationProvider(Name, modPath);
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
            CloseConfigUI();
            localizationProvider = null;
            Log.Info("The 'Stops and Stations' mod has been disabled.");
        }

        /// <summary>Called when this mod's settings page needs to be created.</summary>
        /// <param name="helper">
        /// An <see cref="UIHelperBase"/> reference that can be used to construct the mod's settings page.
        /// </param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Must be instance method due to C:S API")]
        public void OnSettingsUI(UIHelperBase helper)
        {
            if (string.IsNullOrEmpty(modPath))
            {
                helper?.AddGroup(NoWorkshopMessage);
                return;
            }

            if (helper == null || ConfigProvider == null)
            {
                return;
            }

            if (ConfigProvider.Configuration == null)
            {
                Log.Warning("The 'Stops and Stations' mod wants to display the configuration page, but the configuration is unexpectedly missing.");
                ConfigProvider.LoadDefaultConfiguration();
            }

            IViewItemFactory itemFactory = new CitiesViewItemFactory(helper);
            CloseConfigUI();
            configUI = ConfigUI.Create(ConfigProvider, itemFactory);
            ApplyLanguage();
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

        private void ApplyLanguage()
        {
            if (!SingletonLite<LocaleManager>.exists)
            {
                return;
            }

            localizationProvider.LoadTranslation(LocaleManager.instance.language);
            configUI?.Translate(localizationProvider);
        }

        private void CloseConfigUI()
        {
            if (configUI != null)
            {
                configUI.Close();
                configUI = null;
            }
        }
    }
}