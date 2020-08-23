﻿using System;
using NitroxModel.MultiplayerSession;

namespace NitroxModel.Packets
{
    [Serializable]
    public class MultiplayerSessionPolicy : CorrelatedPacket
    {
        public bool RequiresServerPassword { get; }
        public bool DisableConsole { get; }
        public int MaxConnections { get; }

        public MultiplayerSessionAuthenticationAuthority AuthenticationAuthority { get; }
        public Version NitroxVersionAllowed { get; }

        public MultiplayerSessionPolicy(string correlationId, bool disableConsole, int maxConnections) : base(correlationId)
        {
            // This is done intentionally. It is only a stub for future extension.
            RequiresServerPassword = false;
            AuthenticationAuthority = MultiplayerSessionAuthenticationAuthority.SERVER;
            DisableConsole = disableConsole;
            MaxConnections = maxConnections;
            // get the full version name
            Version ver = typeof(MultiplayerSessionPolicy).Assembly.GetName().Version;
            // only the major and minor version number is required
            NitroxVersionAllowed = new Version(ver.Major, ver.Minor);
        }

        public override string ToString()
        {
            return $"[MultiplayerSessionPolicy - RequiresServerPassword: {RequiresServerPassword}, DisableConsole: {DisableConsole}, AuthenticationAuthority: {AuthenticationAuthority}, NitroxVersionAllowed: {NitroxVersionAllowed}]";
        }
    }
}
