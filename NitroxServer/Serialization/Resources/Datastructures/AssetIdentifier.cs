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

        public int GetHashCode(AssetIdentifier obj)
        {
            int hashCode = 39012;

            hashCode = hashCode * -1521 + obj.FileId.GetHashCode();
            hashCode = hashCode * -1521 + obj.IndexId.GetHashCode();

            return hashCode;
        }

        public bool Equals(AssetIdentifier other)
        {
            return FileId == other?.FileId && IndexId == other.IndexId;
        }

        public bool Equals(AssetIdentifier x, AssetIdentifier y)
        {
            return x?.FileId == y?.FileId && x?.IndexId == y?.IndexId;
        }
    }
}
