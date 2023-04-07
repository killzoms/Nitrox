
namespace NitroxModel.DataStructures.GameLogic.Entities
{
    public interface IEntity
    {
        public NitroxId Id { get; }
        public NitroxId ParentId { get; set; }
    }
}
