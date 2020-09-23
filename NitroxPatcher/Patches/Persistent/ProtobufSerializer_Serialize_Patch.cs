using System;
using System.IO;
using System.Reflection;
using Harmony;
using NitroxClient.Helpers;
using NitroxClient.MonoBehaviours;
using NitroxModel.Core;

namespace NitroxPatcher.Patches.Persistent
{
    public class ProtobufSerializer_Serialize_Patch : NitroxPatch, IPersistentPatch
    {
        private static readonly MethodInfo targetMethod = typeof(ProtobufSerializer).GetMethod("Serialize", BindingFlags.NonPublic | BindingFlags.Instance);
        /// <summary>
        ///     This patch is in a hot path so it needs this optimization.
        /// </summary>
        private static readonly NitroxProtobufSerializer serializer = NitroxServiceLocator.LocateServicePreLifetime<NitroxProtobufSerializer>();

        public static bool Prefix(Stream stream, object source, Type type)
        {
            if (Multiplayer.Active && serializer.NitroxTypes.TryGetValue(type, out int _))
            {
                serializer.Serialize(stream, source);
                return false;
            }

            return true;
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchPrefix(harmony, targetMethod);
        }
    }
}
