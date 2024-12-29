using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroxServer.GameLogic
{
    public interface ITickable
    {
        int TickId { get; set; }
        void Tick();
    }
}
