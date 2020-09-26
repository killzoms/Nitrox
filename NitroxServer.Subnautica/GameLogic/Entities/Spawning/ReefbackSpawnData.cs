using System.Collections.Generic;
using NitroxModel.DataStructures.GameLogic;

namespace NitroxServer.Subnautica.GameLogic.Entities.Spawning
{
    public static class ReefbackSpawnData
    {
        public readonly struct ReefbackEntity
        {
            public TechType TechType { get; }
            public float Probability { get; }
            public int MinNumber { get; }
            public int MaxNumber { get; }
            public string ClassId { get; }

            public ReefbackEntity(TechType techType, float probability, int minNumber, int maxNumber, string classId)
            {
                TechType = techType;
                Probability = probability;
                MinNumber = minNumber;
                MaxNumber = maxNumber;
                ClassId = classId;
            }
        }

        public static List<ReefbackEntity> SpawnableCreatures { get; } = new List<ReefbackEntity>()
        {
            new ReefbackEntity(TechType.Peeper, 1f, 1, 2, "3fcd548b-781f-46ba-b076-7412608deeef"),
            new ReefbackEntity(TechType.Boomerang, 1f, 1, 2, "fa4cfe65-4eaf-4d51-ba0d-e8cc9632fd47"),
            new ReefbackEntity(TechType.Hoverfish, 1f, 1, 2, "0a993944-87d3-441e-b21d-6c314f723cc7"),
            new ReefbackEntity(TechType.Bladderfish, 1f, 1, 2, "bf9ccd04-60af-4144-aaa1-4ac184c686c2"),
            new ReefbackEntity(TechType.Eyeye, 1f, 1, 1, "79c1aef0-e505-469c-ab36-c22c76aeae44"),
            new ReefbackEntity(TechType.HoleFish, 1f, 1, 1, "495befa0-0e6b-400d-9734-227e5a732f75"),
            new ReefbackEntity(TechType.Hoopfish, 1f, 1, 1, "284ceeb6-b437-4aca-a8bd-d54f336cbef8"),
            new ReefbackEntity(TechType.Reginald, 1f, 1, 2, "8e82dc63-5991-4c63-a12c-2aa39373a7cf"),
            new ReefbackEntity(TechType.Spadefish, 1f, 1, 2, "d040bec1-0368-4f7c-aed6-93b5e1852d45"),
            new ReefbackEntity(TechType.Biter, 0.5f, 1, 3, "4064a71a-c464-4db2-942a-56391fe69951"),
            new ReefbackEntity(TechType.HoopfishSchool, 1f, 3, 3, "08cb3290-504b-4191-97ee-6af1588af5c0")
        };

        public static List<NitroxVector3> LocalCreatureSpawnPoints { get; } = new List<NitroxVector3>()
        {
            new NitroxVector3(-22.9f, 17.0f, 0.0f),
            new NitroxVector3(5.1f, 17.9f, 22.1f),
            new NitroxVector3(5.1f, 17.9f, -11.6f),
            new NitroxVector3(-5.1f, 17.9f, 5.6f),
            new NitroxVector3(23.6f, 17.9f, 5.6f),
            new NitroxVector3(-16.4f, 17.9f, 25.9f),
            new NitroxVector3(-8.7f, 17.9f, -30.3f),
            new NitroxVector3(15.4f, 17.9f, -30.3f),
            new NitroxVector3(20.9f, 17.9f, -13.9f),
            new NitroxVector3(-17.3f, 17.9f, 22.1f)
        };
    }
}
