using System.Linq;
using FP;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LambdaComparer.Test
{
	[TestClass]
	public class EnumerableExTest
	{
		[TestMethod]
		public void PartitionEmptyTest()
		{
			// ARRANGE
			var x = new int[0];

			// ACT
			var parts = x.Partition();

			// ASSERT
			Assert.IsNotNull(parts, "Empty partitioned sequence should not be null");
			Assert.IsFalse(parts.Any(), "Emty sequence partitioned shold result to an empty sequence");
		}

		[TestMethod]
		public void PartitionTest()
		{
			// ARRANGE
			var input = new string('a', 64);

			// ACT
			var parts = input.Partition(8).Select(x => new string(x.ToArray())).ToList();

			//ASSERT
			Assert.AreEqual(8, parts.Count);
			Assert.IsTrue(parts.All(x => x == "aaaaaaaa"));
		}

		[TestMethod]
		public void DistinctTest()
		{
			// ARRANGE
			var list = new[] { "a", "aa", "aaa", "b", "bb", "bbb" };

			// ACT
			var distinct = list.Distinct(x => x.Length).ToList();

			// ASSERT
			CollectionAssert.AreEqual(new[] { "a", "aa", "aaa" }, distinct);
		}

		[TestMethod]
		public void ExceptTest()
		{
			// ARRANGE
			var list1 = new[] { "a", "aa", "aaa", "aaaa" };
			var list2 = new[] { "dd", "eeee" };

			// ACT
			var except = list1.Except(list2, x => x.Length).ToList();

			// ASSERT
			CollectionAssert.AreEqual(new[] { "a", "aaa" }, except);
		}

		[TestMethod]
		public void IntersectTest()
		{
			// ARRANGE
			var list1 = new[] { "a", "aa", "aaa", "aaaa" };
			var list2 = new[] { "dd", "eeee" };

			// ACT
			var intersect = list1.Intersect(list2, x => x.Length).ToList();

			// ASSERT
			CollectionAssert.AreEqual(new[] { "aa", "aaaa" }, intersect);
		}

		[TestMethod]
		public void ContainsTest()
		{
			//ARRANGE
			var list1 = new[] { "a", "aa", "aaa", "aaaa" };

			//ACT
			var contains = list1.Contains("bbb", x => x.Length);

			//ASSERT
			Assert.IsTrue(contains);
		}

		[TestMethod]
		public void UnionTest()
		{
			//ARRANGE
			var list1 = new[] { "a", "aa", "aaa" };
			var list2 = new[] { "dd", "eee", "ffff" };

			//ACT
			var union = list1.Union(list2, x => x.Length).ToList();

			//ASSERT
			CollectionAssert.AreEqual(new[] { "a", "aa", "aaa", "ffff" }, union);
		}
	}
}
