using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using NitroxModel.DataStructures.Unity;
using NitroxModel.Helper;

namespace NitroxModel.DataStructures
{
    /// <summary>
    ///     Model to allow <see cref="NitroxModel"/> to be decoupled from Assembly-csharp-firstpass (i.e. game code).
    /// </summary>
    [Serializable]
    [DataContract]
    public struct NitroxInt3 : IEquatable<NitroxInt3>
    {
        [DataMember(Order = 1)]
        public int X { get; set; }

        [DataMember(Order = 2)]
        public int Y { get; set; }

        [DataMember(Order = 3)]
        public int Z { get; set; }

        public NitroxInt3(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public override string ToString()
        {
            return $"[NitroxInt3 - {X}, {Y}, {Z}]";
        }

        public bool Equals(NitroxInt3 other)
        {
            return X == other.X
                && Y == other.Y
                && Z == other.Z;
        }

        public override bool Equals(object obj)
        {
            return (obj is NitroxInt3 other) && Equals(other);
        }

        public override int GetHashCode()
        {
            int hashCode = -307843816;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            hashCode = hashCode * -1521134295 + Z.GetHashCode();

            return hashCode;
        }

        public static NitroxInt3 Clamp(NitroxInt3 a, NitroxInt3 min, NitroxInt3 max)
        {
            return new NitroxInt3(Mathf.Clamp(a.X, min.X, max.X), Mathf.Clamp(a.Y, min.Y, max.Y), Mathf.Clamp(a.Z, min.Z, max.Z));
        }

        public NitroxInt3 Clamp(NitroxInt3 mins, NitroxInt3 maxs)
        {
            return Min(maxs, Max(mins, this));
        }

        public static NitroxInt3 Min(NitroxInt3 a, NitroxInt3 b)
        {
            return new NitroxInt3(Mathf.Min(a.X, b.X), Mathf.Min(a.Y, b.Y), Mathf.Min(a.Z, b.Z));
        }

        public static NitroxInt3 Max(NitroxInt3 a, NitroxInt3 b)
        {
            return new NitroxInt3(Mathf.Max(a.X, b.X), Mathf.Max(a.Y, b.Y), Mathf.Max(a.Z, b.Z));
        }

        public static NitroxInt3 Floor(float x, float y, float z)
        {
            return new NitroxInt3(Convert.ToInt32(Math.Floor(x)),
                            Convert.ToInt32(Math.Floor(y)),
                            Convert.ToInt32(Math.Floor(z)));
        }

        public static NitroxInt3 Floor(NitroxVector3 vector)
        {
            return Floor(vector.X, vector.Y, vector.Z);
        }

        public static NitroxInt3 FloorDiv(NitroxInt3 int3, int div)
        {
            return new NitroxInt3(NitroxMath.FloorDiv(int3.X, div), NitroxMath.FloorDiv(int3.Y, div), NitroxMath.FloorDiv(int3.Z, div));
        }

        public static NitroxInt3 FloorDiv(NitroxInt3 int3, NitroxInt3 div)
        {
            return new NitroxInt3(NitroxMath.FloorDiv(int3.X, div.X), NitroxMath.FloorDiv(int3.Y, div.Y), NitroxMath.FloorDiv(int3.Z, div.Z));
        }

        public static NitroxInt3 Ceil(float x, float y, float z)
        {
            return new NitroxInt3(Convert.ToInt32(Math.Ceiling(x)),
                            Convert.ToInt32(Math.Ceiling(y)),
                            Convert.ToInt32(Math.Ceiling(z)));
        }

        public static NitroxInt3 Ceil(NitroxVector3 vector)
        {
            return Ceil(vector.X, vector.Y, vector.Z);
        }

        public static NitroxInt3 CeilDiv(NitroxInt3 a, NitroxInt3 b)
        {
            return new NitroxInt3(NitroxMath.CeilDiv(a.X, b.X), NitroxMath.CeilDiv(a.Y, b.Y), NitroxMath.CeilDiv(a.Z, b.Z));
        }

        public static NitroxInt3 PositiveModulo(NitroxInt3 a, NitroxInt3 b)
        {
            return new NitroxInt3(NitroxMath.PositiveModulo(a.X, b.X), NitroxMath.PositiveModulo(a.Y, b.Y), NitroxMath.PositiveModulo(a.Z, b.Z));
        }

        public static Bounds CenterSize(NitroxInt3 center, NitroxInt3 size)
        {
            NitroxInt3 mins = center - size / 2;
            NitroxInt3 maxs = mins + size - 1;

            return new Bounds(mins, maxs);
        }

        public static RangeEnumerator Range(NitroxInt3 upperBound)
        {
            return new RangeEnumerator(new NitroxInt3(0, 0, 0), upperBound - 1);
        }

        public void Next(NitroxInt3 mins, NitroxInt3 maxs)
        {
            Z++;
            if (Z > maxs.Z)
            {
                Y++;
                if (Y > maxs.Y)
                {
                    X++;
                    Y = mins.Y;
                }
                Z = mins.Z;
            }
        }

        public static bool operator ==(NitroxInt3 u, NitroxInt3 v)
        {
            return u.Equals(v);
        }

        public static bool operator !=(NitroxInt3 u, NitroxInt3 v)
        {
            return !u.Equals(v);
        }

        public static NitroxInt3 operator <<(NitroxInt3 u, int s)
        {
            return new NitroxInt3(u.X << s, u.Y << s, u.Z << s);
        }

        public static NitroxInt3 operator >>(NitroxInt3 u, int s)
        {
            return new NitroxInt3(u.X >> s, u.Y >> s, u.Z >> s);
        }

        public static bool operator <(NitroxInt3 u, NitroxInt3 v)
        {
            return u.X < v.X && u.Y < v.Y && u.Z < v.Z;
        }

        public static bool operator >(NitroxInt3 u, NitroxInt3 v)
        {
            return u.X > v.X && u.Y > v.Y && u.Z > v.Z;
        }

        public static bool operator <=(NitroxInt3 u, NitroxInt3 v)
        {
            return u.X <= v.X && u.Y <= v.Y && u.Z <= v.Z;
        }

        public static bool operator >=(NitroxInt3 u, NitroxInt3 v)
        {
            return u.X >= v.X && u.Y >= v.Y && u.Z >= v.Z;
        }

        public static NitroxInt3 operator +(NitroxInt3 u, NitroxInt3 v)
        {
            return new NitroxInt3(u.X + v.X, u.Y + v.Y, u.Z + v.Z);
        }

        public static NitroxInt3 operator +(NitroxInt3 u, int s)
        {
            return new NitroxInt3(u.X + s, u.Y + s, u.Z + s);
        }

        public static NitroxVector3 operator +(NitroxInt3 u, NitroxVector3 v)
        {
            return new NitroxVector3(u.X + v.X, u.Y + v.Y, u.Z + v.Z);
        }

        public static implicit operator NitroxVector3(NitroxInt3 v)
        {
            return new NitroxVector3(v.X, v.Y, v.Z);
        }

        public static explicit operator NitroxInt3(NitroxVector3 v)
        {
            return new NitroxInt3((int)v.X, (int)v.Y, (int)v.Z);
        }

        public static NitroxInt3 operator -(NitroxInt3 u, NitroxInt3 v)
        {
            return new NitroxInt3(u.X - v.X, u.Y - v.Y, u.Z - v.Z);
        }

        public static NitroxInt3 operator -(NitroxInt3 u, int s)
        {
            return new NitroxInt3(u.X - s, u.Y - s, u.Z - s);
        }

        public static NitroxInt3 operator *(NitroxInt3 u, NitroxInt3 v)
        {
            return new NitroxInt3(u.X * v.X, u.Y * v.Y, u.Z * v.Z);
        }

        public static NitroxInt3 operator *(NitroxInt3 u, int s)
        {
            return new NitroxInt3(u.X * s, u.Y * s, u.Z * s);
        }

        public static NitroxInt3 operator /(NitroxInt3 u, NitroxInt3 v)
        {
            return new NitroxInt3(u.X / v.X, u.Y / v.Y, u.Z / v.Z);
        }

        public static NitroxInt3 operator /(NitroxInt3 u, int s)
        {
            return new NitroxInt3(u.X / s, u.Y / s, u.Z / s);
        }

        public struct Bounds
        {
            public NitroxInt3 Mins;
            public NitroxInt3 Maxs;


            public NitroxVector3 Center => (((NitroxVector3)Mins) + (NitroxVector3)(Maxs + 1)) * 0.5f;
            public NitroxInt3 Size => Maxs - Mins + 1;

            public Bounds(NitroxInt3 mins, NitroxInt3 maxs)
            {
                Mins = mins;
                Maxs = maxs;
            }

            public NitroxInt3 Clamp(NitroxInt3 p)
            {
                return NitroxInt3.Clamp(p, Mins, Maxs);
            }

            public Bounds Clamp(NitroxInt3 cmins, NitroxInt3 cmaxs)
            {
                return new Bounds(Mins.Clamp(cmins, cmaxs), Maxs.Clamp(cmins, cmaxs));
            }

            public static Bounds FinerBounds(NitroxInt3 coarseCell, int finePerCoarseCell)
            {
                return FinerBounds(new Bounds(coarseCell, coarseCell), new NitroxInt3(finePerCoarseCell, finePerCoarseCell, finePerCoarseCell));
            }

            public static Bounds FinerBounds(Bounds coarseBounds, NitroxInt3 finePerCoarseCell)
            {
                return new Bounds(coarseBounds.Mins * finePerCoarseCell, (coarseBounds.Maxs + 1) * finePerCoarseCell - 1);
            }

            public static Bounds OuterCoarserBounds(Bounds fineBounds, NitroxInt3 finePerCoarseCell)
            {
                return new Bounds(FloorDiv(fineBounds.Mins, finePerCoarseCell), CeilDiv(fineBounds.Maxs + 1, finePerCoarseCell) - 1);
            }

            public RangeEnumerator GetEnumerator()
            {
                return GetRangeEnumerator();
            }

            public RangeEnumerator GetRangeEnumerator()
            {
                return new RangeEnumerator(Mins, Maxs);
            }


            public static Bounds operator *(Bounds b, int s)
            {
                return new Bounds(b.Mins * s, b.Maxs * s);
            }

            public static Bounds operator *(Bounds b, NitroxInt3 s)
            {
                return new Bounds(b.Mins * s, b.Maxs * s);
            }

            public static Bounds operator /(Bounds b, int s)
            {
                return new Bounds(b.Mins / s, b.Maxs / s);
            }

            public static Bounds operator /(Bounds b, NitroxInt3 s)
            {
                return new Bounds(b.Mins / s, b.Maxs / s);
            }

            public static Bounds operator +(Bounds b, NitroxInt3 s)
            {
                return new Bounds(b.Mins + s, b.Maxs + s);
            }

            public static Bounds operator -(Bounds b, NitroxInt3 s)
            {
                return new Bounds(b.Mins - s, b.Maxs - s);
            }

            public static Bounds operator <<(Bounds b, int s)
            {
                return new Bounds(b.Mins << s, b.Maxs << s);
            }

            public static Bounds operator >>(Bounds b, int s)
            {
                return new Bounds(b.Mins >> s, b.Maxs >> s);
            }
        }

        public struct RangeEnumerator : IEnumerator<NitroxInt3>, IEnumerator, IDisposable
        {
            private NitroxInt3 mins;
            private NitroxInt3 maxs;
            private NitroxInt3 current;

            public NitroxInt3 Current => current;

            object IEnumerator.Current => current;

            public RangeEnumerator(NitroxInt3 mins, NitroxInt3 maxs)
            {
                this.mins = mins;
                this.maxs = maxs;
                current = mins;
                Reset();
            }

            public void Dispose()
            {}

            public bool MoveNext()
            {
                current.Next(mins, maxs);
                return current <= maxs;
            }

            public bool MoveNext(int step)
            {
                for (int i = 0; i < step; i++)
                {
                    current.Next(mins, maxs);
                }
                return current <= maxs;
            }

            public void Reset()
            {
                current = new NitroxInt3(mins.X, mins.Y, mins.Z - 1);
            }

            public RangeEnumerator GetEnumerator()
            {
                return this;
            }
        }
    }
}
