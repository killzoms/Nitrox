﻿using NitroxClient.Communication.Packets.Processors.Abstract;
using NitroxClient.GameLogic;
using NitroxModel.Subnautica.Packets;

namespace NitroxClient.Communication.Packets.Processors
{
    class CyclopsSonarPingProcessor : ClientPacketProcessor<CyclopsSonarPing>
    {
        private readonly Cyclops cyclops;

        public CyclopsSonarPingProcessor(Cyclops cyclops)
        {
            this.cyclops = cyclops;
        }

        public override void Process(CyclopsSonarPing sonarPacket)
        {
            cyclops.SonarPing(sonarPacket.Id);
        }
    }
}
