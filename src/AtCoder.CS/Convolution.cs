﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace AtCoder.CS
{
    public static class Convolution
    {
        private static MInt[] _sumE;
        private static MInt[] _sumIE;
        private static int _primitiveRoot;

        public static IEnumerable<MInt> Execute(IEnumerable<MInt> a, IEnumerable<MInt> b)
        {
            var (a1, b1) = (a.ToArray(), b.ToArray());
            var (n, m) = (a1.Length, b1.Length);
            var ret = new MInt[n + m - 1];
            if (System.Math.Min(n, m) <= 60)
            {
                for (var i = 0; i < n; i++)
                {
                    for (var j = 0; j < m; j++)
                    {
                        ret[i + j] += a1[i] * b1[j];
                    }
                }

                return ret;
            }

            var z = 1 << CeilPower2(n + m - 1);
            Array.Resize(ref a1, z);
            Array.Resize(ref b1, z);
            a1 = Butterfly(a1).ToArray();
            b1 = Butterfly(b1).ToArray();
            for (var i = 0; i < a1.Length; i++) a1[i] *= b1[i];
            ret = ButterflyInverse(a1).ToArray();
            Array.Resize(ref ret, n + m - 1);
            return ret.Select(x => x / z);
        }

        private static void Initialize()
        {
            if (_sumE != null && _sumIE != null) return;
            var m = MInt.Mod;
            _primitiveRoot = PrimitiveRoot(m);
            _sumE = new MInt[30];
            _sumIE = new MInt[30];
            var es = new MInt[30];
            var ies = new MInt[30];
            var count2 = BitScanForward(m - 1);
            var e = new MInt(_primitiveRoot).Power((m - 1) >> count2);
            var ie = e.Inverse();
            for (var i = count2; i >= 2; i--)
            {
                es[i - 2] = e;
                ies[i - 2] = ie;
                e *= e;
                ie *= ie;
            }

            MInt now = 1;
            MInt inow = 1;
            for (var i = 0; i < count2 - 2; i++)
            {
                _sumE[i] = es[i] * now;
                _sumIE[i] = ies[i] * inow;
                now *= ies[i];
                inow *= es[i];
            }
        }

        private static IEnumerable<MInt> Butterfly(IEnumerable<MInt> items)
        {
            var ret = items.ToArray();
            var h = CeilPower2(ret.Length);
            if (_sumE == null) Initialize();

            for (var ph = 1; ph <= h; ph++)
            {
                var w = 1 << (ph - 1);
                var p = 1 << (h - ph);
                MInt now = 1;
                for (var s = 0; s < w; s++)
                {
                    var offset = s << (h - ph + 1);
                    for (var i = 0; i < p; i++)
                    {
                        var l = ret[i + offset];
                        var r = ret[i + offset + p] * now;
                        ret[i + offset] = l + r;
                        ret[i + offset + p] = l - r;
                    }

                    now *= _sumE[BitScanForward(~s)];
                }
            }

            return ret;
        }

        private static IEnumerable<MInt> ButterflyInverse(IEnumerable<MInt> items)
        {
            var ret = items.ToArray();
            var h = CeilPower2(ret.Length);
            if (_sumIE == null) Initialize();

            for (var ph = h; ph >= 1; ph--)
            {
                var w = 1 << (ph - 1);
                var p = 1 << (h - ph);
                MInt inow = 1;
                for (var s = 0; s < w; s++)
                {
                    var offset = s << (h - ph + 1);
                    for (var i = 0; i < p; i++)
                    {
                        var l = ret[i + offset];
                        var r = ret[i + offset + p];
                        ret[i + offset] = l + r;
                        ret[i + offset + p] = (l - r) * inow;
                    }

                    inow *= _sumIE[BitScanForward(~s)];
                }
            }

            return ret;
        }

        private static int BitScanForward(long n)
        {
            if (n == 0) return 0;
            var x = 0;
            while ((n >> x & 1) == 0) x++;
            return x;
        }

        private static int CeilPower2(int n)
        {
            var x = 0;
            while ((1 << x) < n) x++;
            return x;
        }

        private static long Power(long x, long n, long m)
        {
            if (n < 0) throw new ArgumentException(nameof(n));
            if (m < 1) throw new ArgumentException(nameof(m));
            if (m == 1) return 0;
            uint r = 1;
            var y = (uint) (x % m >= 0 ? x % m : (x + m) % m);
            var um = (uint) m;
            while (n > 1)
            {
                if (n % 1 == 1) r = r * y % um;
                y = y * y % um;
                n >>= 1;
            }

            return r;
        }

        private static int PrimitiveRoot(long m)
        {
            if (_primitiveRoot != 0) return _primitiveRoot;
            switch (m)
            {
                case 2:
                    return _primitiveRoot = 1;
                case 167772161:
                case 469762049:
                case 998244353:
                    return _primitiveRoot = 3;
                case 754974721:
                    return _primitiveRoot = 11;
            }

            var divs = new long[20];
            divs[0] = 2;
            var count = 1;
            var x = (m - 1) / 2;
            while (x % 2 == 0) x /= 2;
            for (var i = 3; (long) i * i < x; i += 2)
            {
                if (x % i != 0) continue;
                divs[count++] = i;
                while (x % i == 0) x /= i;
            }

            if (x > 1) divs[count++] = x;
            for (var g = 2;; g++)
            {
                var ok = true;
                for (var i = 0; i < count && ok; i++)
                {
                    if (Power(g, (m - 1) / divs[i], m) == 1) ok = false;
                }

                if (ok) return _primitiveRoot = g;
            }
        }
    }
}