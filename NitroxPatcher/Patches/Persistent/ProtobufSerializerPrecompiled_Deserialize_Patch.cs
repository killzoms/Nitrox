﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Harmony;
using ProtoBuf;
using NitroxClient.Helpers;

namespace NitroxPatcher.Patches.Persistent
{
    public class ProtobufSerializerPrecompiled_Deserialize_Patch : NitroxPatch
    {
        static Type TARGET_TYPE = typeof(ProtobufSerializerPrecompiled);
        MethodInfo TARGET_METHOD = TARGET_TYPE.GetMethod("Deserialize", BindingFlags.Instance | BindingFlags.NonPublic);

        public static bool Prefix(int num, object obj, ProtoReader reader, ref object __result)
        {
            if (num == int.MaxValue)
            {
                int key = (int)NitroxProtobufSerializer.Main.model.GetType().InvokeMember("GetKey", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod, null, NitroxProtobufSerializer.Main.model, new object[] { obj.GetType(), false, true });
                __result = NitroxProtobufSerializer.Main.model.GetType().InvokeMember("Deserialize", BindingFlags.NonPublic | BindingFlags.InvokeMethod | BindingFlags.Instance, null, NitroxProtobufSerializer.Main.model, new object[] { key, obj, reader });
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
