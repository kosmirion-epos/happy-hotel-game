using NUnit.Framework;
using System.Collections.Generic;

public class IListExtensionsTests
{
	[Test(Description = "Testing for various values and valuetypes, that the correct number of duplicates is returned.")]
	public void NumberOfDuplicatesTest()
	{
		var intList = new List<int>(new int[] { 1, 2, 2, 3, 3, 3, 3, 4, 5, 6, 7, 8 }); //8 individual values, 2 different duplicates
		Assert.AreEqual(4, intList.NumberOfDuplicates());

		var stringList = new List<string>(new string[] { "Kartoffel", "Pinguing", "Tetris" });
		Assert.AreEqual(0, stringList.NumberOfDuplicates());
	}

	[Test(Description = "After Shuffling order of elements should no longer be the same.")]
	public void ShuffleCopyTest()
	{
		UnityEngine.Random.InitState(System.DateTime.Now.GetHashCode());
		var backup = new List<int>(new int[] { 1, 2, 2, 3, 3, 3, 3, 4, 5, 6, 7, 8 });
		var shuffled = backup.ShuffleCopy();

		Assert.AreNotEqual(backup, shuffled);
	}

	[Test(Description = "After Shuffling order of elements should no longer be the same.")]
	public void ShuffleTest()
	{
		var backup = new List<int>(new int[] { 1, 2, 2, 3, 3, 3, 3, 4, 5, 6, 7, 8 });
		var shuffled = new List<int>(backup);
		shuffled.Shuffle();

		Assert.AreNotEqual(backup, shuffled);
	}

	[Test(Description ="The last n elements (n defined on function call) should be the same for this to return to. Always true for n = 1 logically.")]
	public void LastSameTest()
	{
		var data1 = new[] { 1, 2, 3, 4, 5 };
		var data2 = new[] { 1, 2, 3, 4, 4 };
		var data3 = new[] { 1, 2, 3, 4, 4, 4 };

		Assert.IsTrue(data1.LastSameValue(0));
		Assert.IsTrue(data1.LastSameValue(1));
		Assert.IsFalse(data1.LastSameValue(2));

		Assert.IsTrue(data2.LastSameValue(2));
		Assert.IsFalse(data2.LastSameValue(8));

		Assert.IsTrue(data3.LastSameValue(3));
		Assert.IsFalse(data3.LastSameValue(4));
	}

	[Test(Description ="Tests if a collection contains more than n instances of an arbitrary value")]
	public void ContainsMoreThanMinInstancesTest()
	{
		var data = new List<int>(new int[] { 1, 2, 2, 3, 3, 3, 3, 4, 5, 6, 7, 8 });

		Assert.IsTrue(data.ContainsMoreThanMinInstances(2));
		Assert.IsFalse(data.ContainsMoreThanMinInstances(4));
		Assert.IsFalse(data.ContainsMoreThanMinInstances(6));
	}
}
