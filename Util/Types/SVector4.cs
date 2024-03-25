using System;
using UnityEngine;

namespace CSync.Util.Types;

[Serializable]
public class SVector4(float _x, float _y, float _z, float _w) {
    public float x = _x;
    public float y = _y;
    public float z = _z;
    public float w = _w;

    public override string ToString() => string.Format("[{0}, {1}, {2}, {3}]", x, y, z, w);

    public static implicit operator Vector4(SVector4 sv4) => new(sv4.x, sv4.y, sv4.z, sv4.w);
    public static implicit operator SVector4(Vector4 v4) => new(v4.x, v4.y, v4.z, v4.w);
}