using System.Collections.Generic;
using System.Reflection;
using NitroxClient.Communication.Abstract;
using NitroxClient.Communication.Packets.Processors.Abstract;
using NitroxModel.Packets;
using NitroxModel.Subnautica.DataStructures;

namespace NitroxClient.Communication.Packets.Processors
{
    public class PDAScannerEntryRemoveProcessor : ClientPacketProcessor<PDAEntryRemove>
    {
        private readonly FieldInfo pdaScannerPartialField = typeof(PDAScanner).GetField("partial", BindingFlags.NonPublic | BindingFlags.Static);
        private readonly FieldInfo pdaScannerCompleteField = typeof(PDAScanner).GetField("complete", BindingFlags.NonPublic | BindingFlags.Static);

        private readonly IPacketSender packetSender;

        public PDAScannerEntryRemoveProcessor(IPacketSender packetSender)
        {
            this.packetSender = packetSender;
        }

        public override void Process(PDAEntryRemove packet)
        {
            using (packetSender.Suppress<PDAEntryRemove>())
            {

                if (PDAScanner.GetPartialEntryByKey(packet.TechType.ToUnity(), out PDAScanner.Entry entry))
                {
                    List<PDAScanner.Entry> partial = (List<PDAScanner.Entry>)pdaScannerPartialField.GetValue(null);
                    HashSet<TechType> complete = (HashSet<TechType>)pdaScannerCompleteField.GetValue(null);
                    partial.Remove(entry);
                    complete.Add(entry.techType);
                }
            }
        }
    }
}
