//using NUnit.Framework;

//namespace SodokuSolver_vNext.Tests
//{
//	[TestFixture]
//	internal class UniqueCombinationCalculatorTests
//	{
//		[Test]
//		public void LengthOne()
//		{
//			var combos = new[] { 1, 2, 3 }.UniqueCombinations(1);
//			Assert.AreEqual(3, combos.Count);
//			Assert.AreEqual(1, combos[0].Count);
//			Assert.AreEqual(1, combos[1].Count);
//			Assert.AreEqual(1, combos[2].Count);
//			Assert.AreEqual(1, combos[0][0]);
//			Assert.AreEqual(2, combos[1][0]);
//			Assert.AreEqual(3, combos[2][0]);
//		}

//		[Test]
//		public void LengthTwo()
//		{
//			var combos = new[] { 1, 2, 3 }.UniqueCombinations(2);
//			Assert.AreEqual(3, combos.Count);
//			Assert.AreEqual(2, combos[0].Count);
//			Assert.AreEqual(2, combos[1].Count);
//			Assert.AreEqual(2, combos[2].Count);

//			Assert.AreEqual(1, combos[0][0]);
//			Assert.AreEqual(2, combos[0][1]);

//			Assert.AreEqual(1, combos[1][0]);
//			Assert.AreEqual(3, combos[1][1]);

//			Assert.AreEqual(2, combos[2][0]);
//			Assert.AreEqual(3, combos[2][1]);
//		}

//		[Test]
//		public void LengthThree()
//		{
//			var combos = new[] { 1, 2, 3 }.UniqueCombinations(3);
//			Assert.AreEqual(1, combos.Count);
//			Assert.AreEqual(3, combos[0].Count);
//			Assert.AreEqual(1, combos[0][0]);
//			Assert.AreEqual(2, combos[0][1]);
//			Assert.AreEqual(3, combos[0][2]);
//		}
//	}
//}
