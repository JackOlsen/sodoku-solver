using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SodokuSolver_vNext
{
	public static class ObjectExtensions
	{
		public static T DeepClone<T>(this T obj)
		{
			using (var ms = new MemoryStream())
			{
				var formatter = new BinaryFormatter();
				formatter.Serialize(ms, obj);
				ms.Position = 0;

				return (T)formatter.Deserialize(ms);
			}
		}
	}
}
