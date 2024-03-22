using System;
using UnityEngine;

namespace CSync.Util;

[Serializable]
public class SColor {
    public float r, g, b, a;

    public static implicit operator Color(SColor col) => new(col.r, col.g, col.b, col.a);

    public SColor(Color color) {
        SetRGBA(color);
    }

    public SColor(string hex) {
        ColorUtility.TryParseHtmlString(hex, out Color col);
        SetRGBA(col);
    }

    void SetRGBA(Color color) {
        r = color.r;
        g = color.g;
        b = color.b;
        a = color.a;
    }

    public Color AsColor() => new(r, g, b, a);
    public string AsHex() => ColorUtility.ToHtmlStringRGBA(AsColor());
}