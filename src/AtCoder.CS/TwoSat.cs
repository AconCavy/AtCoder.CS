﻿using System;
using System.Linq;

namespace AtCoder.CS
{
    /// <summary>
    /// Reference:
    /// B. Aspvall, M. Plass, and R. Tarjan,
    /// A Linear-Time Algorithm for Testing the Truth of Certain Quantified Boolean Formulas
    /// </summary>
    public class TwoSat
    {
        public bool[] Answer => _answer;
        private readonly int _n;
        private readonly bool[] _answer;
        private readonly SCCGraph _scc;

        public TwoSat(int n = 0)
        {
            _n = n;
            _answer = new bool[n];
            _scc = new SCCGraph(2 * n);
        }

        public void AddClause(int i, bool f, int j, bool g)
        {
            if (i < 0 || _n < i) throw new ArgumentException(nameof(i));
            if (j < 0 || _n < j) throw new ArgumentException(nameof(j));
            _scc.AddEdge(2 * i + (f ? 0 : 1), 2 * j + (g ? 1 : 0));
            _scc.AddEdge(2 * j + (g ? 0 : 1), 2 * i + (f ? 1 : 0));
        }

        public bool IsSatisfiable()
        {
            var (_, tmp) = _scc.Ids();
            var ids = tmp.ToArray();
            for (var i = 0; i < _n; i++)
            {
                if (ids[2 * i] == ids[2 * i + 1]) return false;
                _answer[i] = ids[2 * i] < ids[2 * i + 1];
            }

            return true;
        }
    }
}