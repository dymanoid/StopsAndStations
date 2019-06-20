// <copyright file="StopsAndStationsMod.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

namespace StopsAndStations
{
    using ICities;
    using SkyTools.Tools;

    /// <summary>The main class of the Stops and Stations mod.</summary>
    public sealed class StopsAndStationsMod : IUserMod
    {
        private readonly string modVersion = "0.1"; //GitVersion.GetAssemblyVersion(typeof(StopsAndStationsMod).Assembly);
        private readonly bool isWorkshopMode = IsWorkshopMode();

        /// <summary>Gets the name of this mod.</summary>
        public string Name => "Stops and Stations";

        /// <summary>Gets the description string of this mod.</summary>
        public string Description => "Prevents the stops and stations from being unrealistically overcrowded. Version: " + modVersion;

        /// <summary>Called when this mod is enabled.</summary>
        public void OnEnabled()
        {
            if (!isWorkshopMode)
            {
                Log.Info($"The 'Stops and Stations' mod version {modVersion} cannot be started because of no Steam Workshop");
                return;
            }

            Log.Info("The 'Stops and Stations' mod has been enabled, version: " + modVersion);
        }

        /// <summary>Called when this mod is disabled.</summary>
        public void OnDisabled()
        {
            if (!isWorkshopMode)
            {
                return;
            }

            Log.Info("The 'Stops and Stations' mod has been disabled.");
        }

        // TODO: enable Workshop mode
        private static bool IsWorkshopMode() => true;
            //=> PluginManager.instance.GetPluginsInfo().Any(pi => pi.publishedFileID.AsUInt64 == WorkshopId);
    }
}