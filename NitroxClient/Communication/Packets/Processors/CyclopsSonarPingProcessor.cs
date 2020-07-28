﻿using NitroxClient.Communication.Packets.Processors.Abstract;
using NitroxClient.GameLogic;
using NitroxModel_Subnautica.Packets;

namespace NitroxClient.Communication.Packets.Processors
{
    class CyclopsSonarPingProcessor : ClientPacketProcessor<CyclopsSonarPing>
    {
        private readonly Cyclops cyclops;

        public CyclopsSonarPingProcessor(Cyclops cyclops)
        {
            this.cyclops = cyclops;
        }

        public override void Process(CyclopsSonarPing packet)
        {
            cyclops.SonarPing(packet.Id);
        }
    }
}
