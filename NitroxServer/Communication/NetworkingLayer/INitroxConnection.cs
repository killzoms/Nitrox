using System.Net;
using NitroxModel.Packets;
using NitroxModel.Packets.Processors.Abstract;

namespace NitroxServer.Communication.NetworkingLayer
{
    public interface INitroxConnection : IProcessorContext
    {
        IPEndPoint Endpoint { get; }
        void SendPacket(Packet packet);
    }
}
