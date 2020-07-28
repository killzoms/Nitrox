﻿using NitroxClient.Communication.Packets.Processors.Abstract;
using NitroxClient.GameLogic;
using NitroxModel_Subnautica.Packets;

namespace NitroxClient.Communication.Packets.Processors
{
    public class CyclopsFireSuppressionProcessor : ClientPacketProcessor<CyclopsFireSuppression>
    {
        private readonly Cyclops cyclops;

        public CyclopsFireSuppressionProcessor(Cyclops cyclops)
        {
            this.cyclops = cyclops;
        }

        public override void Process(CyclopsFireSuppression packet)
        {
            cyclops.StartFireSuppression(packet.Id);
        }
    }
}
