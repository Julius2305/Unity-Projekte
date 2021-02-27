using System;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using UnityEngine;

public class DoseCalculatorTest
{
    [Test]
    [TestCase(new float[] { }, new float[] { })]
    [TestCase(new float[] { 1, 2f, 2.1f, 0f, 150f, float.NaN, -1f, -150f}, new float[] { 1.333f, 0.333f, 0.302f, float.PositiveInfinity, 0f, 0f, 1.333f, 0f})]
    public void CalculateReturnsCorrectDoses_Test(float[] distances, float[] expected)
    {
        // perform
        float[] doses = DoseCalculator.Calculate(distances, 100f, 1f);

        // for easy readable numbers and corrections
        for (int i = 0; i < doses.Length; i++)
        {
            doses[i] = (float)Math.Round(doses[i], 3);
        }

        // assert
        Assert.AreEqual(expected, doses);
    }

    [Test]
    [TestCase(new float[] { 1f, 2f, 3f, 4f, 5f, 6f, 7f, 8f, 9f, 10f}, 5.5f)]
    [TestCase(new float[] { 1f, 2f, 1f, 1f, 1.5f, 2f}, 1.417f)]
    public void GetAVGDoseReturnsCorrectDose_Test(float[] doses, float expected)
    {
        // perform
        float actual = DoseCalculator.GetAVGDose(doses);

        // assert
        Assert.AreEqual(expected, actual);
    }
}