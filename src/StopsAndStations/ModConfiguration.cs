// <copyright file="ModConfiguration.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

namespace StopsAndStations
{
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
        [ConfigItemSlider(10, 500, ValueType = SliderValueType.Default)]
        public int MaxWaitingPassengersBus { get; set; } = 50;

        /// <summary>
        /// Gets or sets the maximum waiting passengers count for an evacuation bus stop.
        /// </summary>
        [ConfigItem("Passengers", "MaximumPassengers", 1)]
        [ConfigItemSlider(10, 500, ValueType = SliderValueType.Default)]
        public int MaxWaitingPassengersEvacuationBus { get; set; } = 100;

        /// <summary>
        /// Gets or sets the maximum waiting passengers count for a tourist bus stop.
        /// </summary>
        [ConfigItem("Passengers", "MaximumPassengers", 2)]
        [ConfigItemSlider(10, 500, ValueType = SliderValueType.Default)]
        public int MaxWaitingPassengersTouristBus { get; set; } = 50;

        /// <summary>
        /// Gets or sets the maximum waiting passengers count for a tram stop.
        /// </summary>
        [ConfigItem("Passengers", "MaximumPassengers", 3)]
        [ConfigItemSlider(10, 500, ValueType = SliderValueType.Default)]
        public int MaxWaitingPassengersTram { get; set; } = 80;

        /// <summary>
        /// Gets or sets the maximum waiting passengers count for a metro station.
        /// </summary>
        [ConfigItem("Passengers", "MaximumPassengers", 4)]
        [ConfigItemSlider(10, 500, ValueType = SliderValueType.Default)]
        public int MaxWaitingPassengersMetro { get; set; } = 250;

        /// <summary>
        /// Gets or sets the maximum waiting passengers count for a train station.
        /// </summary>
        [ConfigItem("Passengers", "MaximumPassengers", 5)]
        [ConfigItemSlider(10, 500, ValueType = SliderValueType.Default)]
        public int MaxWaitingPassengersTrain { get; set; } = 250;

        /// <summary>
        /// Gets or sets the maximum waiting passengers count for a monorail stop.
        /// </summary>
        [ConfigItem("Passengers", "MaximumPassengers", 6)]
        [ConfigItemSlider(10, 500, ValueType = SliderValueType.Default)]
        public int MaxWaitingPassengersMonorail { get; set; } = 250;

        /// <summary>
        /// Gets or sets the maximum waiting passengers count for a ship harbor.
        /// </summary>
        [ConfigItem("Passengers", "MaximumPassengers", 7)]
        [ConfigItemSlider(10, 500, ValueType = SliderValueType.Default)]
        public int MaxWaitingPassengersShip { get; set; } = 150;

        /// <summary>
        /// Gets or sets the maximum waiting passengers count for an airline.
        /// </summary>
        [ConfigItem("Passengers", "MaximumPassengers", 8)]
        [ConfigItemSlider(10, 500, ValueType = SliderValueType.Default)]
        public int MaxWaitingPassengersAirplane { get; set; } = 250;

        /// <summary>
        /// Gets or sets the maximum waiting passengers count for a cable car stop.
        /// </summary>
        [ConfigItem("Passengers", "MaximumPassengers", 9)]
        [ConfigItemSlider(10, 500, ValueType = SliderValueType.Default)]
        public int MaxWaitingPassengersCableCar { get; set; } = 40;

        /// <summary>
        /// Gets or sets the maximum waiting passengers count for a balloon stop.
        /// </summary>
        [ConfigItem("Passengers", "MaximumPassengers", 10)]
        [ConfigItemSlider(10, 500, ValueType = SliderValueType.Default)]
        public int MaxWaitingPassengersHotAirBalloon { get; set; } = 40;

        /// <summary>Checks the inner state of the object and migrates it to the latest version when necessary.</summary>
        public void MigrateWhenNecessary()
        {
        }

        /// <summary>Validates this instance and corrects possible invalid property values.</summary>
        public void Validate()
        {
            MaxWaitingPassengersBus = FastMath.Clamp(MaxWaitingPassengersBus, 10, 500);
            MaxWaitingPassengersTouristBus = FastMath.Clamp(MaxWaitingPassengersTouristBus, 10, 500);
            MaxWaitingPassengersEvacuationBus = FastMath.Clamp(MaxWaitingPassengersEvacuationBus, 10, 500);
            MaxWaitingPassengersTram = FastMath.Clamp(MaxWaitingPassengersTram, 10, 500);
            MaxWaitingPassengersMetro = FastMath.Clamp(MaxWaitingPassengersMetro, 10, 500);
            MaxWaitingPassengersTrain = FastMath.Clamp(MaxWaitingPassengersTrain, 10, 500);
            MaxWaitingPassengersMonorail = FastMath.Clamp(MaxWaitingPassengersMonorail, 10, 500);
            MaxWaitingPassengersAirplane = FastMath.Clamp(MaxWaitingPassengersAirplane, 10, 500);
            MaxWaitingPassengersShip = FastMath.Clamp(MaxWaitingPassengersShip, 10, 500);
            MaxWaitingPassengersCableCar = FastMath.Clamp(MaxWaitingPassengersCableCar, 10, 500);
            MaxWaitingPassengersHotAirBalloon = FastMath.Clamp(MaxWaitingPassengersHotAirBalloon, 10, 500);
        }
    }
}
