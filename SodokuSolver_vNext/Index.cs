using System;
using System.Collections.Generic;
using System.Linq;

namespace SodokuSolver_vNext
{
	[Flags]
	internal enum Index
	{
		None = 0,
		_0 = 1 << 0,
		_1 = 1 << 1,
		_2 = 1 << 2,
		_3 = 1 << 3,
		_4 = 1 << 4,
		_5 = 1 << 5,
		_6 = 1 << 6,
		_7 = 1 << 7,
		_8 = 1 << 8
	}

	internal static class IndexUtil
	{
		public static readonly Dictionary<Index, byte> INDEX_BYTE_MAP = new Dictionary<Index, byte>
		{
			[Index._0] = 0,
			[Index._1] = 1,
			[Index._2] = 2,
			[Index._3] = 3,
			[Index._4] = 4,
			[Index._5] = 5,
			[Index._6] = 6,
			[Index._7] = 7,
			[Index._8] = 8
		};

		public static readonly Dictionary<byte, Index> BYTE_INDEX_MAP = new Dictionary<byte, Index>
		{
			[0] = Index._0,
			[1] = Index._1,
			[2] = Index._2,
			[3] = Index._3,
			[4] = Index._4,
			[5] = Index._5,
			[6] = Index._6,
			[7] = Index._7,
			[8] = Index._8
		};

		public static readonly Index[] ALL =
		{
			Index._0,
			Index._1,
			Index._2,
			Index._3,
			Index._4,
			Index._5,
			Index._6,
			Index._7,
			Index._8
		};

		private static readonly List<Index> ALL_PAIRS;
		private static readonly List<Index> ALL_TRIPLES;
		private static readonly List<Index> ALL_QUADS;
		private static readonly List<Index> ALL_QUINTS;

		static IndexUtil()
		{
			var pairs = new List<Index>();
			var triples = new List<Index>();
			var quads = new List<Index>();
			var quints = new List<Index>();
			foreach (var c0 in ALL)
			{
				foreach (var c1 in ALL)
				{
					pairs.Add(c0 | c1);
					foreach (var c2 in ALL)
					{
						triples.Add(c0 | c1 | c2);
						foreach (var c3 in ALL)
						{
							quads.Add(c0 | c1 | c2 | c3);
							foreach (var c4 in ALL)
							{
								quints.Add(c0 | c1 | c2 | c3 | c4);
							}
						}
					}
				}
			}
			ALL_PAIRS = pairs.Distinct().ToList();
			ALL_PAIRS.Sort();
			ALL_TRIPLES = triples.Distinct().ToList();
			ALL_TRIPLES.Sort();
			ALL_QUADS = quads.Distinct().ToList();
			ALL_QUADS.Sort();
			ALL_QUINTS = quints.Distinct().ToList();
			ALL_QUINTS.Sort();
		}

		public static bool HasNumberOfIndicies(this Index indicies, byte count)
		{
			if (indicies == Index.None)
			{
				return false;
			}
			switch (count)
			{
				case 1:
					var byteVal = (short)indicies;
					return (byteVal & (byteVal - 1)) == 0;
				case 2:
					return ALL_PAIRS.BinarySearch(indicies) > 0;
				case 3:
					return ALL_TRIPLES.BinarySearch(indicies) > 0;
				case 4:
					return ALL_QUADS.BinarySearch(indicies) > 0;
				default:
					throw new ApplicationException();
			}
		}

		public static List<Index> GetValues(this Index indicies)
		{
			var results = new List<Index>();
			foreach (var index in ALL)
			{
				if ((indicies & index) != 0)
				{
					results.Add(index);
				}
			}
			return results;
		}
	}
}
