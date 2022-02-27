using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NitroxModel.Helper;

namespace NitroxModel.DataStructures.GameLogic.Pda
{
    public static class NitroxPdaScanner
    {
        private static bool initialized;
        private static Dictionary<string, float> fragments;
        private static List<Entry> partial;
        private static HashSet<NitroxTechType> complete;
        private static Dictionary<NitroxTechType, EntryData> mapping;

        public static void Initialize(NitroxPdaData pdaData)
        {
            if (initialized)
            {
                return;
            }
            initialized = true;
            fragments = new Dictionary<string, float>();
            partial = new List<Entry>();
            complete = new HashSet<NitroxTechType>();
            mapping = new Dictionary<NitroxTechType, EntryData>();
            List<EntryData> scanner = pdaData.Scanner;
            int i = 0;
            for (int count = scanner.Count; i < count; i++)
            {
                EntryData entryData = scanner[i];
                NitroxTechType key = entryData.Key;

                if (key.Name == "None")
                {
                    Log.Error("NitroxPDAScanner : Initialize() : TechType.None key found at index " + i);
                    continue;
                }

                if (mapping.ContainsKey(key))
                {
                    Log.Error(string.Concat("NitroxPDAScanner : Initialize() : Duplicate key '", key, "' found at index ", i));
                    continue;
                }
                if (entryData.TotalFragments < 1 || entryData.TotalFragments > 30)
                {
                    entryData.TotalFragments = Mathf.Clamp(entryData.TotalFragments, 1, 30);
                    Log.Error(string.Format("NitroxPDAScanner : Initialize() : Number of fragments cannot be less than {0} or greater than {1}! (TechType = {2}, index = {3}). Clamped to {4}", 1, 30, key, i, entryData.TotalFragments));
                }
                string encyclopedia = entryData.Encyclopedia;
                if (!string.IsNullOrEmpty(encyclopedia) && !NitroxPdaEncyclopedia.HasEntryData(encyclopedia))
                {
                    entryData.Encyclopedia = string.Empty;
                    Log.Error("PDAScanner : Initialize() : '" + encyclopedia + "' is an unknown entry for PDAEncyclopedia! (index = " + i + ")");
                }
                mapping.Add(key, entryData);
            }


            foreach (KeyValuePair<NitroxTechType, EntryData> kvPair in mapping)
            {
                if (kvPair.Value.Locked)
                {
                    Add(kvPair.Key, 0);
                }
            }
        }

        private static Entry Add(NitroxTechType techType, int unlocked)
        {
            if (unlocked < 0)
            {
                unlocked = 0;
            }
            if (techType.Name == "None" || complete.Contains(techType))
            {
                return null;
            }
            if (GetPartialEntryByKey(techType, out Entry entry))
            {
                if (unlocked > entry.Unlocked)
                {
                    entry.Unlocked = unlocked;
                    //NotifyProgress(entry);
                }
            }
            else
            {
                entry = new Entry();
                entry.TechType = techType;
                entry.Unlocked = unlocked;
                partial.Add(entry);
                //NotifyAdd(entry);
            }
            return entry;
        }

        private static bool GetPartialEntryByKey(NitroxTechType key, out Entry entry)
        {
            int i = 0;
            for (int count = partial.Count; i < count; i++)
            {
                Entry entry2 = partial[i];
                if (entry2.TechType == key)
                {
                    entry = entry2;
                    return true;
                }
            }
            entry = null;
            return false;
        }

        public static bool IsFragment(NitroxTechType techType)
        {
            if (techType != null && mapping.TryGetValue(techType, out EntryData value))
            {
                return value.IsFragment;
            }
            return false;
        }

        public static bool ContainsCompleteEntry(NitroxTechType techType)
        {
            if (complete == null)
            {
                return false;
            }
            return complete.Contains(techType);
        }

        public class Entry
        {
            public NitroxTechType TechType;
            public float Progress;
            public int Unlocked;
        }

        public struct EntryData
        {
            public NitroxTechType Key;
            public bool Locked;
            public int TotalFragments;
            public bool DestroyAfterScan;
            public string Encyclopedia;
            public NitroxTechType Blueprint;
            public float ScanTime;
            public bool IsFragment;
        }
    }
}
