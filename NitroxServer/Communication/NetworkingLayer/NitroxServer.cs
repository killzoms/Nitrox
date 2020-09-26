using System;
using System.Collections.Generic;
using NitroxModel.DataStructures;
using NitroxModel.Logger;
using NitroxModel.Packets;
using NitroxModel.Server;
using NitroxServer.Communication.Packets;
using NitroxServer.GameLogic;
using NitroxServer.GameLogic.Entities;

namespace NitroxServer.Communication.NetworkingLayer
{
    public abstract class NitroxServer
    {
        protected bool isStopped = true;
        protected readonly int portNumber, maxConnections;

        protected readonly PacketHandler packetHandler;
        protected readonly EntitySimulation entitySimulation;
        protected readonly Dictionary<long, INitroxConnection> connectionsByRemoteIdentifier = new Dictionary<long, INitroxConnection>();
        protected readonly PlayerManager playerManager;

        protected NitroxServer(PacketHandler packetHandler, PlayerManager playerManager, EntitySimulation entitySimulation, ServerConfig serverConfig)
        {
            this.packetHandler = packetHandler;
            this.playerManager = playerManager;
            this.entitySimulation = entitySimulation;

            portNumber = serverConfig.ServerPort;
            maxConnections = serverConfig.MaxConnections;
        }

        public abstract bool Start();

        public abstract void Stop();

        protected void ClientDisconnected(INitroxConnection connection)
        {
            Player player = playerManager.GetPlayer(connection);

            if (player != null)
            {
                playerManager.PlayerDisconnected(connection);

                Disconnect disconnect = new Disconnect(player.Id);
                playerManager.SendPacketToAllPlayers(disconnect);

                List<SimulatedEntity> ownershipChanges = entitySimulation.CalculateSimulationChangesFromPlayerDisconnect(player);

                if (ownershipChanges.Count > 0)
                {
                    playerManager.SendPacketToAllPlayers(new SimulationOwnershipChange(ownershipChanges));
                }
            }
        }

        protected void ProcessIncomingData(INitroxConnection connection, Packet packet)
        {
            try
            {
                packetHandler.Process(packet, connection);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Exception while processing packet: {packet}.");
            }
        }
    }
}
