using System;
using UnityEngine;

namespace CSync.Util.Types;

[Serializable]
public class SVector3(float _x, float _y, float _z) {
    public float x = _x;
    public float y = _y;
    public float z = _z;

    public override string ToString() => string.Format("[{0}, {1}, {2}]", x, y, z);

    public static implicit operator Vector3(SVector3 sv3) => new(sv3.x, sv3.y, sv3.z);
    public static implicit operator SVector3(Vector3 v3) => new(v3.x, v3.y, v3.z);
}