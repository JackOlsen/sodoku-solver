using System;
using System.Linq;

namespace SodokuSolver_vNext
{
	[Serializable]
	internal class BoxRow : IBoxGroup
	{
		public Cell[][] BoxLine { get; private set; }
		public Box[] Boxes { get; private set; }
		public Box[][] BoxPairsPlusOtherBox { get; private set; }
		public Func<Cell, byte> GetLineIdx => c => c.RowIdx;

		public BoxRow(Cell[][] rows, Box[] boxes)
		{
			BoxLine = rows;
			Boxes = boxes;
			BoxPairsPlusOtherBox = new[]
			{
				new [] { boxes[0], boxes[1], boxes[2] },
				new [] { boxes[0], boxes[2], boxes[1] },
				new [] { boxes[1], boxes[2], boxes[0] },
			};
		}
	}

	[Serializable]
	internal class BoxCol : IBoxGroup
	{
		public Cell[][] BoxLine { get; private set; }
		public Box[] Boxes { get; private set; }
		public Box[][] BoxPairsPlusOtherBox { get; private set; }
		public Func<Cell, byte> GetLineIdx => c => c.ColIdx;

		public BoxCol(Cell[][] cols, Box[] boxes)
		{
			BoxLine = cols;
			Boxes = boxes;
			BoxPairsPlusOtherBox = new[]
			{
				new [] { boxes[0], boxes[1], boxes[2] },
				new [] { boxes[0], boxes[2], boxes[1] },
				new [] { boxes[1], boxes[2], boxes[0] },
			};
		}
	}

	internal interface IBoxGroup
	{
		Cell[][] BoxLine { get; }
		Box[] Boxes { get; }
		Box[][] BoxPairsPlusOtherBox { get; }

		Func<Cell,byte> GetLineIdx { get; }
	}

	internal static class IBoxGroupExtensions
	{
		public static bool TryDoublePairElimination(this IBoxGroup boxGroup)
		{
			var anyChanges = false;
			foreach (var candidate in CandidatesUtil.ALL)
			{
				var candidateEliminated = false;
				foreach (var boxPairPlusOtherBox in boxGroup.BoxPairsPlusOtherBox)
				{
					var box0 = boxPairPlusOtherBox[0];
					var box0CellsIncludingCandidate = box0.Cells
						.Where(c => c.Candidates.Intersects(candidate))
						.ToArray();
					if (box0CellsIncludingCandidate.Length != 2)
					{
						continue;
					}

					var box1 = boxPairPlusOtherBox[1];
					var box1CellsIncludingCandidate = box1.Cells
						.Where(c => c.Candidates.Intersects(candidate))
						.ToArray();
					if (box1CellsIncludingCandidate.Length != 2)
					{
						continue;
					}
					var lineIdxs = box0CellsIncludingCandidate.Concat(box1CellsIncludingCandidate)
						.GroupBy(c => boxGroup.GetLineIdx(c))
						.Select(g => g.Key)
						.ToArray();
					if (lineIdxs.Length != 2)
					{
						continue;
					}
					foreach(var cell in boxPairPlusOtherBox[2].Cells)
					{
						if(!lineIdxs.Contains(cell.Idx) && cell.RemoveCandidates(candidate))
						{
							candidateEliminated = true;
							anyChanges = true;
						}
					}
					if (candidateEliminated)
					{
						continue;
					}
				}
			}
			return anyChanges;
		}
	}
}
