using System;
using ProtoBufNet;

namespace NitroxModel.DataStructures.Util
{
    [Serializable]
    [ProtoContract]
    public class Optional<T>
    {
        public T Value { 
            get
            {
                if (value == null || value.Equals(default(T)))
                {
                    throw new OptionalEmptyException<T>();
                }
                return value;
            }
            set
            {
                if (value == null || value.Equals(default(T)))
                {
                    throw new ArgumentNullException(nameof(Value), "Value cannot be null");
                }
                this.value = value;
            }
        }

        [ProtoMember(1)]
        private T value;

        private Optional()
        {}

        private Optional(T value)
        {
            Value = value;
        }

        public static Optional<T> Empty()
        {
            return new Optional<T>();
        }

        public static Optional<T> Of(T value)
        {
            if (value == null || value.Equals(default(T)))
            {
                throw new ArgumentNullException(nameof(value), "Value cannot be null");
            }

            return new Optional<T>(value);
        }

        public static Optional<T> OfNullable(T value)
        {
            if (value == null || value.Equals(default(T)))
            {
                return new Optional<T>();
            }

            return new Optional<T>(value);
        }

        public bool IsPresent()
        {
            if (value == null || value.Equals(default(T)))
            {
                return false;
            }
            return true;
        }

        public bool IsEmpty()
        {
            if (value == null || value.Equals(default(T)))
            {
                return true;
            }
            return false;
        }

        public T OrElse(T elseValue)
        {
            if (IsEmpty())
            {
                return elseValue;
            }

            return Value;
        }
    }

    [Serializable]
    public sealed class OptionalEmptyException<T> : Exception
    {
        public OptionalEmptyException() : base($"Optional <{nameof(T)}> is empty.")
        {
        }

        public OptionalEmptyException(string message) : base($"Optional <{nameof(T)}> is empty:\n\t{message}")
        {
        }
    }
}
