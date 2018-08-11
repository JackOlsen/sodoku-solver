using System;
using System.Collections.Generic;
using System.Linq;

namespace SodokuSolver_vNext
{
	internal static class NakedCandidateGroupEliminator
	{
		private static readonly int[][] PAIR_COMBINATIONS;
		private static readonly int[][] TRIPLE_COMBINATIONS;
		private static readonly int[][] QUAD_COMBINATIONS;
		private static readonly int[][] QUINT_COMBINATIONS;

		static NakedCandidateGroupEliminator()
		{
			var pairs = new List<int[]>();
			var triples = new List<int[]>();
			var quads = new List<int[]>();
			var quints = new List<int[]>();
			for (var i = 0; i < 9; i++)
			{
				for (var j = 0; j < 9; j++)
				{
					if(j == i)
					{
						continue;
					}
					if(!pairs.Any(p => p.Contains(i) && p.Contains(j)))
					{
						pairs.Add(new[] { i, j });
					}
					for (var k = 0; k < 9; k++)
					{
						if (k == i || k == j)
						{
							continue;
						}
						if(!triples.Any(t => t.Contains(i) && t.Contains(j) && t.Contains(k)))
						{
							triples.Add(new[] { i, j, k });
						}
						for (var l = 0; l < 9; l++)
						{
							if (l == i || l == j || l == k)
							{
								continue;
							}
							if (quads.Any(q => q.Contains(i) && q.Contains(j) && q.Contains(k) && q.Contains(l)))
							{
								quads.Add(new[] { i, j, k, l });
							}
							for(var m = 0; m < 9; m++)
							{
								if(m == i || m== j || m == k || m == l)
								{
									continue;
								}
								if (quints.Any(q => q.Contains(i) && q.Contains(j) && q.Contains(k) && q.Contains(l) && q.Contains(m)))
								{
									quints.Add(new[] { i, j, k, l, m });
								}
							}						
						}
					}
				}
			}
			PAIR_COMBINATIONS = pairs.ToArray();
			TRIPLE_COMBINATIONS = triples.ToArray();
			QUAD_COMBINATIONS = quads.ToArray();
			QUINT_COMBINATIONS = quints.ToArray();
		}

		public static bool TryNakedPairElimination(this Cell[] unit) =>
			TryNakedGroupElimination(unit, PAIR_COMBINATIONS, 2);

		public static bool TryNakedTripleElimination(this Cell[] unit) => 
			TryNakedGroupElimination(unit, TRIPLE_COMBINATIONS, 3);

		public static bool TryNakedQuadElimination(this Cell[] unit) =>
			TryNakedGroupElimination(unit, QUAD_COMBINATIONS, 4);

		public static bool TryNakedQuintElimination(this Cell[] unit) =>
			TryNakedGroupElimination(unit, QUINT_COMBINATIONS, 5);

		/// <summary>
		/// Searches for a group of cells within the unit where the number of distinct candidates is equal 
		/// to the number of cells in the group. In that case, all of those candidates will end up within 
		/// the group, and therefore can be removed from all other cells in the unit.
		/// </summary>
		/// <param name="unit"></param>
		/// <param name="groupings">All distinct combinations of cell indicies for a group of size <paramref name="groupSize"/></param>
		/// <param name="groupSize"></param>
		/// <returns>True if any candidates were eliminated, otherwise false.</returns>
		private static bool TryNakedGroupElimination(
			this Cell[] unit, 
			int[][] groupings,
			byte groupSize)
		{
			var anyChanges = false;
			foreach(var group in groupings)
			{
				var cells = new List<Cell>();
				foreach(var idx in group)
				{
					cells.Add(unit[idx]);
				}
				if (cells.Any(c => c.Solution.HasValue))
				{
					continue;
				}
				var aggregateCandidates = Candidates.None;
				foreach (var cell in cells)
				{
					aggregateCandidates |= cell.Candidates;
				}
				if (aggregateCandidates.HasNumberOfCandidates(groupSize))
				{
					foreach (var cell in unit)
					{
						if (cell.Solution.HasValue || cells.Contains(cell))
						{
							continue;
						}
						anyChanges |= cell.RemoveCandidates(aggregateCandidates);
					}
				}
			}
			return anyChanges;
		}
	}
}
