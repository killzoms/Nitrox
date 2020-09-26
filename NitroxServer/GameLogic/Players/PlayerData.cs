using System.Collections.Generic;
using System.Linq;
using NitroxModel.DataStructures.Util;
using ProtoBufNet;

namespace NitroxServer.GameLogic.Players
{
    [ProtoContract]
    public class PlayerData
    {
        public const short VERSION = 2;

        [ProtoMember(1)]
        private List<PersistedPlayerData> players = new List<PersistedPlayerData>();

        public List<Player> GetPlayers()
        {
            List<Player> boPlayers = new List<Player>();

            foreach (PersistedPlayerData playerData in players)
            {
                Player player = new Player(playerData.Id,
                                           playerData.Name,
                                           playerData.IsPermaDeath,
                                           null, //no connection/context as this player is not connected.
                                           null,
                                           playerData.SpawnPosition,
                                           playerData.NitroxId,
                                           Optional.OfNullable(playerData.SubRootId),
                                           playerData.Permissions,
                                           playerData.CurrentStats,
                                           playerData.EquippedItems,
                                           playerData.Modules);
                boPlayers.Add(player);
            }

            return boPlayers;
        }

        public static PlayerData From(IEnumerable<Player> players)
        {
            List<PersistedPlayerData> persistedPlayers = players.Select(player => new PersistedPlayerData
            {
                Name = player.Name,
                EquippedItems = player.GetEquipment(),
                Modules = player.GetModules(),
                Id = player.Id,
                SpawnPosition = player.Position,
                CurrentStats = player.Stats,
                SubRootId = player.SubRootId.OrElse(null),
                Permissions = player.Permissions,
                NitroxId = player.GameObjectId,
                IsPermaDeath = player.IsPermaDeath
            }).ToList();

            return new PlayerData { players = persistedPlayers };
        }
    }
}
