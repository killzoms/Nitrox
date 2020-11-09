using System;
using NitroxModel.DataStructures;

namespace NitroxModel.MultiplayerSession
{
    [Serializable]
    public class AuthenticationContext
    {
        public string Username { get; }
        public Optional<string> ServerPassword { get; }

        public AuthenticationContext(string username, string serverPassword = null)
        {
            Username = username;
            ServerPassword = Optional.OfNullable(serverPassword);
        }

        public override string ToString()
        {
#if DEBUG
            return $"[AuthenticationContext - Username: {Username} ServerPassword: {ServerPassword}]";
#else
            return $"[AuthenticationContext - Username: {Username} ServerPassword: *****]";
#endif
        }
    }
}
