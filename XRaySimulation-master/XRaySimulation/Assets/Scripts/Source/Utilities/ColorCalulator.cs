using System;
using System.Collections.Generic;
using UnityEngine;

public static class ColorCalculator
{
    public static Color BASE_COLOR = new Color(1f, 1f, 0f);
    private static readonly HSVColor startColor = new HSVColor(BASE_COLOR);
    private static float maxDose = 50f;

    /// <summary>
    /// Calculates a Color32 array based on the given doses.
    /// </summary>
    public static Color32[] Calculate(float[] doses)
    {
        int num = doses.Length;
        Color32[] colors = new Color32[num];

        for (int i = 0; i < num; i++)
        {
            colors[i] = Calculate(doses[i]);
        }

        return colors;
    }

    /// <summary>
    /// Calculates a Color32 based on the given dose.
    /// </summary>
    private static Color32 Calculate(float dose)
    {
        dose = Mathf.Abs(dose);

        float baseH = startColor.h;

        // since HSV Colors have a h value range from 0 to 360
        // and yellow is at 60
        float stepSize = 300f / maxDose;
        float newH = baseH + stepSize * dose / 360f;

        // since unity h values only range from 0 to 1
        newH = Mathf.Clamp01(newH);

        HSVColor hsvColor = startColor.GetNew(newH);

        return (Color32)hsvColor.ToRGB();
    }
}
