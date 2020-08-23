using System;
using System.ComponentModel;

namespace NitroxModel.MultiplayerSession
{
    [Flags]
    public enum MultiplayerSessionReservationStates
    {
        RESERVED = 0,
        REJECTED = 1 << 0,

        [Description("The player name is already in use. Please try again with a different name.")]
        UNIQUE_PLAYER_NAME_CONSTRAINT_VIOLATED = 1 << 1,

        // These are all intended for future use. Maybe YAGNI, but this is where we should look to expand upon server reservations
        [Description("The server is currently at capacity. Please try again later.")]
        SERVER_PLAYER_CAPACITY_REACHED = 1 << 2,

        [Description("The password that you provided for the server is incorrect.")]
        AUTHENTICATION_FAILED = 1 << 3,

        [Description("The server is using hardcore gamemode, player is dead.")]
        HARDCORE_PLAYER_DEAD = 1 << 4,
    }
}
