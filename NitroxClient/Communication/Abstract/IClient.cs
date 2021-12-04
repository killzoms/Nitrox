﻿using NitroxModel.Packets;
using NitroxModel.Networking;

namespace NitroxClient.Communication.Abstract
{
    // Abstracted IClient in order to give us options in the underlying protocol that we use to communicate with the server.
    // Ex: We may want to also roll a UDP client in the future to handle packets where we don't necessarily care
    // about transmission order or error recovery.
    public interface IClient
    {
        bool IsConnected { get; }
        void Start(IConnectionInfo connectionInfo);
        void Stop();
        void Send(Packet packet);
    }
}
