using NitroxModel.DataStructures.GameLogic;
using NitroxModel.Subnautica.DataStructures;
using NitroxServer.GameLogic.Entities.Spawning;

namespace NitroxServer.Subnautica.GameLogic.Entities.Spawning
{
    public class CrashFishBootstrapper : IEntityBootstrapper
    {
        public void Prepare(Entity parentEntity, DeterministicBatchGenerator deterministicBatchGenerator)
        {
            Entity crashFish = SpawnChild(parentEntity, deterministicBatchGenerator, TechType.Crash, "7d307502-46b7-4f86-afb0-65fe8867f893");
            crashFish.Transform.LocalRotation = new NitroxQuaternion(-0.7071068f, 0, 0, 0.7071068f);
            parentEntity.ChildEntities.Add(crashFish);
        }

        private static Entity SpawnChild(Entity parentEntity, DeterministicBatchGenerator deterministicBatchGenerator, TechType techType, string classId)
        {
            return new Entity(new NitroxVector3(0, 0, 0), new NitroxQuaternion(0, 0, 0, 1), new NitroxVector3(1, 1, 1), techType.ToDto(), parentEntity.Level, classId, true, deterministicBatchGenerator.NextId(), null, parentEntity);
        }
    }
}
