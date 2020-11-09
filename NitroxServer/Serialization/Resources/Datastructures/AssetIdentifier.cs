namespace NitroxServer.Serialization.Resources.DataStructures
{
    public sealed class AssetIdentifier
    {
        public int FileId { get; }
        public long IndexId { get; }

        public AssetIdentifier(int fileId, long indexId)
        {
            FileId = fileId;
            IndexId = indexId;
        }

        public override bool Equals(object obj)
        {
            AssetIdentifier other = obj as AssetIdentifier;

            return FileId == other?.FileId && IndexId == other.IndexId;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = 390124324;
                hashCode = hashCode * -1521134295 + FileId.GetHashCode();
                hashCode = hashCode * -1521134295 + IndexId.GetHashCode();
                return hashCode;
            }
        }
    }
}
