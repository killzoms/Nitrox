using System;
using System.Collections.Generic;
using NitroxModel.DataStructures;
using NitroxModel.DataStructures.GameLogic;
using NitroxModel.DataStructures.Util;
using NitroxModel.MultiplayerSession;
using NitroxModel.Packets;
using NitroxModel.Packets.Processors.Abstract;
using NitroxServer.Communication.NetworkingLayer;

namespace NitroxServer
{
    public sealed class Player : IProcessorContext, IEquatable<Player>, IEqualityComparer<Player>
    {
        private readonly ThreadSafeCollection<EquippedItemData> equippedItems;
        private readonly ThreadSafeCollection<EquippedItemData> modules;
        private readonly ThreadSafeCollection<AbsoluteEntityCell> visibleCells;

        public INitroxConnection Connection { get; set; }
        public PlayerContext PlayerContext { get; set; }
        public ushort Id { get; }
        public string Name { get; private set; }
        public bool IsPermaDeath { get; set; }
        public NitroxVector3 Position { get; set; }
        public NitroxId GameObjectId { get; }
        public Optional<NitroxId> SubRootId { get; set; }
        public Perms Permissions { get; set; }
        public PlayerStatsData Stats { get; set; }
        public NitroxVector3? LastStoredPosition { get; set; }

        public Player(ushort id, string name, bool isPermaDeath, PlayerContext playerContext, INitroxConnection connection,
                      NitroxVector3 position, NitroxId playerId, Optional<NitroxId> subRootId, Perms perms, PlayerStatsData stats,
                      IEnumerable<EquippedItemData> equippedItems, IEnumerable<EquippedItemData> modules)
        {
            Id = id;
            Name = name;
            IsPermaDeath = isPermaDeath;
            PlayerContext = playerContext;
            Connection = connection;
            Position = position;
            SubRootId = subRootId;
            GameObjectId = playerId;
            Permissions = perms;
            Stats = stats;
            LastStoredPosition = null;
            this.equippedItems = new ThreadSafeCollection<EquippedItemData>(equippedItems);
            this.modules = new ThreadSafeCollection<EquippedItemData>(modules);
            visibleCells = new ThreadSafeCollection<AbsoluteEntityCell>(new HashSet<AbsoluteEntityCell>(), false);
        }

        public void AddCells(IEnumerable<AbsoluteEntityCell> cells)
        {
            foreach (AbsoluteEntityCell cell in cells)
            {
                visibleCells.Add(cell);
            }
        }

        public void RemoveCells(IEnumerable<AbsoluteEntityCell> cells)
        {
            foreach (AbsoluteEntityCell cell in cells)
            {
                visibleCells.Remove(cell);
            }
        }

        public bool HasCellLoaded(AbsoluteEntityCell cell)
        {
            return visibleCells.Contains(cell);
        }

        public void AddModule(EquippedItemData module)
        {
            modules.Add(module);
        }

        public void RemoveModule(NitroxId id)
        {
            modules.RemoveAll(item => item.ItemId.Equals(id));
        }

        public List<EquippedItemData> GetModules()
        {
            return modules.ToList();
        }

        public void AddEquipment(EquippedItemData equipment)
        {
            equippedItems.Add(equipment);
        }

        public void RemoveEquipment(NitroxId id)
        {
            equippedItems.RemoveAll(item => item.ItemId.Equals(id));
        }

        public List<EquippedItemData> GetEquipment()
        {
            return equippedItems.ToList();
        }

        public bool CanSee(Entity entity)
        {
            return entity.ExistsInGlobalRoot || HasCellLoaded(entity.AbsoluteEntityCell);
        }

        public void SendPacket(Packet packet)
        {
            Connection.SendPacket(packet);
        }

        public void Teleport(NitroxVector3 destination)
        {
            PlayerTeleported playerTeleported = new PlayerTeleported(Name, Position, destination);

            SendPacket(playerTeleported);
            Position = playerTeleported.DestinationTo;
            LastStoredPosition = playerTeleported.DestinationFrom;
        }

        public bool Equals(Player other)
        {
            return Id == other?.Id;
        }

        public bool Equals(Player x, Player y)
        {
            return x?.Id == y?.Id;
        }

        public int GetHashCode(Player obj)
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return $"[Player - Id: {Id}, Name: {Name}, Perms: {Permissions}, Position: {Position}]";
        }
    }
}
