using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System;

[TestFixture]
public class TestDiamondSquare
{
        
    [Test]
    public void Width_WhenValueIsAPowerOfTwoPlusOne()
    {
        int width = 257;
        DiamondSquare diamondSquare = new DiamondSquare(width);
    }

    [Test]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void Width_WhenValueIsNotPowerOfTwoPlusOne()
    {
        int width = 100;
        DiamondSquare diamondSquare = new DiamondSquare(width);
    }

    [Test]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void Width_WhenValueIsZero()
    {
        int width = 0;
        DiamondSquare diamondSquare = new DiamondSquare(width);
    }

    [Test]
    public void Array_ExpectedValues()
    {
        /*Test method for expected array values, using non-randomized numbers
            Expected values are
            1    2.5   1 
            2.5  2     2.5
            1    2.5   1
        */
        float[] expectedArray = new float[] {1, 2.5f, 1,
                                             2.5f, 2, 2.5f,
                                             1, 2.5f, 1 };
        int width = 3;
        int size = width * width;
        DiamondSquare diamondSquare = new DiamondSquare(width);
        diamondSquare.CreateFractal(1, 1, false);
        float[] actualArray = diamondSquare.GetFractal();

        for (int i = 0; i < size; i++)
        {
            Assert.AreEqual(expectedArray[i], actualArray[i], "Index " + i);
        }
    }
}
