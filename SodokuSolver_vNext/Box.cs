using System;

namespace SodokuSolver_vNext
{
	[Serializable]
	internal class Box
	{
		public readonly Cell[] Cells;
		public readonly Cell[][] Rows;
		public readonly Cell[][] Cols;

		public Box(Cell[] cells)
		{
			Cells = cells;
			Rows = new Cell[][]
			{
				new Cell[] { cells[0], cells[1], cells[2] },
				new Cell[] { cells[3], cells[4], cells[5] },
				new Cell[] { cells[6], cells[7], cells[8] }
			};
			Cols = new Cell[][]
			{
				new Cell[] { cells[0], cells[3], cells[6] },
				new Cell[] { cells[1], cells[4], cells[7] },
				new Cell[] { cells[2], cells[5], cells[8] }
			};
		}		
	}
}
