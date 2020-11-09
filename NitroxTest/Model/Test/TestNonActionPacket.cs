using System;
using NitroxModel.Packets;

namespace NitroxTest.Model.Test
{
    [Serializable]
    public class TestNonActionPacket : Packet
    {
        public ushort PlayerId { get; }

        public TestNonActionPacket(ushort playerId)
        {
            PlayerId = playerId;
        }
    }
}
