﻿using System;
using NUnit.Framework;

namespace AtCoderLibraryCSharp.Tests
{
    public class FenwickTreeTests
    {
        [Test]
        public void EmptyTest()
        {
            var ft = new FenwickTree();
            Assert.That(ft.Sum(0), Is.Zero);
            Assert.That(ft.Sum(0, 0), Is.Zero);
        }

        [Test]
        public void NativeTest([Range(0, 50)] int n)
        {
            var ft = new FenwickTree(n);
            for (var i = 0; i < n; i++) ft.Add(i, i * i);

            for (var i = 0; i <= n; i++)
            {
                var expected = 0L;
                for (var j = 0; j < i; j++) expected += j * j;
                Assert.That(ft.Sum(i), Is.EqualTo(expected));
            }

            for (var l = 0; l <= n; l++)
            {
                for (var r = l; r <= n; r++)
                {
                    var expected = 0L;
                    for (var i = l; i < r; i++) expected += i * i;
                    Assert.That(ft.Sum(l, r), Is.EqualTo(expected));
                }
            }
        }

        [Test]
        public void LowerBoundTest([Range(0, 50)] int n)
        {
            var ft = new FenwickTree(n);
            for (var i = 0; i < n; i++) ft.Add(i, i * i);
            var sum = new int[n];
            for (var i = 1; i < n; i++) sum[i] = sum[i - 1] + i * i;
            for (var i = 0; i < n; i++)
            {
                Assert.That(ft.LowerBound(sum[i]), Is.EqualTo(i));
                Assert.That(ft.LowerBound(sum[i] + 1), Is.EqualTo(i + 1));
                if (i >= 2) Assert.That(ft.LowerBound(sum[i] - 1), Is.EqualTo(i));
            }
        }

        [Test]
        public void UpperBoundTest([Range(0, 50)] int n)
        {
            var ft = new FenwickTree(n);
            for (var i = 0; i < n; i++) ft.Add(i, i * i);
            var sum = new int[n];
            for (var i = 1; i < n; i++) sum[i] = sum[i - 1] + i * i;
            for (var i = 0; i < n; i++)
            {
                Assert.That(ft.UpperBound(sum[i]), Is.EqualTo(i + 1));
                if (i >= 2) Assert.That(ft.UpperBound(sum[i] + 1), Is.EqualTo(i + 1));
                Assert.That(ft.UpperBound(sum[i] - 1), Is.EqualTo(i));
            }
        }

        [Test]
        public void InvalidArgumentsTest()
        {
            Assert.Throws<OverflowException>(() => _ = new FenwickTree(-1));

            var ft = new FenwickTree(10);
            Assert.Throws<IndexOutOfRangeException>(() => ft.Add(-1, 0));
            Assert.Throws<IndexOutOfRangeException>(() => ft.Add(10, 0));

            Assert.Throws<IndexOutOfRangeException>(() => ft.Sum(-1));
            Assert.Throws<IndexOutOfRangeException>(() => ft.Sum(11));
            Assert.Throws<IndexOutOfRangeException>(() => ft.Sum(-1, 3));
            Assert.Throws<IndexOutOfRangeException>(() => ft.Sum(3, 11));
            Assert.Throws<IndexOutOfRangeException>(() => ft.Sum(5, 3));
        }

        [Test]
        public void BoundTest()
        {
            var ft = new FenwickTree(10);
            ft.Add(3, long.MaxValue);
            ft.Add(5, long.MinValue);
            Assert.That(ft.Sum(0, 10), Is.EqualTo(-1));
            Assert.That(ft.Sum(0, 10), Is.EqualTo(-1));
            Assert.That(ft.Sum(3, 4), Is.EqualTo(long.MaxValue));
            Assert.That(ft.Sum(4, 10), Is.EqualTo(long.MinValue));
        }
    }
}