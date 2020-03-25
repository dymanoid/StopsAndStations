// <copyright file="ModConfiguration.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

namespace StopsAndStations
{
    using System;
    using SkyTools.Configuration;
    using SkyTools.Tools;
    using SkyTools.UI;

    /// <summary>
    /// A container class that keeps the mod configuration.
    /// </summary>
    public sealed class ModConfiguration : IConfiguration
    {
        /// <summary>The storage ID for the configuration objects.</summary>
        public const string StorageId = "StopsAndStationsConfiguration";

        /// <summary>
        /// Gets or sets the maximum waiting passengers count for a bus stop.
        /// </summary>
        [ConfigItem("Passengers", "MaximumPassengers", 0)]
        [ConfigItemSlider(10, 500, 5, ValueType = SliderValueType.Default)]
        public int MaxWaitingPassengersBus { get; set; } = 50;

        /// <summary>
        /// Gets or sets the maximum waiting passengers count for a trolleybus stop.
        /// </summary>
        [ConfigItem("Passengers", "MaximumPassengers", 1)]
        [ConfigItemSlider(10, 500, 5, ValueType = SliderValueType.Default)]
        public int MaxWaitingPassengersTrolleybus { get; set; } = 50;

        /// <summary>
        /// Gets or sets the maximum waiting passengers count for an evacuation bus stop.
        /// </summary>
        [ConfigItem("Passengers", "MaximumPassengers", 2)]
        [ConfigItemSlider(10, 500, 5, ValueType = SliderValueType.Default)]
        public int MaxWaitingPassengersEvacuationBus { get; set; } = 100;

        /// <summary>
        /// Gets or sets the maximum waiting passengers count for a tourist bus stop.
        /// </summary>
        [ConfigItem("Passengers", "MaximumPassengers", 3)]
        [ConfigItemSlider(10, 500, 5, ValueType = SliderValueType.Default)]
        public int MaxWaitingPassengersTouristBus { get; set; } = 50;

        /// <summary>
        /// Gets or sets the maximum waiting passengers count for a tram stop.
        /// </summary>
        [ConfigItem("Passengers", "MaximumPassengers", 4)]
        [ConfigItemSlider(10, 500, 5, ValueType = SliderValueType.Default)]
        public int MaxWaitingPassengersTram { get; set; } = 80;

        /// <summary>
        /// Gets or sets the maximum waiting passengers count for a metro station.
        /// </summary>
        [ConfigItem("Passengers", "MaximumPassengers", 5)]
        [ConfigItemSlider(50, 2000, 25, ValueType = SliderValueType.Default)]
        public int MaxWaitingPassengersMetro { get; set; } = 250;

        /// <summary>
        /// Gets or sets the maximum waiting passengers count for a train station.
        /// </summary>
        [ConfigItem("Passengers", "MaximumPassengers", 6)]
        [ConfigItemSlider(50, 2000, 25, ValueType = SliderValueType.Default)]
        public int MaxWaitingPassengersTrain { get; set; } = 250;

        /// <summary>
        /// Gets or sets the maximum waiting passengers count for a monorail stop.
        /// </summary>
        [ConfigItem("Passengers", "MaximumPassengers", 7)]
        [ConfigItemSlider(50, 2000, 25, ValueType = SliderValueType.Default)]
        public int MaxWaitingPassengersMonorail { get; set; } = 250;

        /// <summary>
        /// Gets or sets the maximum waiting passengers count for a ship harbor.
        /// </summary>
        [ConfigItem("Passengers", "MaximumPassengers", 8)]
        [ConfigItemSlider(50, 1000, 10, ValueType = SliderValueType.Default)]
        public int MaxWaitingPassengersShip { get; set; } = 150;

        /// <summary>
        /// Gets or sets the maximum waiting passengers count for an airline.
        /// </summary>
        [ConfigItem("Passengers", "MaximumPassengers", 9)]
        [ConfigItemSlider(50, 1000, 10, ValueType = SliderValueType.Default)]
        public int MaxWaitingPassengersAirplane { get; set; } = 250;

        /// <summary>
        /// Gets or sets the maximum waiting passengers count for a cable car stop.
        /// </summary>
        [ConfigItem("Passengers", "MaximumPassengers", 10)]
        [ConfigItemSlider(10, 500, 5, ValueType = SliderValueType.Default)]
        public int MaxWaitingPassengersCableCar { get; set; } = 40;

        /// <summary>
        /// Gets or sets the maximum waiting passengers count for a balloon stop.
        /// </summary>
        [ConfigItem("Passengers", "MaximumPassengers", 11)]
        [ConfigItemSlider(10, 500, 5, ValueType = SliderValueType.Default)]
        public int MaxWaitingPassengersHotAirBalloon { get; set; } = 40;

        /// <summary>
        /// Gets or sets the maximum waiting passengers count for a passenger helicopter stop.
        /// </summary>
        [ConfigItem("Passengers", "MaximumPassengers", 12)]
        [ConfigItemSlider(10, 500, 5, ValueType = SliderValueType.Default)]
        public int MaxWaitingPassengersHelicopter { get; set; } = 40;

        /// <summary>Checks the inner state of the object and migrates it to the latest version when necessary.</summary>
        public void MigrateWhenNecessary()
        {
        }

        /// <summary>Validates this instance and corrects possible invalid property values.</summary>
        public void Validate()
        {
            MaxWaitingPassengersBus = RoundToNearest(FastMath.Clamp(MaxWaitingPassengersBus, 10, 500), 5);
            MaxWaitingPassengersTrolleybus = RoundToNearest(FastMath.Clamp(MaxWaitingPassengersTrolleybus, 10, 500), 5);
            MaxWaitingPassengersTouristBus = RoundToNearest(FastMath.Clamp(MaxWaitingPassengersTouristBus, 10, 500), 5);
            MaxWaitingPassengersEvacuationBus = RoundToNearest(FastMath.Clamp(MaxWaitingPassengersEvacuationBus, 10, 500), 5);
            MaxWaitingPassengersTram = RoundToNearest(FastMath.Clamp(MaxWaitingPassengersTram, 10, 500), 5);
            MaxWaitingPassengersMetro = RoundToNearest(FastMath.Clamp(MaxWaitingPassengersMetro, 50, 2000), 25);
            MaxWaitingPassengersTrain = RoundToNearest(FastMath.Clamp(MaxWaitingPassengersTrain, 50, 2000), 25);
            MaxWaitingPassengersMonorail = RoundToNearest(FastMath.Clamp(MaxWaitingPassengersMonorail, 50, 2000), 25);
            MaxWaitingPassengersAirplane = RoundToNearest(FastMath.Clamp(MaxWaitingPassengersAirplane, 50, 1000), 10);
            MaxWaitingPassengersShip = RoundToNearest(FastMath.Clamp(MaxWaitingPassengersShip, 50, 1000), 10);
            MaxWaitingPassengersCableCar = RoundToNearest(FastMath.Clamp(MaxWaitingPassengersCableCar, 10, 500), 5);
            MaxWaitingPassengersHotAirBalloon = RoundToNearest(FastMath.Clamp(MaxWaitingPassengersHotAirBalloon, 10, 500), 5);
            MaxWaitingPassengersHelicopter = RoundToNearest(FastMath.Clamp(MaxWaitingPassengersHelicopter, 10, 500), 5);
        }

        private static int RoundToNearest(int value, int boundary) => (int)Math.Round((float)value / boundary) * boundary;
    }
}
