using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AssetsTools.NET;
using NitroxServer.Serialization.Resources.Datastructures;
using NitroxModel.DataStructures.GameLogic.Pda;
using NitroxModel_Subnautica.DataStructures;
using NitroxModel.DataStructures.GameLogic;

namespace NitroxServer_Subnautica.Serialization.Resources.Parsers.Monobehaviours
{
    public class PDADataParser : MonobehaviourParser
    {
        public static NitroxPdaData pdaData;

        public override void Parse(AssetIdentifier identifier, AssetIdentifier gameObjectIdentifier, AssetsFileReader reader, ResourceAssets resourceAssets, Dictionary<int, string> relativeFileIdToPath)
        {
            pdaData = new NitroxPdaData();
            reader.Align();
            reader.Position += 12;

            int count = reader.ReadInt32(); // Log
            List<NitroxPdaLog.EntryData> pdaLogEntryData = new List<NitroxPdaLog.EntryData>(count);
            for (int i = 0; i < count; i++)
            {
                NitroxPdaLog.EntryData entryData = new NitroxPdaLog.EntryData();
                entryData.Key = reader.ReadCountStringInt32();
                reader.Align();
                entryData.EntryType = reader.ReadInt32();
                pdaLogEntryData.Add(entryData);

                reader.Position += 24;
            }

            pdaData.Log = pdaLogEntryData;

            count = reader.ReadInt32(); // Encyclopedia
            List<NitroxPdaEncyclopedia.EntryData> encyclopediaData = new List<NitroxPdaEncyclopedia.EntryData>(count);
            for (int i = 0; i < count; i++)
            {
                NitroxPdaEncyclopedia.EntryData entryData = new NitroxPdaEncyclopedia.EntryData();
                entryData.Key = reader.ReadCountStringInt32();
                reader.Align();
                entryData.Path = reader.ReadCountStringInt32();
                reader.Align();
                entryData.Unlocked = reader.ReadBoolean();
                reader.Align();

                encyclopediaData.Add(entryData);

                reader.Position += 48;
            }

            pdaData.Encyclopedia = encyclopediaData;

            count = reader.ReadInt32(); // Scanner
            List<NitroxPdaScanner.EntryData> scannerData = new List<NitroxPdaScanner.EntryData>(count);
            for (int i = 0; i < count; i++)
            {
                NitroxPdaScanner.EntryData entryData = new NitroxPdaScanner.EntryData();
                entryData.Key = ((TechType)reader.ReadInt32()).ToDto();
                entryData.Locked = reader.ReadBoolean();
                reader.Align();
                entryData.TotalFragments = reader.ReadInt32();
                entryData.DestroyAfterScan = reader.ReadBoolean();
                reader.Align();
                entryData.Encyclopedia = reader.ReadCountStringInt32();
                reader.Align();
                entryData.Blueprint = ((TechType)reader.ReadInt32()).ToDto();
                entryData.ScanTime = reader.ReadSingle();
                entryData.IsFragment = reader.ReadBoolean();
                reader.Align();
                scannerData.Add(entryData);
            }

            pdaData.Scanner = scannerData;

            count = reader.ReadInt32(); // Default Tech
            pdaData.DefaultTech = new List<NitroxTechType>(count);
            for (int i = 0; i < count;i++)
            {
                TechType defaultTech = (TechType)reader.ReadInt32();
                pdaData.DefaultTech.Add(defaultTech.ToDto());
                reader.Align();
            }

            count = reader.ReadInt32(); // Analysis Tech
            List<NitroxKnownTech.AnalysisTech> analysisTechList = new List<NitroxKnownTech.AnalysisTech>(count);
            for (int i = 0; i < count;i++)
            {
                NitroxKnownTech.AnalysisTech analysisTech = new NitroxKnownTech.AnalysisTech();
                analysisTech.TechType = ((TechType)reader.ReadInt32()).ToDto();
                analysisTech.UnlockMessage = reader.ReadCountStringInt32();
                reader.Align();

                reader.Position += 24;

                int internalCount = reader.ReadInt32();
                analysisTech.UnlockTechTypes = new List<NitroxTechType>(internalCount);
                for (int j = 0; j < internalCount; j++)
                {
                    TechType unlockTechType = (TechType)reader.ReadInt32();
                    analysisTech.UnlockTechTypes.Add(unlockTechType.ToDto());
                }
                analysisTechList.Add(analysisTech);
            }

            pdaData.AnalysisTech = analysisTechList;

            count = reader.ReadInt32(); // Compound Tech
            List<NitroxKnownTech.CompoundTech> compoundTechList = new List<NitroxKnownTech.CompoundTech>(count);
            for (int i = 0; i < count; i++)
            {
                NitroxKnownTech.CompoundTech compoundTech = new NitroxKnownTech.CompoundTech();
                compoundTech.UnlockTechType = ((TechType)reader.ReadInt32()).ToDto();
                reader.Align();

                int internalCount = reader.ReadInt32();
                compoundTech.Dependencies = new List<NitroxTechType>(internalCount);
                for (int j = 0; j < internalCount; j++)
                {
                    TechType dependsTechType = (TechType)reader.ReadInt32();
                    compoundTech.Dependencies.Add(dependsTechType.ToDto());
                    reader.Align();
                }
                compoundTechList.Add(compoundTech);
            }

            pdaData.CompoundTech = compoundTechList;

            resourceAssets.NitroxPdaData = pdaData;
        }
    }
}
