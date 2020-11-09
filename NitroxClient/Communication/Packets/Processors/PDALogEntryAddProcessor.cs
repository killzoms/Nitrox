using System.Collections.Generic;
using System.Reflection;
using NitroxClient.Communication.Abstract;
using NitroxClient.Communication.Packets.Processors.Abstract;
using NitroxModel.Packets;

namespace NitroxClient.Communication.Packets.Processors
{
    public class PDALogEntryAddProcessor : ClientPacketProcessor<PDALogEntryAdd>
    {
        private readonly FieldInfo pdaLogEntriesField = typeof(PDALog).GetField("entries", BindingFlags.NonPublic | BindingFlags.Static);

        private readonly IPacketSender packetSender;

        public PDALogEntryAddProcessor(IPacketSender packetSender)
        {
            this.packetSender = packetSender;
        }

        public override void Process(PDALogEntryAdd packet)
        {
            using (packetSender.Suppress<PDALogEntryAddProcessor>())
            {
                Dictionary<string, PDALog.Entry> entries = (Dictionary<string, PDALog.Entry>)pdaLogEntriesField.GetValue(null);

                if (!entries.ContainsKey(packet.Key))
                {

                    if (!PDALog.GetEntryData(packet.Key, out PDALog.EntryData entryData))
                    {
                        entryData = new PDALog.EntryData
                        {
                            key = packet.Key,
                            type = PDALog.EntryType.Invalid
                        };
                    }

                    PDALog.Entry entry = new PDALog.Entry
                    {
                        data = entryData,
                        timestamp = packet.Timestamp
                    };
                    entries.Add(entryData.key, entry);
                }
            }
        }
    }
}
