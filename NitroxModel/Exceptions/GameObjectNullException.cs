using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace NitroxModel.Exceptions
{
    [Serializable]
    public sealed class GameObjectNullException : Exception
    {
        public GameObjectNullException() : base("GameObject is null.")
        {
        }

        public GameObjectNullException(string message) : base($"GameObject is null:\n\t{message}")
        {
        }

        private GameObjectNullException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}
