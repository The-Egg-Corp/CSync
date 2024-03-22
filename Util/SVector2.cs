using System;
using UnityEngine;

namespace CSync.Util;

[Serializable]
public class SVector2(float _x, float _y) {
    public float x = _x; 
    public float y = _y;

    public override string ToString() => string.Format("[{0}, {1}]", x, y);

    public static implicit operator SVector2(SVector3 sv2) => new(sv2.x, sv2.y);
    public static implicit operator SVector2(Vector3 v2) => new(v2.x, v2.y);
}