using NitroxModel.DataStructures.GameLogic;
using NitroxModel.DataStructures.GameLogic.Entities;
using NitroxModel.DataStructures.Util;
using NitroxModel.Packets;
using NitroxServer.Communication.Packets.Processors.Abstract;
using NitroxServer.GameLogic;
using NitroxServer.GameLogic.Entities;

namespace NitroxServer.Communication.Packets.Processors;

public class EntityReparentedProcessor : AuthenticatedPacketProcessor<EntityReparented>
{
    private readonly PlayerManager playerManager;

    public EntityReparentedProcessor(PlayerManager playerManager)
    {
        this.playerManager = playerManager;
    }

    public override void Process(EntityReparented packet, Player player)
    {
        EntityRegistry.SetParent(packet.Id, packet.NewParentId);
        playerManager.SendPacketToOtherPlayers(packet, player);
    }
}
