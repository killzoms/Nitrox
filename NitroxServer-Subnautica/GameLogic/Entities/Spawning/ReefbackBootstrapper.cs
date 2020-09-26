using NitroxModel.DataStructures;
using NitroxModel.DataStructures.GameLogic;
using NitroxModel_Subnautica.DataStructures;
using NitroxServer.GameLogic.Entities.Spawning;
using static NitroxServer_Subnautica.GameLogic.Entities.Spawning.EntityBootstrappers.ReefbackSpawnData;

namespace NitroxServer_Subnautica.GameLogic.Entities.Spawning.EntityBootstrappers
{
    public class ReefbackBootstrapper : IEntityBootstrapper
    {
        private readonly float creatureProbabilitySum;

        public ReefbackBootstrapper()
        {
            foreach (ReefbackEntity creature in SpawnableCreatures)
            {
                creatureProbabilitySum += creature.Probability;
            }
        }

        public void Prepare(Entity parentEntity, DeterministicBatchGenerator deterministicBatchGenerator)
        {
            foreach (NitroxVector3 localSpawnPosition in LocalCreatureSpawnPoints)
            {
                float targetProbabilitySum = (float)deterministicBatchGenerator.NextDouble() * creatureProbabilitySum;
                float probabilitySum = 0;

                foreach (ReefbackEntity creature in SpawnableCreatures)
                {
                    probabilitySum += creature.Probability;

                    if (probabilitySum >= targetProbabilitySum)
                    {
                        int totalToSpawn = deterministicBatchGenerator.NextInt(creature.MinNumber, creature.MaxNumber + 1);

                        for (int i = 0; i < totalToSpawn; i++)
                        {
                            NitroxId id = deterministicBatchGenerator.NextId();
                            Entity child = new Entity(localSpawnPosition, new NitroxQuaternion(0, 0, 0, 1), new NitroxVector3(1, 1, 1), creature.TechType.ToDto(), parentEntity.Level, creature.ClassId, true, id, null, parentEntity);
                            parentEntity.ChildEntities.Add(child);
                        }

                        break;
                    }
                }
            }
        }
    }
}
