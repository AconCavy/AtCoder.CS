using System;
using NUnit.Framework;

namespace AlgorithmSharp.Tests
{
    public class FenwickTree2DTests
    {
        [Test]
        public void InitializeTest()
        {
            Assert.DoesNotThrow(() => _ = new FenwickTree2D(1, 1));
            Assert.Throws<ArgumentOutOfRangeException>(() => _ = new FenwickTree2D(0, 1));
            Assert.Throws<ArgumentOutOfRangeException>(() => _ = new FenwickTree2D(1, 0));
        }

        [TestCase(-1, 0)]
        [TestCase(0, -1)]
        public void ArgumentOutOfRangeTest(int h, int w)
        {
            var cum = new FenwickTree2D(1, 1);
            Assert.Throws<ArgumentOutOfRangeException>(() => cum.Add(h, w, 1));
            Assert.Throws<ArgumentOutOfRangeException>(() => cum.Sum(h, w));
            Assert.Throws<ArgumentOutOfRangeException>(() => cum.Sum(h, w, 0, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => cum.Sum(0, 0, h, w));
        }

        [Test]
        public void AddTest()
        {
            var cum = new FenwickTree2D(1, 1);
            const int expected = 1;
            cum.Add(0, 0, expected);
            var actual = cum.Sum(0, 0);
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void SumTest()
        {
            const int height = 3;
            const int width = 3;
            var ft = new FenwickTree2D(height, width);
            for (var i = 0; i < height; i++)
                for (var j = 0; j < width; j++)
                    ft.Add(i, j, 1);

            for (var i = 0; i < height; i++)
                for (var j = 0; j < width; j++)
                {
                    var expected = (i + 1) * (j + 1);
                    var actual = ft.Sum(i, j);
                    Assert.That(actual, Is.EqualTo(expected));
                }
        }

        [Test]
        public void Sum2DTest()
        {
            const int height = 3;
            const int width = 3;
            var ft = new FenwickTree2D(height, width);
            for (var i = 0; i < height; i++)
                for (var j = 0; j < width; j++)
                    ft.Add(i, j, 1);

            for (var i = 0; i < height; i++)
                for (var j = 0; j < width; j++)
                    for (var r = 0; r <= i; r++)
                        for (var c = 0; c <= j; c++)
                        {
                            var expected = (i + 1) * (j + 1) + (r + 1) * (c + 1) - (i + 1) * (c + 1) - (r + 1) * (j + 1);
                            var actual = ft.Sum(i, j, r, c);
                            Assert.That(actual, Is.EqualTo(expected));
                        }
        }
    }
}