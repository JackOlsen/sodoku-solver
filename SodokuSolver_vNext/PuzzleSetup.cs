using System.Linq;

namespace SodokuSolver_vNext
{
	public class PuzzleSetup
	{
		public readonly string Name;
		public readonly byte?[] Values;

		public PuzzleSetup(string name, byte[] values)
		{
			Name = name;
			Values = values
				.Select(v => v == 0 ? new byte?() : v)
				.ToArray();
		}
	}
}
