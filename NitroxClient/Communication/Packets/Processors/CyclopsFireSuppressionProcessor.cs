﻿using NitroxClient.Communication.Packets.Processors.Abstract;
using NitroxClient.GameLogic;
using NitroxModel.Subnautica.Packets;

namespace NitroxClient.Communication.Packets.Processors
{
    public class CyclopsFireSuppressionProcessor : ClientPacketProcessor<CyclopsFireSuppression>
    {
        private readonly Cyclops cyclops;

        public CyclopsFireSuppressionProcessor(Cyclops cyclops)
        {
            this.cyclops = cyclops;
        }

        public override void Process(CyclopsFireSuppression fireSuppressionPacket)
        {
            cyclops.StartFireSuppression(fireSuppressionPacket.Id);
        }
    }
}
