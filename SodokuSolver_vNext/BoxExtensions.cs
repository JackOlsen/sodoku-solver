using System.Collections.Generic;
using System.Linq;

namespace SodokuSolver_vNext
{
	static class BoxExtensions
	{
		/// <summary>
		/// Looks for candidates that occur in only one row/col within the box. If found,
		/// removes that candidate from all cells in that row/col in other boxes box.
		/// </summary>
		/// <param name="box"></param>
		/// <returns>True if any candidates were eliminated, otherwise False.</returns>
		public static bool TryCandidateLineElimination(this Box box)
		{
			var anyChanges = false;
			foreach (var candidate in CandidatesUtil.ALL)
			{
				var rowsIncludingCandidate = new List<Cell[]>();
				if (box.Rows[0].Any(c => c.Candidates.Intersects(candidate)))
				{
					rowsIncludingCandidate.Add(box.Rows[0]);
				}
				if (box.Rows[1].Any(c => c.Candidates.Intersects(candidate)))
				{
					rowsIncludingCandidate.Add(box.Rows[1]);
				}
				if (box.Rows[2].Any(c => c.Candidates.Intersects(candidate)))
				{
					rowsIncludingCandidate.Add(box.Rows[2]);
				}
				if (rowsIncludingCandidate.Count == 1)
				{
					var row = rowsIncludingCandidate[0];
					foreach (var cell in row[0].RowFriends)
					{
						if (row.Contains(cell))
						{
							continue;
						}
						anyChanges |= cell.RemoveCandidates(candidate);
					}
				}

				var colsIncludingCandidate = new List<Cell[]>();
				if (box.Cols[0].Any(c => c.Candidates.Intersects(candidate)))
				{
					colsIncludingCandidate.Add(box.Cols[0]);
				}
				if (box.Cols[1].Any(c => c.Candidates.Intersects(candidate)))
				{
					colsIncludingCandidate.Add(box.Cols[1]);
				}
				if (box.Cols[2].Any(c => c.Candidates.Intersects(candidate)))
				{
					colsIncludingCandidate.Add(box.Cols[2]);
				}
				if (colsIncludingCandidate.Count == 1)
				{
					var col = colsIncludingCandidate[0];
					foreach (var cell in col[0].ColFriends)
					{
						if (col.Contains(cell))
						{
							continue;
						}
						anyChanges |= cell.RemoveCandidates(candidate);
					}
				}
			}
			return anyChanges;
		}

		public static bool TryCandidateLineElimination(this Box[] allBoxes)
		{
			var anyChanges = false;
			foreach (var box in allBoxes)
			{
				anyChanges |= box.TryCandidateLineElimination();
			}
			return anyChanges;
		}
	}
}
