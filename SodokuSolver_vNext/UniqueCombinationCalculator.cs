using System.Collections.Generic;
using System.Linq;

namespace SodokuSolver_vNext
{
	public static class UniqueCombinationCalculator
	{
		public static List<List<int>> UniqueCombinations(this List<int> values, int length)
		{
			if (length == 0)
			{
				return new List<List<int>>();
			}
			if(length == 1)
			{
				return values.Select(v => new List<int> { v }).ToList();
			}
			int count = 1;
			var ret = new List<List<int>>();
			foreach (var value in values)
			{
				var valueList = new List<int>{ value };
				foreach (var innerSequence in values.Skip(count).ToList().UniqueCombinations(length - 1))
				{
					ret.Add(valueList.Concat(innerSequence).ToList());
				}
				count++;
			}
			return ret;
		}
	}
}
