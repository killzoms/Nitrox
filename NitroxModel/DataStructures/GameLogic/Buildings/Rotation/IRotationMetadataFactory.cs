
namespace NitroxModel.DataStructures.GameLogic.Buildings.Rotation
{
    public interface IRotationMetadataFactory
    {
        Optional<RotationMetadata> From(object o);
    }
}
