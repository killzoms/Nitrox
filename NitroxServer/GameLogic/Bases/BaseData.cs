using System.Collections.Generic;
using NitroxModel.DataStructures.GameLogic;
using ProtoBufNet;

namespace NitroxServer.GameLogic.Bases
{
    [ProtoContract]
    public class BaseData
    {
        public const short VERSION = 2;

        [ProtoMember(1)]
        public List<BasePiece> PartiallyConstructedPieces { get; private set; } = new List<BasePiece>();

        [ProtoMember(2)]
        public List<BasePiece> CompletedBasePieceHistory { get; private set; } = new List<BasePiece>();

        public static BaseData From(List<BasePiece> partiallyConstructedPieces, List<BasePiece> completedBasePieceHistory)
        {
            BaseData baseData = new BaseData
            {
                PartiallyConstructedPieces = partiallyConstructedPieces,
                CompletedBasePieceHistory = completedBasePieceHistory
            };

            return baseData;
        }
    }
}
