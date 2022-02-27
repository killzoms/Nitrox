using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroxModel.DataStructures.GameLogic.Pda
{
    public static class NitroxPdaEncyclopedia
    {
        private static bool initialized;
        private static Dictionary<string, Entry> entries;
        private static Dictionary<string, EntryData> mapping;

        public static void Initialize(NitroxPdaData pdaData)
        {
            if (initialized)
            {
                return;
            }
            initialized = true;
            List<EntryData> encyclopedia = pdaData.Encyclopedia;
            entries = new Dictionary<string, Entry>(StringComparer.OrdinalIgnoreCase);
            mapping = new Dictionary<string, EntryData>(StringComparer.OrdinalIgnoreCase);
            int i = 0;
            for (int count = encyclopedia.Count; i < count; i++)
            {
                EntryData entryData = encyclopedia[i];
                string key = entryData.Key;
                if (string.IsNullOrEmpty(key))
                {
                    Log.Error("PDAEncyclopedia : Initialize() : Empty key found at index " + i);
                }
                else if (mapping.ContainsKey(key))
                {
                    Log.Error(string.Format("PDAEncyclopedia : Initialize() : Duplicate key '{0}' found at index {1}.", key, i));
                }
                else
                {
                    mapping.Add(key, entryData);
                }
            }
            int j = 0;
            for (int count2 = encyclopedia.Count; j < count2; j++)
            {
                EntryData entryData2 = encyclopedia[j];
                if (entryData2.Unlocked)
                {
                    Add(entryData2.Key, null, verbose: false);
                }
            }
        }

        private static EntryData Add(string key, Entry entry, bool verbose)
        {
            if (!ContainsEntry(key))
            {
                if (entry == null)
                {
                    entry = new Entry();
                    /*if (Application.isPlaying)
                    {
                        entry.timestamp = DayNightCycle.main.timePassedAsFloat;
                    }*/
                    entry.Timestamp = 0f;
                }
                entries.Add(key, entry);
                if (GetEntryData(key, out EntryData entryData))
                {
                    entry.TimeCapsuleId = null;
                }
                else if (!string.IsNullOrEmpty(entry.TimeCapsuleId))
                {
                    if (!string.IsNullOrEmpty(entry.TimeCapsuleId))
                    {
                        entryData = new EntryData();
                        entryData.TimeCapsule = true;
                        entryData.Key = entry.TimeCapsuleId;
                        entryData.Path = "TimeCapsules";
                        entryData.Unlocked = false;
                        mapping.Add(entry.TimeCapsuleId, entryData);
                    }
                }
                else
                {
                    Log.Error("PDAEncyclopedia : Add() : Entry for key='" + key + "' is not found! It is either never existed in PDAData prefab or was removed.");
                }
                if (entryData != null)
                {
                    return entryData;
                }
            }
            return null;
        }

        internal static bool HasEntryData(string encyclopedia)
        {
            if (mapping == null)
            {
                return false;
            }

            return mapping.ContainsKey(encyclopedia);
        }

        private static bool GetEntryData(string key, out EntryData entryData)
        {
            if (mapping == null)
            {
                entryData = null;
                return false;
            }

            return mapping.TryGetValue(key, out entryData);
        }

        private static bool ContainsEntry(string key)
        {
            if (entries == null)
            {
                return false;
            }

            return entries.ContainsKey(key);
        }

        public class Entry
        {
            public float Timestamp;
            public string TimeCapsuleId;
        }

        public class EntryData
        {
            public string Key;
            public string Path;
            public bool Unlocked;
            public bool TimeCapsule;
        }
    }
}
