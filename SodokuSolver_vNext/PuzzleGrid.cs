using System;
using System.Collections.Generic;
using System.Linq;

namespace SodokuSolver_vNext
{
	[Serializable]
	internal class PuzzleGrid
	{
		public readonly Cell[] Cells;
		public readonly Cell[][] Rows;
		public readonly Cell[][] Cols;
		public readonly Box[] Boxes;
		public readonly Cell[][] AllUnits;
		public readonly BoxRow[] BoxRows;
		public readonly BoxCol[] BoxCols;
		public PuzzleGrid(PuzzleSetup setup)
		{
			var cells = new List<Cell>();
			for (byte i = 0; i < setup.Values.Length; i++)
			{
				cells.Add(new Cell(
					idx: i,
					rowIdx: (byte)(i / 9),
					colIdx: (byte)(i % 9),
					boxIdx: (byte)(((i % 9) / 3) + (3 * (i / 27))),
					solution: setup.Values[i]));
			}
			for (byte i = 0; i < setup.Values.Length; i++)
			{
				var cell = cells[i];
				cell.SetFriends(
					rowFriends: CellIdxDictionaries.ROW_FRIENDS[i].Select(idx => cells[idx]).ToArray(),
					colFriends: CellIdxDictionaries.COL_FRIENDS[i].Select(idx => cells[idx]).ToArray(),
					boxFriends: CellIdxDictionaries.BOX_FRIENDS[i].Select(idx => cells[idx]).ToArray());
			}
			Cells = cells.ToArray();
			Rows = CellIdxDictionaries.ROWS.Select(idxs => idxs.Select(idx => cells[idx]).ToArray()).ToArray();
			Cols = CellIdxDictionaries.COLS.Select(idxs => idxs.Select(idx => cells[idx]).ToArray()).ToArray();
			Boxes = CellIdxDictionaries.BOXES.Select(idxs => new Box(idxs.Select(idx => cells[idx]).ToArray())).ToArray();
			AllUnits = Rows.Concat(Cols).Concat(Boxes.Select(b => b.Cells)).ToArray();
			BoxRows = new BoxRow[]
			{
				new BoxRow(
					rows: new [] { Rows[0], Rows[1], Rows[2] },
					boxes: new [] { Boxes[0], Boxes[1], Boxes[2] }),
				new BoxRow(
					rows: new [] { Rows[3], Rows[4], Rows[5] },
					boxes: new [] { Boxes[3], Boxes[4], Boxes[5] }),
				new BoxRow(
					rows: new [] { Rows[6], Rows[7], Rows[8] },
					boxes: new [] { Boxes[6], Boxes[7], Boxes[8] }),
			};
			BoxCols = new BoxCol[]
			{
				new BoxCol(
					cols: new [] { Cols[0], Cols[1], Cols[2] },
					boxes: new [] { Boxes[0], Boxes[3], Boxes[6] }),
				new BoxCol(
					cols: new [] { Cols[3], Cols[4], Cols[5] },
					boxes: new [] { Boxes[1], Boxes[4], Boxes[7] }),
				new BoxCol(
					cols: new [] { Cols[6], Cols[7], Cols[8] },
					boxes: new [] { Boxes[2], Boxes[5], Boxes[8] }),
			};
		}

		public void Print()
		{
			//Console.WriteLine();
			//for (var i = 0; i < Cells.Length; i++)
			//{
			//	if (i % 9 == 0)
			//	{
			//		Console.WriteLine();
			//	}
			//	Console.Write(Cells[i].Solution.HasValue ? Cells[i].Solution.Value.ToString() : "-");
			//}
			//Console.WriteLine();
			//Console.WriteLine();

			var solvedCount = 0;
			var candidateCount = 0;
			for (var i = 0; i < 9; i++)
			{
				for (var subrow = 0; subrow < 3; subrow++)
				{
					for (var j = 0; j < 9; j++)
					{
						var cell = Cells[(i * 9) + j];
						if (subrow == 0)
						{
							solvedCount++;
							foreach (var candidate in CandidatesUtil.ALL)
							{
								if (cell.Candidates.Intersects(candidate))
								{
									candidateCount++;
								}
							}
						}
						if (cell.Solution.HasValue)
						{							
							switch (subrow)
							{
								case 0:
									Console.Write(" ***");
									break;
								case 1:
									Console.Write($" *{ cell.Solution.Value.ToString() }*");
									break;
								case 2:
									Console.Write(" ***");
									break;
							}
						}
						else
						{
							switch (subrow)
							{
								case 0:
									Console.Write(" {0}{1}{2}",
										cell.Candidates.Intersects(Candidates._1) ? "1" : " ",
										cell.Candidates.Intersects(Candidates._2) ? "2" : " ",
										cell.Candidates.Intersects(Candidates._3) ? "3" : " ");
									break;
								case 1:
									Console.Write(" {0}{1}{2}",
										cell.Candidates.Intersects(Candidates._4) ? "4" : " ",
										cell.Candidates.Intersects(Candidates._5) ? "5" : " ",
										cell.Candidates.Intersects(Candidates._6) ? "6" : " ");
									break;
								case 2:
									Console.Write(" {0}{1}{2}",
										cell.Candidates.Intersects(Candidates._7) ? "7" : " ",
										cell.Candidates.Intersects(Candidates._8) ? "8" : " ",
										cell.Candidates.Intersects(Candidates._9) ? "9" : " ");
									break;
							}
						}
					}
					Console.WriteLine();
				}
				Console.WriteLine();
			}
			Console.WriteLine();
			Console.WriteLine();
			Console.WriteLine($"Solved count: { solvedCount }");
			Console.WriteLine($"Candidate count: { candidateCount }");
			Console.WriteLine();
			Console.WriteLine();
		}
	}
}
