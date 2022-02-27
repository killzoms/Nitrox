using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NitroxModel.DataStructures.GameLogic;
using NitroxModel.DataStructures;
using NitroxModel.DataStructures.Unity;
using NitroxServer.ConsoleCommands.Abstract;
using NitroxServer.GameLogic.Entities.Spawning;
using NitroxServer.Serialization;

namespace NitroxServer.ConsoleCommands
{
    public class TestInsideUnitSphereCommand : Command
    {
        private DeterministicBatchGenerator deterministicBatchGenerator;

        public TestInsideUnitSphereCommand(ServerConfig config) : base("testinsideunitsphere", Perms.CONSOLE, "Debug")
        {
            this.deterministicBatchGenerator = new DeterministicBatchGenerator(config.Seed, new NitroxInt3());
        }

        protected override void Execute(CallArgs args)
        {
            NitroxVector3[] vector3s = new NitroxVector3[1000000];

            for (int i = 0; i < 1000000; i++)
            {
                deterministicBatchGenerator.NextInsideUnitSphere2();
            }

            Log.Debug("done");
        }
    }
}
