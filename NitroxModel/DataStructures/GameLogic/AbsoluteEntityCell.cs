﻿using System;
using System.Collections.Generic;
using NitroxModel.Helper;
using ProtoBufNet;

namespace NitroxModel.DataStructures.GameLogic
{
    [Serializable]
    [ProtoContract]
    public sealed class AbsoluteEntityCell : IEquatable<AbsoluteEntityCell>, IEqualityComparer<AbsoluteEntityCell>
    {
        [ProtoMember(1)]
        public NitroxInt3 BatchId { get; }

        [ProtoMember(2)]
        public NitroxInt3 CellId { get; }

        [ProtoMember(3)]
        public int Level { get; }

        private NitroxInt3 BatchPosition => BatchId * Map.Main.BatchSize - Map.Main.BatchDimensionCenter;
        public NitroxInt3 Position => BatchPosition + CellId * GetCellSize();

        public NitroxInt3 Center
        {
            get
            {
                NitroxInt3 cellSize = GetCellSize();
                return BatchPosition + CellId * cellSize + (cellSize >> 1);
            }
        }

        protected AbsoluteEntityCell()
        {
            // Constructor for serialization. Has to be "protected" for json serialization.
        }

        public AbsoluteEntityCell(NitroxInt3 batchId, NitroxInt3 cellId, int level)
        {
            BatchId = batchId;
            CellId = cellId;
            Level = level;
        }

        public AbsoluteEntityCell(NitroxVector3 worldSpace, int level)
        {
            Level = level;

            NitroxVector3 localPosition = (worldSpace + Map.Main.BatchDimensionCenter) / Map.Main.BatchSize;
            BatchId = NitroxInt3.Floor(localPosition);

            NitroxVector3 cell = (localPosition - BatchId) * GetCellsPerBlock();
            CellId = NitroxInt3.Floor(new NitroxVector3(cell.X + 0.0001f, cell.Y + 0.0001f, cell.Z + 0.0001f));
        }

        public static bool operator ==(AbsoluteEntityCell left, AbsoluteEntityCell right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(AbsoluteEntityCell left, AbsoluteEntityCell right)
        {
            return !Equals(left, right);
        }

        public static NitroxInt3 GetCellSize(int level, NitroxInt3 blocksPerBatch)
        {
            // Our own implementation for BatchCells.GetCellSize, that works on the server and client.
            return blocksPerBatch / GetCellsPerBlock(level);
        }

        public static int GetCellsPerBlock(int level)
        {
            switch (level)
            {
                case 0:
                    return 10;
                case 1:
                case 2:
                case 3:
                    return 5;
                default:
                    throw new ArgumentOutOfRangeException($"Given level '{level}' does not have any defined cells.");
            }
        }

        public override string ToString()
        {
            return "[AbsoluteEntityCell Position: " + Position + " BatchId: " + BatchId + " CellId: " + CellId + " Level: " + Level + " ]";
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != GetType())
            {
                return false;
            }
            return Equals((AbsoluteEntityCell)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = BatchId != null ? BatchId.GetHashCode() : 0;
                hash = (hash * 397) ^ (CellId != null ? CellId.GetHashCode() : 0);
                hash = (hash * 397) ^ Level;
                return hash;
            }
        }

        public NitroxInt3 GetCellSize()
        {
            return GetCellSize(Map.Main.BatchDimensions);
        }

        public NitroxInt3 GetCellSize(NitroxInt3 blocksPerBatch)
        {
            return GetCellSize(Level, blocksPerBatch);
        }

        public int GetCellsPerBlock()
        {
            return GetCellsPerBlock(Level);
        }

        public bool Equals(AbsoluteEntityCell other)
        {
            return Equals(this, other);
        }

        public bool Equals(AbsoluteEntityCell x, AbsoluteEntityCell y)
        {
            return
            x.BatchId.Equals(y.BatchId) &&
            x.CellId.Equals(y.CellId) &&
            x.Level.Equals(y.Level);
        }

        public int GetHashCode(AbsoluteEntityCell obj)
        {
            unchecked
            {
                int hashCode = 658330915;
                hashCode = hashCode * -1521134295 + EqualityComparer<Int3>.Default.GetHashCode(obj.BatchId);
                hashCode = hashCode * -1521134295 + EqualityComparer<Int3>.Default.GetHashCode(obj.CellId);
                hashCode = hashCode * -1521134295 + obj.Level.GetHashCode();
                return hashCode;
            }
        }

        public override string ToString()
        {
            return $"[AbsoluteEntityCell - Position: {Position} BatchId: {BatchId} CellId: {CellId} Level: {Level} ]";
        }
    }
}
