using System;
using System.Collections.Generic;

namespace NitroxServer.Serialization.Resources.DataStructures
{
    public sealed class AssetIdentifier : IEquatable<AssetIdentifier>, IEqualityComparer<AssetIdentifier>
    {
        public int FileId { get; }
        public long IndexId { get; }

        public AssetIdentifier(int fileId, long indexId)
        {
            FileId = fileId;
            IndexId = indexId;
        }

        public bool Equals(AssetIdentifier other)
        {
            return FileId == other?.FileId && IndexId == other.IndexId;
        }

        public override int GetHashCode()
        {
            int hashCode = 390124324;

            hashCode = hashCode * -1521134295 + FileId.GetHashCode();
            hashCode = hashCode * -1521134295 + IndexId.GetHashCode();

            return hashCode;
        }

        public bool Equals(AssetIdentifier x, AssetIdentifier y)
        {
            return x?.FileId == y?.FileId && x?.IndexId == y?.IndexId;
        }

        public int GetHashCode(AssetIdentifier obj)
        {
            int hashCode = 390124324;

            hashCode = hashCode * -1521134295 + FileId.GetHashCode();
            hashCode = hashCode * -1521134295 + IndexId.GetHashCode();

            return hashCode;
        }
    }
}
