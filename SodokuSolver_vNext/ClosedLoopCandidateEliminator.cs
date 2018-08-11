using System;
using System.Collections.Generic;
using System.Linq;

namespace SodokuSolver_vNext
{
	internal static class ClosedLoopCandidateEliminator
	{
		public static bool ClosedLoop(
			this PuzzleGrid puzzle,
			byte loopScale,
			Func<PuzzleGrid, Cell[][]> getUnits, // row or cols
			Func<PuzzleGrid, Cell[][]> getOtherUnits, // cols or rows
			Func<Cell, byte> getIdxInOtherUnit) // RowIdx or ColIdx
		{
			var anyChanges = false;

			foreach (var candidate in CandidatesUtil.ALL)
			{
				var indiciesOfCandidateWithinUnits = new Dictionary<int, Index>();
				for (var i = 0; i < 9; i++)
				{
					Index indicies;
					if (GetIndiciesOfCandidateIfOccuringTwice(getUnits(puzzle)[i], candidate, out indicies))
					{
						indiciesOfCandidateWithinUnits.Add(i, indicies);
					}
				}
				if (indiciesOfCandidateWithinUnits.Count < loopScale)
				{
					continue;
				}
				var unitCount = indiciesOfCandidateWithinUnits.Count;
				var unitIdxGroups = indiciesOfCandidateWithinUnits.Select(i => i.Key).ToList().UniqueCombinations(loopScale);
				foreach (var unitIdxGroup in unitIdxGroups)
				{
					var units = indiciesOfCandidateWithinUnits
						.Where(i => unitIdxGroup.Contains(i.Key))
						.ToArray();
					var indicies = Index.None;
					foreach(var index in units.Select(u => u.Value))
					{
						indicies |= index;
					}
					if (!indicies.HasNumberOfIndicies(loopScale))
					{
						continue;
					}
					foreach (var unitIndexAndCellIndiciesPair in indiciesOfCandidateWithinUnits)
					{
						var unitIdx = unitIndexAndCellIndiciesPair.Key;
						var indexesInUnit = unitIndexAndCellIndiciesPair.Value.GetValues()
							.Select(idx => IndexUtil.INDEX_BYTE_MAP[idx])
							.ToArray();
						var cell1 = getUnits(puzzle)[unitIdx][indexesInUnit[0]];
						var cell2 = getUnits(puzzle)[unitIdx][indexesInUnit[1]];

						foreach (var cell in getOtherUnits(puzzle)[getIdxInOtherUnit(cell1)])
						{
							if (cell == cell1 || cell == cell2)
							{
								continue;
							}
							anyChanges |= cell.RemoveCandidates(candidate);
						}
					}
				}
			}
			return anyChanges;
		}
		
		private static bool GetIndiciesOfCandidateIfOccuringTwice(
			Cell[] unit,
			Candidates candidate,
			out Index indicies)
		{
			indicies = Index.None;
			var count = 0;
			for (byte i = 0; i < unit.Length; i++)
			{
				if (unit[i].Candidates.Intersects(candidate))
				{
					indicies |= IndexUtil.BYTE_INDEX_MAP[i];
					count++;
				}
			}
			return count == 2;
		}
	}
}
