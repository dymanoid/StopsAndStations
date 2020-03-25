// <copyright file="StopsAndStationsMod.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

namespace StopsAndStations
{
    using System;
    using System.Linq;
    using ColossalFramework;
    using ColossalFramework.Globalization;
    using ColossalFramework.Plugins;
    using ICities;
    using SkyTools.Configuration;
    using SkyTools.Localization;
    using SkyTools.Storage;
    using SkyTools.Tools;
    using SkyTools.UI;

    /// <summary>The main class of the Stops and Stations mod.</summary>
    public sealed class StopsAndStationsMod : LoadingExtensionBase, IUserMod
    {
        private const ulong WorkshopId = 1776052533ul;
        private const string NoWorkshopMessage = "Stops and Stations can only run when subscribed to in Steam Workshop";

        private readonly string modVersion = GitVersion.GetAssemblyVersion(typeof(StopsAndStationsMod).Assembly);
        private readonly string modPath = GetModPath();

        private readonly ConfigurationProvider<ModConfiguration> configProvider;

        private LocalizationProvider localizationProvider;
        private ConfigUI configUI;

        /// <summary>
        /// Initializes a new instance of the <see cref="StopsAndStationsMod"/> class.
        /// </summary>
        public StopsAndStationsMod()
        {
            configProvider = new ConfigurationProvider<ModConfiguration>(ModConfiguration.StorageId, Name, () => new ModConfiguration());
        }

        /// <summary>Gets the name of this mod.</summary>
        public string Name => "Stops and Stations";

        /// <summary>Gets the description string of this mod.</summary>
        public string Description => "Prevents the stops and stations from being unrealistically overcrowded. Version: " + modVersion;

        /// <summary>
        /// Gets or sets the object where the mod logic is implemented.
        /// </summary>
        internal PassengerCountLimiter PassengerCountLimiter { get; set; }

        /// <summary>Called when this mod is enabled.</summary>
        public void OnEnabled()
        {
            if (string.IsNullOrEmpty(modPath))
            {
                Log.Info($"The 'Stops and Stations' mod version {modVersion} cannot be started because of no Steam Workshop");
                return;
            }

            localizationProvider = new LocalizationProvider(Name, modPath);
            configProvider.LoadDefaultConfiguration();
            Log.Info("The 'Stops and Stations' mod has been enabled, version: " + modVersion);
        }

        /// <summary>Called when this mod is disabled.</summary>
        public void OnDisabled()
        {
            if (string.IsNullOrEmpty(modPath))
            {
                return;
            }

            configProvider.SaveDefaultConfiguration();
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

            if (helper == null || configProvider == null)
            {
                return;
            }

            if (configProvider.Configuration == null)
            {
                Log.Warning("The 'Stops and Stations' mod wants to display the configuration page, but the configuration is unexpectedly missing.");
                configProvider.LoadDefaultConfiguration();
            }

            IViewItemFactory itemFactory = new CitiesViewItemFactory(helper);
            CloseConfigUI();
            configUI = ConfigUI.Create(configProvider, itemFactory);
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
            StorageBase.CurrentLevelStorage.Deserialize(configProvider);
            if (PassengerCountLimiter != null)
            {
                PassengerCountLimiter.Configuration = configProvider.Configuration;
            }
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
                configProvider.LoadDefaultConfiguration();
            }
        }

        private static string GetModPath()
        {
            var pluginInfo = PluginManager.instance.GetPluginsInfo()
                .FirstOrDefault(pi => pi.publishedFileID.AsUInt64 == WorkshopId);

            return pluginInfo?.modPath;
        }

        private void GameSaving(object sender, EventArgs e) => StorageBase.CurrentLevelStorage.Serialize(configProvider);

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
