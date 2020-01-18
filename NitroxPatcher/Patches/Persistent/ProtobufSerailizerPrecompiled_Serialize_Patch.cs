using System;
using System.IO;
using System.Reflection;
using Harmony;
using NitroxClient.Helpers;
using NitroxModel.Core;

namespace NitroxPatcher.Patches.Persistent
{
    public class ProtobufSerializerPrecompiled_Serialize_Patch : NitroxPatch, IPersistentPatch
    {
        private static readonly Type TARGET_TYPE = typeof(ProtobufSerializer);
        private static readonly MethodInfo TARGET_METHOD = TARGET_TYPE.GetMethod("Serialize", BindingFlags.Instance | BindingFlags.NonPublic);
        private static NitroxProtobufSerializer serializer { get; } = NitroxServiceLocator.LocateServiceNoScope<NitroxProtobufSerializer>();

        public static bool Prefix(Stream stream, object source, Type type)
        {
            int key;
            if (serializer.NitroxTypes.TryGetValue(type, out key))
            {
                serializer.Serialize(stream, source);
                return false;
            }

            return true;
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchPrefix(harmony, TARGET_METHOD);
        }
    }
}
