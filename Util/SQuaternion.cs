using System;
using UnityEngine;

namespace CSync.Util;

[Serializable]
public class SQuaternion(float _x, float _y, float _z, float _w) {
    public float x = _x;
    public float y = _y;
    public float z = _z;
    public float w = _w;

    public override string ToString() => string.Format("[{0}, {1}, {2}, {3}]", x, y, z, w);

    public static implicit operator Quaternion(SQuaternion sq) => new(sq.x, sq.y, sq.z, sq.w);
    public static implicit operator SQuaternion(Quaternion q) => new(q.x, q.y, q.z, q.w);
}