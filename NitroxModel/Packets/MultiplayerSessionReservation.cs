﻿using System;
using NitroxModel.MultiplayerSession;

namespace NitroxModel.Packets
{
    [Serializable]
    public class MultiplayerSessionReservation : CorrelatedPacket
    {
        public MultiplayerSessionReservationStates ReservationState { get; }
        public ushort PlayerId { get; }
        public string ReservationKey { get; }

        public MultiplayerSessionReservation(string correlationId, MultiplayerSessionReservationStates reservationState)
            : base(correlationId)
        {
            ReservationState = reservationState;
        }

        public MultiplayerSessionReservation(string correlationId, ushort playerId, string reservationKey)
            : this(correlationId, MultiplayerSessionReservationStates.RESERVED)
        {
            PlayerId = playerId;
            ReservationKey = reservationKey;
        }

        public override string ToString()
        {
            return $"[MultiplayerSessionReservation - ReservationState: {ReservationState}, PlayerId: {PlayerId}, ReservationKey: {ReservationKey}]";
        }
    }
}
