using System;

namespace NitroxModel.Packets
{
    [Serializable]
    public class ServerCommand : Packet
    {
        public string Command { get; }

        public ServerCommand(string command)
        {
            Command = command;
        }

        public ServerCommand(string[] CommandArgs)
        {
            Command = string.Join(" ", CommandArgs);
        }

        public override string ToString()
        {
            return $"[ServerCommand - Command: {Command}]";
        }
    }
}
