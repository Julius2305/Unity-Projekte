using System;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;

public class HSVColorTest
{
    [Test]
    public void HSVConstructorWithRGBColor_Test()
    {
        // setup
        float r = 1f;
        float g = 0.4f;
        float b = 0.2f;
        Color rgbColor = new Color(r, g, b);

        HSVColor hsvColor = new HSVColor(rgbColor);

        float h = 1f;
        float s = 1f;
        float v = 1f;

        // perform
        Color.RGBToHSV(rgbColor, out h, out s, out v);

        // assert
        Assert.AreEqual(h, hsvColor.h);
        Assert.AreEqual(s, hsvColor.s);
        Assert.AreEqual(v, hsvColor.v);
    }

    [Test]
    [TestCase(0f)]
    [TestCase(0.1f)]
    [TestCase(0.5f)]
    [TestCase(0.9f)]
    //No need to test other values, because Unity's h values are only defined from [0, 1)
    public void GetNew_Test(float value)
    {
        // setup
        float expected = value;
        HSVColor hsvColor = new HSVColor(Color.blue);

        // perform
        hsvColor = hsvColor.GetNew(value);

        // assert
        Assert.AreEqual(value, (float) Math.Round(hsvColor.h, 1));
    }

    private readonly Color[] colors = new Color[]
    {
        Color.red,
        Color.blue,
        Color.yellow,
        Color.green
    };

    [Test]
    public void ToRGB_Test()
    {
        // setup
        Color expected = Color.white;
        Color rgbColor = Color.black;

        // perform
        for (int i = 0; i < colors.Length; i++)
        {
            expected = colors[i];
            rgbColor = new HSVColor(colors[i]).ToRGB();
        }

        // assert
        Assert.AreEqual(expected, rgbColor);
    }

    [Test]
    [TestCase(0f, 1f, 0f, 0f)]
    [TestCase(1f, 1f, 0f, 0f)]
    [TestCase(0.5f, 0f, 1f, 1f)]
    [TestCase(0.25f, 0.5f, 1f, 0f)]
    [TestCase(0.666666667f, 0f, 0f, 1f)]
    [TestCase(0.333333334f, 0f, 1f, 0f)]
    public void ToRGBAfterGetNew_Test(float h, float r, float g, float b)
    {
        // setup
        Color expected = new Color(r, g, b);
        HSVColor hsvColor = new HSVColor(Color.red);

        // perform
        hsvColor = hsvColor.GetNew(h);
        Color actual = hsvColor.ToRGB();

        // assert
        Assert.AreEqual(expected, actual);
    }
}