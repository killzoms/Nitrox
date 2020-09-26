using UnityEngine;

namespace NitroxModel.Subnautica.DataStructures.GameLogic.Creatures.Actions
{
    public interface ISerializableCreatureAction
    {
        CreatureAction GetCreatureAction(GameObject gameObject);
    }
}
