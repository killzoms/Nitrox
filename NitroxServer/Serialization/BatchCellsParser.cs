using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using NitroxModel.DataStructures;
using NitroxModel.DataStructures.GameLogic;
using NitroxModel.Discovery;
using NitroxModel.Helper;
using NitroxModel.Logger;
using NitroxServer.GameLogic.Entities.Spawning;
using NitroxServer.UnityStubs;
using ProtoBufNet;

namespace NitroxServer.Serialization
{
    /**
     * Parses the files in build18 in the format of batch-cells-x-y-z-slot-type.bin
     * These files contain serialized GameObjects with EntitySlot components. These
     * represent areas that entities (creatures, objects) can spawn within the world.
     * This class consolidates the gameObject, entitySlot, and cellHeader data to
     * create EntitySpawnPoint objects.
     */
    public class BatchCellsParser
    {
        private readonly EntitySpawnPointFactory entitySpawnPointFactory;
        private readonly ServerProtoBufSerializer serializer;
        private readonly Dictionary<string, Type> surrogateTypes;

        public BatchCellsParser(EntitySpawnPointFactory entitySpawnPointFactory, ServerProtoBufSerializer serializer)
        {
            this.entitySpawnPointFactory = entitySpawnPointFactory;
            this.serializer = serializer;

            surrogateTypes = new Dictionary<string, Type>
            {
                { "UnityEngine.Transform", typeof(NitroxTransform) },
                { "UnityEngine.Vector3", typeof(NitroxVector3) },
                { "UnityEngine.Quaternion", typeof(NitroxQuaternion) }
            };
        }

        public List<EntitySpawnPoint> ParseBatchData(NitroxInt3 batchId)
        {
            List<EntitySpawnPoint> spawnPoints = new List<EntitySpawnPoint>();

            ParseFile(batchId, "CellsCache", "baked-", "", spawnPoints);

            return spawnPoints;
        }

        public void ParseFile(NitroxInt3 batchId, string pathPrefix, string prefix, string suffix, List<EntitySpawnPoint> spawnPoints)
        {
            List<string> errors = new List<string>();
            string subnauticaPath = GameInstallationFinder.Instance.FindGame(errors);

            if (subnauticaPath == null)
            {
                Log.Error($"Could not locate Subnautica installation directory: {Environment.NewLine}{string.Join(Environment.NewLine, errors)}");
                return;
            }

            string path = Path.Combine(subnauticaPath, "Subnautica_Data", "StreamingAssets", "SNUnmanagedData", "Build18");
            string fileName = Path.Combine(path, pathPrefix, $"{prefix}batch-cells-{batchId.X}-{batchId.Y}-{batchId.Z}{suffix}.bin");

            if (!File.Exists(fileName))
            {
                return;
            }

            ParseCacheCells(batchId, fileName, spawnPoints);
        }

        /**
         * It is suspected that 'cache' is a misnomer carried over from when UWE was actually doing procedurally
         * generated worlds.  In the final release, this 'cache' has simply been baked into a final version that
         * we can parse. 
         */
        private void ParseCacheCells(NitroxInt3 batchId, string fileName, List<EntitySpawnPoint> spawnPoints)
        {
            using (Stream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                CellsFileHeader cellsFileHeader = serializer.Deserialize<CellsFileHeader>(stream);

                for (int cellCounter = 0; cellCounter < cellsFileHeader.NumCells; cellCounter++)
                {
                    CellHeaderEx cellHeader = serializer.Deserialize<CellHeaderEx>(stream);


                    byte[] serialData = new byte[cellHeader.DataLength];
                    stream.Read(serialData, 0, cellHeader.DataLength);
                    ParseGameObjectsWithHeader(serialData, batchId, cellHeader.CellId, cellHeader.Level, spawnPoints, out bool wasLegacy);

                    if (!wasLegacy)
                    {
                        byte[] legacyData = new byte[cellHeader.LegacyDataLength];
                        stream.Read(legacyData, 0, cellHeader.LegacyDataLength);
                        ParseGameObjectsWithHeader(legacyData, batchId, cellHeader.CellId, cellHeader.Level, spawnPoints, out _);

                        byte[] waiterData = new byte[cellHeader.WaiterDataLength];
                        stream.Read(waiterData, 0, cellHeader.WaiterDataLength);
                        ParseGameObjectsFromStream(new MemoryStream(waiterData), batchId, cellHeader.CellId, cellHeader.Level, spawnPoints);
                    }
                }
            }
        }

        private void ParseGameObjectsWithHeader(byte[] data, NitroxInt3 batchId, NitroxInt3 cellId, int level, List<EntitySpawnPoint> spawnPoints, out bool wasLegacy)
        {
            wasLegacy = false;

            if (data.Length == 0)
            {
                return;
            }

            Stream stream = new MemoryStream(data);

            StreamHeader header = serializer.Deserialize<StreamHeader>(stream);

            if (header == null)
            {
                return;
            }

            ParseGameObjectsFromStream(stream, batchId, cellId, level, spawnPoints);

            wasLegacy = (header.Version < 9);
        }

