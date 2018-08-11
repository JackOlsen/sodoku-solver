using System;
using System.Linq;

namespace SodokuSolver_vNext
{
	[Serializable]
	internal class Cell
	{
		public readonly byte Idx;
		public byte RowIdx;
		public byte ColIdx;
		public byte BoxIdx;
		public byte? Solution;

		public Cell[] RowFriends;
		public Cell[] ColFriends;
		public Cell[] BoxFriends;
		public Cell[] AllFriends;
		public Cell[][] FriendGroups;

		public Candidates Candidates;

		public Cell(
			byte idx, 
			byte rowIdx,
			byte colIdx,
			byte boxIdx,
			byte? solution)
		{
			Idx = idx;
			RowIdx = rowIdx;
			ColIdx = colIdx;
			BoxIdx = boxIdx;
			Solution = solution;
		}

		public void SetFriends(Cell[] rowFriends, Cell[] colFriends, Cell[] boxFriends)
		{
			RowFriends = rowFriends;
			ColFriends = colFriends;
			BoxFriends = boxFriends;
			AllFriends = rowFriends.Concat(colFriends).Concat(boxFriends).ToArray();
			FriendGroups = new []
			{
				RowFriends,
				ColFriends,
				BoxFriends
			};
		}		

		public override string ToString()
		{
			return $"[{Idx}] : {Solution} - {Candidates}";
		}

		public override bool Equals(object obj)
		{
			return Idx == ((Cell)obj).Idx;
		}

		public override int GetHashCode()
		{
			return Idx.GetHashCode();
		}
	}
}
