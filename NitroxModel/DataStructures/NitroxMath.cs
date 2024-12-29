using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using NitroxModel.DataStructures.Unity;
using NitroxModel.Helper;

namespace NitroxModel.DataStructures;
public class NitroxMath
{
    public static int FloorDiv(int a, int b)
    {
        return (a - (a % b + b) % b) / b;
    }

    public static int CeilDiv(int a, int b)
    {
        return (a - (a % b - b) % b) / b;
    }

    public static int PositiveModulo(int v, int m)
    {
        return (v % m + m) % m;
    }

    public static int CeilShiftRight(int x, int shift)
    {
        return x + (1 << shift) - 1 >> shift;
    }

    public static float GetPointToBoxDistanceSquared(NitroxVector3 point, NitroxVector3 min, NitroxVector3 max)
    {
        return (new NitroxVector3(Mathf.Clamp(point.X, min.X, max.X), Mathf.Clamp(point.Y, min.Y, max.Y), Mathf.Clamp(point.Z, min.Z, max.Z)) - point).SqrMagnitude;
    }

}