        private void ParseGameObjectsFromStream(Stream stream, NitroxInt3 batchId, NitroxInt3 cellId, int level, List<EntitySpawnPoint> spawnPoints)
        {
            LoopHeader gameObjectCount = serializer.Deserialize<LoopHeader>(stream);

            for (int goCounter = 0; goCounter < gameObjectCount.Count; goCounter++)
            {
                GameObject gameObject = DeserializeGameObject(stream);

                if (gameObject.TotalComponents > 0)
                {

                    AbsoluteEntityCell absoluteEntityCell = new AbsoluteEntityCell(batchId, cellId, level);
                    NitroxTransform transform = gameObject.GetComponent<NitroxTransform>();
                    spawnPoints.AddRange(entitySpawnPointFactory.From(absoluteEntityCell, transform, gameObject));
                }
            }
        }

        private GameObject DeserializeGameObject(Stream stream)
        {
            GameObjectData goData = serializer.Deserialize<GameObjectData>(stream);

            GameObject gameObject = new GameObject(goData);
            DeserializeComponents(stream, gameObject);

            return gameObject;
        }

        private void DeserializeComponents(Stream stream, GameObject gameObject)
        {
            LoopHeader components = serializer.Deserialize<LoopHeader>(stream);

            for (int componentCounter = 0; componentCounter < components.Count; componentCounter++)
            {
                ComponentHeader componentHeader = serializer.Deserialize<ComponentHeader>(stream);

                if (!surrogateTypes.TryGetValue(componentHeader.TypeName, out Type type))
                {
                    type = AppDomain.CurrentDomain.GetAssemblies()
                        .Select(a => a.GetType(componentHeader.TypeName))
                        .FirstOrDefault(t => t != null);
                }

                Validate.NotNull(type, $"No type or surrogate found for {componentHeader.TypeName}!");

                object component = FormatterServices.GetUninitializedObject(type);
                serializer.Deserialize(stream, component, type);

                gameObject.AddComponent(component, type);
            }
        }
    }

    [ProtoContract]
    public class CellsFileHeader
    {
        [ProtoMember(1)]
        public int Version { get; }

        [ProtoMember(2)]
        public int NumCells { get; }

        public override string ToString() => $"(version={Version}, numCells={NumCells})";
    }

    [ProtoContract]
    public class CellHeader
    {
        [ProtoMember(1)]
        public NitroxInt3 CellId;

        [ProtoMember(2)]
        public int Level { get; }

        public override string ToString() => $"(cellId={CellId}, level={Level})";
    }

    [ProtoContract]
    public class CellHeaderEx
    {
        [ProtoMember(1)]
        public NitroxInt3 CellId;

        [ProtoMember(2)]
        public int Level { get; }

        [ProtoMember(3)]
        public int DataLength { get; }

        [ProtoMember(4)]
        public int LegacyDataLength { get; }

        [ProtoMember(5)]
        public int WaiterDataLength { get; }

        public override string ToString() => $"(cellId={CellId}, level={Level}, dataLength={DataLength}, legacyDataLength={LegacyDataLength}, waiterDataLength={WaiterDataLength})";
    }

    [ProtoContract]
    public class StreamHeader
    {
        [ProtoMember(1)]
        public int Signature { get; private set; }

        [ProtoMember(2)]
        public int Version { get; private set; }

        public void Reset()
        {
            Signature = 0;
            Version = 0;
        }

        public override string ToString() => $"(UniqueIdentifier={Signature}, Version={Version})";
    }

    [ProtoContract]
    public class LoopHeader
    {
        [ProtoMember(1)]
        public int Count { get; private set; }

        public void Reset()
        {
            Count = 0;
        }
        public override string ToString() => $"(Count={Count})";
    }

    [ProtoContract]
    public class GameObjectData
    {
        [ProtoMember(1)]
        public bool CreateEmptyObject { get; private set; }

        [ProtoMember(2)]
        public bool IsActive { get; private set; }

        [ProtoMember(3)]
        public int Layer { get; private set; }

        [ProtoMember(4)]
        public string Tag { get; private set; }

        [ProtoMember(6)]
        public string Id { get; private set; }

        [ProtoMember(7)]
        public string ClassId { get; private set; }

        [ProtoMember(8)]
        public string Parent { get; private set; }

        [ProtoMember(9)]
        public bool OverridePrefab { get; private set; }

        [ProtoMember(10)]
        public bool MergeObject { get; private set; }

        public void Reset()
        {
            CreateEmptyObject = false;
            IsActive = false;
            Layer = 0;
            Tag = null;
            Id = null;
            ClassId = null;
            Parent = null;
            OverridePrefab = false;
            MergeObject = false;
        }

        public override string ToString() => $"(CreateEmptyObject={CreateEmptyObject}, IsActive={IsActive}, Layer={Layer}, Tag={Tag}, Id={Id}, ClassId={ClassId}, Parent={Parent}, OverridePrefab={OverridePrefab}, MergeObject={MergeObject})";
    }

    [ProtoContract]
    public class ComponentHeader
    {
        [ProtoMember(1)]
        public string TypeName { get; private set; }

        [ProtoMember(2)]
        public bool IsEnabled { get; private set; }

        public void Reset()
        {
            TypeName = null;
            IsEnabled = false;
        }

        public override string ToString() => $"(TypeName={TypeName}, IsEnabled={IsEnabled})";
    }
}
