using System;
using System.Collections.Generic;
using System.Linq;

namespace SodokuSolver_vNext
{
	internal class Program
	{
		internal static void Main(string[] args)
		{
			while (true)
			{
				Console.WriteLine("Puzzles:");
				for (var i = 0; i < PuzzleSetups.PUZZLES.Count(); i++)
				{
					Console.WriteLine($"{i + 1}) {PuzzleSetups.PUZZLES[i].Name}");
				}
				var input = Console.ReadLine();
				if (input == "exit")
				{
					return;
				}
				int puzzleIdx;
				while (!int.TryParse(input, out puzzleIdx) || puzzleIdx > PuzzleSetups.PUZZLES.Length)
				{
					Console.WriteLine("Invalid.");
					input = Console.ReadLine();
				}
				var start = DateTime.Now;
				Solve(PuzzleSetups.PUZZLES[puzzleIdx - 1]);
				var end = DateTime.Now;
				Console.WriteLine($"Execution time (ms): {(end - start).TotalMilliseconds}");
			}
		}

		private static Dictionary<int, Candidates> IncorrectGuesses;

		private static void Solve(PuzzleSetup puzzleSetup)
		{
			IncorrectGuesses = new Dictionary<int, Candidates>();
			for(var i = 0; i < 81; i++)
			{
				IncorrectGuesses[i] = Candidates.None;
			}
			var puzzle = new PuzzleGrid(puzzleSetup);
			foreach (var cell in puzzle.Cells)
			{
				cell.Initialize();
			}
			puzzle.Print();
			bool solved;
			var result = Solve(puzzle, out solved);
			Console.WriteLine(solved ? "Solved!" : "Failed");
			result.Print();
		}

		private static PuzzleGrid Solve(PuzzleGrid puzzle, out bool solved)
		{
			var anyChanges = false;
			do
			{
				anyChanges = false;
				var unsolvedCells = puzzle.Cells
					.Where(c => !c.Solution.HasValue)
					.ToArray();
				if (!unsolvedCells.Any())
				{
					solved = true;
					return puzzle;
				}
				if (anyChanges |= unsolvedCells.TrySingleCandidateSolve())
				{
					Console.WriteLine("Single candidate.");
					continue;
				}
				if (anyChanges |= unsolvedCells.TryUniqueCandidateSolve())
				{
					Console.WriteLine("Unique candidate.");
					continue;
				}
				if (anyChanges |= puzzle.Boxes.TryCandidateLineElimination())
				{
					Console.WriteLine("Candidate line.");
					continue;
				}
				if (anyChanges |= puzzle.AllUnits.TryNakedPairElimination())
				{
					Console.WriteLine("Naked pair.");
					continue;
				}
				if (anyChanges |= puzzle.AllUnits.TryNakedTripleElimination())
				{
					Console.WriteLine("Naked triple.");
					continue;
				}
				if (anyChanges |= puzzle.AllUnits.TryNakedQuadElimination())
				{
					Console.WriteLine("Naked quadruple.");
					continue;
				}
				if (anyChanges |= puzzle.AllUnits.TryNakedQuintElimination())
				{
					Console.WriteLine("Naked quintuplet.");
					continue;
				}
				if (anyChanges |= puzzle.TryDoublePairElimination())
				{
					Console.WriteLine("Double pair.");
					continue;
				}
				if (anyChanges |= puzzle.TryClosedLoopCandidateElimination(loopScale: 2))
				{
					Console.WriteLine("X-Wing.");
					continue;
				}
				if (anyChanges |= puzzle.TryClosedLoopCandidateElimination(loopScale: 3))
				{
					Console.WriteLine("Swordfish.");
					continue;
				}
				if (anyChanges |= puzzle.TryClosedLoopCandidateElimination(loopScale: 4))
				{
					Console.WriteLine("Closed loop, scale 4.");
					continue;
				}
				if (anyChanges |= puzzle.TryClosedLoopCandidateElimination(loopScale: 5))
				{
					Console.WriteLine("Closed loop, scale 5.");
					continue;
				}
				//var mostSolvedCellIdx = unsolvedCells
				//	.OrderBy(c => c.Candidates.CountExcluding(IncorrectGuesses[c.Idx]))
				//	.First()
				//	.Idx;
				//var clone = puzzle.DeepClone();
				//var cell = clone.Cells[mostSolvedCellIdx];
				//var nextGuess = cell.Candidates.FirstOrDefaultExcluding(IncorrectGuesses[mostSolvedCellIdx]);
				//if(nextGuess == Candidates.None)
				//{
				//	throw new InvalidOperationException($"No remaining guesses for cell {mostSolvedCellIdx}");
				//}
				//Console.WriteLine($"Guessing {nextGuess} on cell {cell.Idx}");				
				//cell.Solve(nextGuess);
				//try
				//{
				//	Solve(clone, out solved);
				//	if (solved)
				//	{
				//		return clone;
				//	}
				//	anyChanges = true;
				//}
				//catch (InvalidSolutionException)
				//{
				//	Console.WriteLine("Incorrect guess.");					
				//	IncorrectGuesses[mostSolvedCellIdx] |= nextGuess;
				//	solved = false;
				//	return clone;
				//}
			}
			while (anyChanges);
			solved = false;
			return puzzle;
		}
	}
}
