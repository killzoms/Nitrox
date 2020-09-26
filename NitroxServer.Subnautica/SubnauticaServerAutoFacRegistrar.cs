using System.Collections.Generic;
using Autofac;
using NitroxModel.DataStructures.GameLogic;
using NitroxModel.DataStructures.GameLogic.Entities;
using NitroxModel.Subnautica.DataStructures;
using NitroxModel.Subnautica.DataStructures.GameLogic.Entities;
using NitroxServer.GameLogic.Entities.Spawning;
using NitroxServer.Serialization;
using NitroxServer.Subnautica.GameLogic.Entities;
using NitroxServer.Subnautica.GameLogic.Entities.Spawning;
using NitroxServer.Subnautica.Serialization;
using NitroxServer.Subnautica.Serialization.Resources;

namespace NitroxServer.Subnautica
{
    public class SubnauticaServerAutoFacRegistrar : ServerAutoFacRegistrar
    {
        public override void RegisterDependencies(ContainerBuilder containerBuilder)
        {
            base.RegisterDependencies(containerBuilder);

            containerBuilder.Register(c => SimulationWhitelist.ForServerSpawned).SingleInstance();
            containerBuilder.Register(c => new SubnauticaServerProtoBufSerializer(
                                          "Assembly-CSharp",
                                          "Assembly-CSharp-firstpass",
                                          "NitroxModel",
                                          "NitroxModel.Subnautica"))
                            .As<ServerProtoBufSerializer, IServerSerializer>()
                            .SingleInstance();
            containerBuilder.Register(c => new SubnauticaServerJsonSerializer())
                            .As<ServerJsonSerializer, IServerSerializer>()
                            .SingleInstance();

            containerBuilder.RegisterType<SubnauticaEntitySpawnPointFactory>().As<EntitySpawnPointFactory>().SingleInstance();

            ResourceAssets resourceAssets = ResourceAssetsParser.Parse();

            containerBuilder.Register(c => resourceAssets).SingleInstance();
            containerBuilder.Register(c => resourceAssets.WorldEntitiesByClassId).SingleInstance();
            containerBuilder.Register(c => resourceAssets.PrefabPlaceholderGroupsByGroupClassId).SingleInstance();
            containerBuilder.RegisterType<SubnauticaUweWorldEntityFactory>().As<UweWorldEntityFactory>().SingleInstance();

            SubnauticaUwePrefabFactory prefabFactory = new SubnauticaUwePrefabFactory(resourceAssets.LootDistributionsJson);
            containerBuilder.Register(c => prefabFactory).As<UwePrefabFactory>().SingleInstance();
            containerBuilder.Register(c => new Dictionary<NitroxTechType, IEntityBootstrapper>
            {
                [TechType.CrashHome.ToDto()] = new CrashFishBootstrapper(),
                [TechType.Reefback.ToDto()] = new ReefbackBootstrapper()
            }).SingleInstance();
        }
    }
}
