using System;
using System.Collections.Generic;
using System.Linq;

namespace SodokuSolver_vNext
{
	[Flags]
	public enum Candidates
	{
		None = 0,
		_1 = 1 << 0,
		_2 = 1 << 1,
		_3 = 1 << 2,
		_4 = 1 << 3,
		_5 = 1 << 4,
		_6 = 1 << 5,
		_7 = 1 << 6,
		_8 = 1 << 7,
		_9 = 1 << 8,
		All = _1 | _2 | _3 | _4 | _5 | _6 | _7 | _8 | _9
	}

	public static class CandidatesUtil
	{
		public static readonly Candidates[] ALL =
		{
			Candidates._1,
			Candidates._2,
			Candidates._3,
			Candidates._4,
			Candidates._5,
			Candidates._6,
			Candidates._7,
			Candidates._8,
			Candidates._9
		};

		private static readonly List<Candidates> ALL_PAIRS;
		private static readonly List<Candidates> ALL_TRIPLES;
		private static readonly List<Candidates> ALL_QUADS;
		private static readonly List<Candidates> ALL_QUINTS;

		static CandidatesUtil()
		{
			var pairs = new List<Candidates>();
			var triples = new List<Candidates>();
			var quads = new List<Candidates>();
			var quints = new List<Candidates>();
			foreach (var c0 in ALL)
			{
				foreach (var c1 in ALL)
				{
					pairs.Add(c0 | c1);
					foreach (var c2 in ALL)
					{
						triples.Add(c0 | c1 | c2);
						foreach(var c3 in ALL)
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

		public static bool Intersects(this Candidates candidates, Candidates candidate)
		{
			return (candidates & candidate) != 0;
		}

		public static readonly Dictionary<Candidates, byte> CANDIDATE_BYTE_MAP = new Dictionary<Candidates, byte>
		{
			[Candidates._1] = 1,
			[Candidates._2] = 2,
			[Candidates._3] = 3,
			[Candidates._4] = 4,
			[Candidates._5] = 5,
			[Candidates._6] = 6,
			[Candidates._7] = 7,
			[Candidates._8] = 8,
			[Candidates._9] = 9,
		};

		public static readonly Dictionary<byte, Candidates> BYTE_CANDIDATE_MAP = new Dictionary<byte, Candidates>
		{
			[1] = Candidates._1,
			[2] = Candidates._2,
			[3] = Candidates._3,
			[4] = Candidates._4,
			[5] = Candidates._5,
			[6] = Candidates._6,
			[7] = Candidates._7,
			[8] = Candidates._8,
			[9] = Candidates._9,
		};

		public static bool HasNumberOfCandidates(this Candidates candidates, byte count)
		{
			if(candidates == Candidates.None)
			{
				return false;
			}
			switch (count)
			{
				case 1:
					var byteVal = (short)candidates;
					return (byteVal & (byteVal - 1)) == 0;
				case 2:
					return ALL_PAIRS.BinarySearch(candidates) > 0;
				case 3:
					return ALL_TRIPLES.BinarySearch(candidates) > 0;
				case 4:
					return ALL_QUADS.BinarySearch(candidates) > 0;
				case 5:
					return ALL_QUINTS.BinarySearch(candidates) > 0;
				default:
					throw new ApplicationException();
			}
		}

		public static int CountExcluding(this Candidates candidates, Candidates exclusions)
		{
			var count = 0;
			while(candidates != Candidates.None)
			{
				candidates = candidates & (candidates - 1);
				count++;
			}
			var exclusionCount = 0;
			while (exclusions != Candidates.None)
			{
				exclusions = exclusions & (exclusions - 1);
				count++;
			}
			return count - exclusionCount;
		}

		public static Candidates First(this Candidates candidates)
		{
			return ALL.First(c => candidates.Intersects(c));
		}

		public static Candidates FirstOrDefaultExcluding(this Candidates candidates, Candidates exclusions)
		{
			return ALL
				.Where(c => !exclusions.Intersects(c))
				.FirstOrDefault(c => candidates.Intersects(c));
		}
	}
}
