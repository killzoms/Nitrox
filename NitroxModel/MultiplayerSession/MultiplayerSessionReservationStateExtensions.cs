using System;
using System.ComponentModel;
using System.Text;

namespace NitroxModel.MultiplayerSession
{
    public static class MultiplayerSessionReservationStateExtensions
    {
        public static bool HasStateFlag(this MultiplayerSessionReservationStates currentState, MultiplayerSessionReservationStates checkedState)
        {
            return (currentState & checkedState) == checkedState;
        }

        public static string Describe(this MultiplayerSessionReservationStates currentState)
        {
            StringBuilder descriptionBuilder = new StringBuilder();

            foreach (string reservationStateName in Enum.GetNames(typeof(MultiplayerSessionReservationStates)))
            {
                MultiplayerSessionReservationStates reservationState = (MultiplayerSessionReservationStates)Enum.Parse(typeof(MultiplayerSessionReservationStates), reservationStateName);
                if (currentState.HasStateFlag(reservationState))
                {
                    DescriptionAttribute descriptionAttribute = reservationState.GetAttribute<DescriptionAttribute>();

                    if (!string.IsNullOrEmpty(descriptionAttribute?.Description))
                    {
                        descriptionBuilder.AppendLine(descriptionAttribute.Description);
                    }
                }
            }

            return descriptionBuilder.ToString();
        }
    }
}
