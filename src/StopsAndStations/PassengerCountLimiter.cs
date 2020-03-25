// <copyright file="PassengerCountLimiter.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

namespace StopsAndStations
{
    using System;
    using ColossalFramework.Plugins;
    using ICities;

    /// <summary>
    /// A service that observes the stops in the city and calculates their current passenger count.
    /// Based on the provided <see cref="ModConfiguration"/>, it also limits the number of citizens
    /// waiting for transport at those stops by setting the passenger status to 'cannot use transport'.
    /// </summary>
    public sealed class PassengerCountLimiter : ThreadingExtensionBase
    {
        private const int StepMask = 0xF;
        private const int StepSize = CitizenManager.MAX_INSTANCE_COUNT / (StepMask + 1);
        private const CitizenInstance.Flags InstanceUsingTransport = CitizenInstance.Flags.OnPath | CitizenInstance.Flags.WaitingTransport;

        private readonly ushort[] passengerCount = new ushort[NetManager.MAX_NODE_COUNT];
        private readonly NetSegment[] segments;
        private readonly CitizenInstance[] instances;
        private readonly PathUnit[] pathUnits;
        private readonly NetNode[] nodes;
        private readonly TransportLine[] transportLines;

        /// <summary>
        /// Initializes a new instance of the <see cref="PassengerCountLimiter"/> class.
        /// </summary>
        public PassengerCountLimiter()
        {
            segments = NetManager.instance.m_segments.m_buffer;
            instances = CitizenManager.instance.m_instances.m_buffer;
            pathUnits = PathManager.instance.m_pathUnits.m_buffer;
            nodes = NetManager.instance.m_nodes.m_buffer;
            transportLines = TransportManager.instance.m_lines.m_buffer;
        }

        /// <summary>
        /// Gets or sets the mod Configuration to run with.
        /// </summary>
        public ModConfiguration Configuration { get; set; }

        /// <summary>
        /// A method that is called by the game after this instance is created.
        /// </summary>
        /// <param name="threading">A reference to the game's <see cref="IThreading"/> implementation.</param>
        public override void OnCreated(IThreading threading)
        {
            var mod = (StopsAndStationsMod)PluginManager.instance.FindPluginInfo(GetType().Assembly).userModInstance;
            mod.PassengerCountLimiter = this;
        }

        /// <summary>
        /// A method that is called by the game before each simulation tick.
        /// Each tick contains multiple frames.
        /// Calculates the passenger count for every transport line stop.
        /// </summary>
        public override void OnBeforeSimulationTick()
        {
            if (Configuration == null)
            {
                return;
            }

            Array.Clear(passengerCount, 0, passengerCount.Length);

            for (int i = 0; i < instances.Length; ++i)
            {
                ref var instance = ref instances[i];
                uint pathId = instance.m_path;
                if (pathId != 0 && (instance.m_flags & InstanceUsingTransport) == InstanceUsingTransport)
                {
                    var pathPosition = pathUnits[pathId].GetPosition(instance.m_pathPositionIndex >> 1);
                    ushort nodeId = segments[pathPosition.m_segment].m_startNode;
                    ++passengerCount[nodeId];
                }
            }
        }

        /// <summary>
        /// A method that is called by the game before each simulation frame.
        /// </summary>
        public override void OnBeforeSimulationFrame()
        {
            if (Configuration == null)
            {
                return;
            }

            uint step = SimulationManager.instance.m_currentFrameIndex & StepMask;
            uint startIndex = step * StepSize;
            uint endIndex = (step + 1) * StepSize;

            for (uint i = startIndex; i < endIndex; ++i)
            {
                ref var instance = ref instances[i];
                uint pathId = instance.m_path;
                if (pathId != 0
                    && instance.m_waitCounter == 0
                    && (instance.m_flags & InstanceUsingTransport) == InstanceUsingTransport)
                {
                    var pathPosition = pathUnits[pathId].GetPosition(instance.m_pathPositionIndex >> 1);
                    ushort nodeId = segments[pathPosition.m_segment].m_startNode;
                    if (passengerCount[nodeId] > GetMaximumAllowedPassengers(nodeId))
                    {
                        --passengerCount[nodeId];
                        instance.m_flags |= CitizenInstance.Flags.BoredOfWaiting;
                        instance.m_waitCounter = byte.MaxValue;
                    }
                }
            }
        }

        private int GetMaximumAllowedPassengers(ushort nodeId)
        {
            ushort transportLineId = nodes[nodeId].m_transportLine;
            if (transportLineId == 0)
            {
                return int.MaxValue;
            }

            switch (transportLines[transportLineId].Info?.m_transportType)
            {
                case TransportInfo.TransportType.EvacuationBus:
                    return Configuration.MaxWaitingPassengersEvacuationBus;

                case TransportInfo.TransportType.Bus:
                    return Configuration.MaxWaitingPassengersBus;

                case TransportInfo.TransportType.Trolleybus:
                    return Configuration.MaxWaitingPassengersTrolleybus;

                case TransportInfo.TransportType.TouristBus:
                    return Configuration.MaxWaitingPassengersTouristBus;

                case TransportInfo.TransportType.Tram:
                    return Configuration.MaxWaitingPassengersTram;

                case TransportInfo.TransportType.Metro:
                    return Configuration.MaxWaitingPassengersMetro;

                case TransportInfo.TransportType.Train:
                    return Configuration.MaxWaitingPassengersTrain;

                case TransportInfo.TransportType.Monorail:
                    return Configuration.MaxWaitingPassengersMonorail;

                case TransportInfo.TransportType.Airplane:
                    return Configuration.MaxWaitingPassengersAirplane;

                case TransportInfo.TransportType.Ship:
                    return Configuration.MaxWaitingPassengersShip;

                case TransportInfo.TransportType.CableCar:
                    return Configuration.MaxWaitingPassengersCableCar;

                case TransportInfo.TransportType.HotAirBalloon:
                    return Configuration.MaxWaitingPassengersHotAirBalloon;

                case TransportInfo.TransportType.Helicopter:
                    return Configuration.MaxWaitingPassengersHelicopter;

                default:
                    return int.MaxValue;
            }
        }
    }
}
