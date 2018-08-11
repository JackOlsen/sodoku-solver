using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SodokuSolver_vNext.Tests
{
	[TestFixture]
	internal class CandidatesTests
	{
		[Test]
		public void something()
		{
			var x = Candidates._1 | Candidates._2;
			var x2 = Candidates._1 | Candidates._2;
			var y = Candidates._2 | Candidates._3;

			var i = Candidates._1 | Candidates._2 | Candidates._3 | Candidates._4;
			var j = Candidates._1 | Candidates._5;
			var k = i & ~j;
			Assert.IsTrue(i.Intersects(j));
			//var z = x &= ~y;
			Assert.IsTrue((x & ~y) != x);
			Assert.AreEqual(x, x2);
			Assert.IsTrue(x.Intersects(x2));
			//Assert.AreEqual(Candidates._1, z);

			//Assert.IsTrue(x.Includes(y));
			////Assert.IsTrue(x.HasFlag(y));
			//Assert.IsTrue(y.Includes(x));
			//Assert.IsTrue(y.HasFlag(x));
		}
	}
}
