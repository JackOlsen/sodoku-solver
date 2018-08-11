using System.Linq;

namespace SodokuSolver_vNext
{
	static class CellExtensions
	{		public static void Initialize(this Cell cell)
		{
			cell.Candidates = cell.Solution.HasValue ? Candidates.None : cell.Candidates = Candidates.All;
			foreach (var value in cell.AllFriends
				.Where(f => f.Solution.HasValue)
				.Select(f => f.Solution.Value)
				.Distinct()
				.ToArray())
			{
				cell.RemoveCandidates(CandidatesUtil.BYTE_CANDIDATE_MAP[value]);
			}
		}

		public static void Solve(this Cell cell, Candidates solutionCandidate)
		{
			cell.Solution = CandidatesUtil.CANDIDATE_BYTE_MAP[solutionCandidate];
			foreach (var friend in cell.AllFriends)
			{
				friend.RemoveCandidates(solutionCandidate);
			}
			if (cell.AllFriends.Any(c => c.Solution == cell.Solution))
			{
				throw new InvalidSolutionException();
			}
			cell.Candidates = Candidates.None;
		}

		/// <summary>
		/// Removes candidates from cell's Candidates. In the event that the cell is left with
		/// just one candidate, also solves the cell.
		/// </summary>
		/// <param name="cell"></param>
		/// <param name="candidates"></param>
		/// <returns>True if any candidates were removed.</returns>
		public static bool RemoveCandidates(this Cell cell, Candidates candidates)
		{
			if ((cell.Candidates & candidates) != 0)
			{
				cell.Candidates &= ~candidates;
				if (cell.Candidates.HasNumberOfCandidates(1))
				{
					cell.Solve(cell.Candidates);
				}
				return true;
			}
			return false;
		}

		/// <summary>
		/// Check to see if there is exactly one candidate in this cell.
		/// If so, solve the cell.
		/// </summary>
		/// <returns>True if the cell was solved, otherwise False.</returns>
		public static bool TrySingleCandidateSolve(this Cell cell)
		{
			if (cell.Candidates.HasNumberOfCandidates(1))
			{
				cell.Solve(cell.Candidates);
				return true;
			}
			return false;
		}

		/// <summary>
		/// For each candidate in this cell, check to see if it is unqiue among all of this cell's friends' candidates.
		/// If so, solve the cell.
		/// </summary>
		/// <returns>True if the cell was solved, otherwise False.</returns>
		public static bool TryUniqueCandidateSolve(this Cell cell)
		{
			foreach (var candidate in CandidatesUtil.ALL)
			{
				if (!cell.Candidates.Intersects(candidate))
				{
					continue;
				}
				foreach (var friendGroup in cell.FriendGroups)
				{
					if (!friendGroup.Any(f => f.Candidates.Intersects(candidate)))
					{
						cell.Candidates = candidate;
						cell.Solve(candidate);
						return true;
					}
				}
			}
			return false;
		}

		public static bool TrySingleCandidateSolve(this Cell[] unsolvedCells)
		{
			var anyChanges = false;
			foreach (var cell in unsolvedCells)
			{
				anyChanges |= cell.TrySingleCandidateSolve();
			}
			return anyChanges;
		}

		public static bool TryUniqueCandidateSolve(this Cell[] unsolvedCells)
		{
			var anyChanges = false;
			foreach (var cell in unsolvedCells)
			{
				anyChanges |= cell.TryUniqueCandidateSolve();
			}
			return anyChanges;
		}

		public static bool TryNakedPairElimination(this Cell[][] allUnits)
		{
			var anyChanges = false;
			foreach (var unit in allUnits)
			{
				anyChanges |= unit.TryNakedPairElimination();
			}
			return anyChanges;
		}

		public static bool TryNakedTripleElimination(this Cell[][] allUnits)
		{
			var anyChanges = false;
			foreach (var unit in allUnits)
			{
				anyChanges |= unit.TryNakedTripleElimination();
			}
			return anyChanges;
		}

		public static bool TryNakedQuadElimination(this Cell[][] allUnits)
		{
			var anyChanges = false;
			foreach (var unit in allUnits)
			{
				anyChanges |= unit.TryNakedQuadElimination();
			}
			return anyChanges;
		}

		public static bool TryNakedQuintElimination(this Cell[][] allUnits)
		{
			var anyChanges = false;
			foreach (var unit in allUnits)
			{
				anyChanges |= unit.TryNakedQuintElimination();
			}
			return anyChanges;
		}
	}
}
