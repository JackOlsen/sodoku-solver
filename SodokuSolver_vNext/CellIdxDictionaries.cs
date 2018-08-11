using System.Collections.Generic;
using System.Linq;

namespace SodokuSolver_vNext
{
	internal static class CellIdxDictionaries
	{
		private static readonly byte[] ROW_STARTS = { 0, 9, 18, 27, 36, 45, 54, 63, 72 };
		private static readonly byte[] COL_STARTS = { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
		private static readonly byte[] BOX_STARTS = { 0, 3, 6, 27, 30, 33, 54, 57, 60 };

		public static readonly List<byte[]> ROWS = new List<byte[]>();
		public static readonly List<byte[]> BOXES = new List<byte[]>();
		public static readonly List<byte[]> COLS = new List<byte[]>();

		public static readonly Dictionary<byte, byte[]> ROW_FRIENDS = new Dictionary<byte, byte[]>();
		public static readonly Dictionary<byte, byte[]> COL_FRIENDS = new Dictionary<byte, byte[]>();
		public static readonly Dictionary<byte, byte[]> BOX_FRIENDS = new Dictionary<byte, byte[]>();

		static CellIdxDictionaries()
		{
			foreach(var rowStart in ROW_STARTS)
			{
				ROWS.Add(new byte[]
				{
					rowStart,
					(byte)(rowStart + 1),
					(byte)(rowStart + 2),
					(byte)(rowStart + 3),
					(byte)(rowStart + 4),
					(byte)(rowStart + 5),
					(byte)(rowStart + 6),
					(byte)(rowStart + 7),
					(byte)(rowStart + 8)
				});
			}
			foreach (var colStart in COL_STARTS)
			{
				COLS.Add(new byte[]
				{
					colStart,
					(byte)(colStart + 9),
					(byte)(colStart + 18),
					(byte)(colStart + 27),
					(byte)(colStart + 36),
					(byte)(colStart + 45),
					(byte)(colStart + 54),
					(byte)(colStart + 63),
					(byte)(colStart + 72)
				});
			}
			foreach (var boxStart in BOX_STARTS)
			{
				BOXES.Add(new byte[]
				{
					boxStart,
					(byte)(boxStart + 1),
					(byte)(boxStart + 2),
					(byte)(boxStart + 9),
					(byte)(boxStart + 10),
					(byte)(boxStart + 11),
					(byte)(boxStart + 18),
					(byte)(boxStart + 19),
					(byte)(boxStart + 20)
				});
			}
			for (byte i = 0; i < 81; i++)
			{
				ROW_FRIENDS[i] = GetFriends(ROWS, i);
				COL_FRIENDS[i] = GetFriends(COLS, i);
				BOX_FRIENDS[i] = GetFriends(BOXES, i);
			}
		}

		private static byte[] GetFriends(List<byte[]> friendSets, byte idx)
		{
			return friendSets
				.First(r => r.Contains(idx))
				.Where(i => i != idx)
				.ToArray();
		}
	}
}
