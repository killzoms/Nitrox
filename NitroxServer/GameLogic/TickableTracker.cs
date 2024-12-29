using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroxServer.GameLogic
{
    public static class TickableTracker
    {
        private static int lastId = 0;
        private static Dictionary<int, ITickable> tickables = new Dictionary<int, ITickable>();

        public static void RegisterTickable(ITickable tickable)
        {
            tickable.TickId = lastId++;
            tickables[tickable.TickId] = tickable;
        }

        public static void TickAll()
        {
            for (int i = 0; i < tickables.Count; i++)
            {
                tickables[i].Tick();
            }
        }
    }
}
