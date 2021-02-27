using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HSVColor
{
    public float h;
    public float s;
    public float v;

    public HSVColor(Color col)
    {
        Color.RGBToHSV(col, out this.h, out this.s, out this.v);
    }

    /// <summary>
    /// Returns a HSVColor object based on the old one, but with new h value.
    /// </summary>
    public HSVColor GetNew(float newH)
    {
        Color rgbColor = Color.HSVToRGB(newH, this.s, this.v);
        return new HSVColor(rgbColor);
    }

    /// <summary>
    /// Returns the Unity RGB color for this HSVColor object. 
    /// </summary>
    public Color ToRGB()
    {
        return Color.HSVToRGB(this.h, this.s, this.v);
    }
}
