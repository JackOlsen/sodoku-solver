namespace SodokuSolver_vNext
{
	static class PuzzleGridExtensions
	{
		public static bool TryDoublePairElimination(this PuzzleGrid puzzle)
		{
			var anyChanges = false;
			foreach (var boxRow in puzzle.BoxRows)
			{
				anyChanges |= boxRow.TryDoublePairElimination();
			}
			foreach (var boxCol in puzzle.BoxCols)
			{
				anyChanges |= boxCol.TryDoublePairElimination();
			}
			return anyChanges;
		}

		public static bool TryClosedLoopCandidateElimination(this PuzzleGrid puzzle, byte loopScale)
		{
			if (puzzle.ClosedLoop(
				loopScale: loopScale,
				getUnits: p => p.Rows,
				getOtherUnits: p => p.Cols,
				getIdxInOtherUnit: c => c.ColIdx))
			{
				return true;
			}
			return puzzle.ClosedLoop(
				loopScale: loopScale,
				getUnits: p => p.Cols,
				getOtherUnits: p => p.Rows,
				getIdxInOtherUnit: c => c.RowIdx);
		}
	}
}
