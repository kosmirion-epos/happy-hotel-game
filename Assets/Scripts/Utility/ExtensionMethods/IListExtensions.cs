using System;
using System.Collections.Generic;
using UnityEngine;

public static class IListExtensions
{
	/// <summary>
	/// Shuffles the element order of the specified list.
	/// </summary>
	public static void Shuffle<T>(this IList<T> ts)
	{
		var count = ts.Count;
		var last = count - 1;
		for (var i = 0; i < last; ++i)
		{
			var r = UnityEngine.Random.Range(i, count);
			(ts[r], ts[i]) = (ts[i], ts[r]);
		}
	}

	/// <summary>
	/// Shuffles the element order of the specified list and returns the shuffled version as a copy."
	/// </summary>
	public static IList<T> ShuffleCopy<T>(this IList<T> ls)
	{
		List<T> ts = new(ls);

		var count = ts.Count;
		var last = count - 1;
		for (var i = 0; i < last; ++i)
		{
			var r = UnityEngine.Random.Range(i, count);
			(ts[r], ts[i]) = (ts[i], ts[r]);
		}

		return ts;
	}

	/// <summary>
	/// Inspects the given List for each individual duplicate.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="ts"></param>
	/// <returns>The total number of duplicates. A value that is present 4 times will count as 3 duplicates.</returns>
	public static int NumberOfDuplicates<T>(this IList<T> ts)
	{
		return ts.Count - new HashSet<T>(ts).Count;
	}

	/// <summary>
	/// Checks if the last <paramref name="numberOfChecks"/> values in the List <paramref name="ts"/> are the same
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="ts"></param>
	/// <param name="numberOfChecks">The number of values to be checked</param>
	/// <returns>True if the <paramref name="numberOfChecks"/> last values are the same</returns>
	public static bool LastSameValue<T>(this IList<T> ts, int numberOfChecks) where T : IEquatable<T>
	{
		if (ts.Count < numberOfChecks)
		{
			return false;
		}
		T b = ts[ts.Count - 1];
		for (int i = 1; i <= numberOfChecks; i++)
		{
			if (!ts[ts.Count - i].Equals(b))
			{
				return false;
			}
		}
		return true;
	}

	/// <summary>
	/// Checks if the <paramref name="ts"/> contains any value more than <paramref name="minInstances"/> times
	/// <br><b>Uses &gt; for comparison.</b></br>
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="ts"></param>
	/// <param name="minInstances"></param>
	/// <returns></returns>
	public static bool ContainsMoreThanMinInstances<T>(this IList<T> ts, int minInstances)
	{
		Dictionary<T, int> b = new();

		for (int i = 0; i < ts.Count; i++)
		{
			T key = ts[i];
			if (!b.ContainsKey(key))
			{
				b.Add(key, 0);
			}

			b[key]++;
		}

		foreach (KeyValuePair<T, int> pair in b)
		{
			if (pair.Value > minInstances)
			{
				return true;
			}
		}

		return false;
	}

	/// <summary>
	/// Checks if the <paramref name="ts"/> contains any value more than or equal to <paramref name="minInstances"/> times.
	/// <br><b>Uses &gt;= for comparisons.</b></br>
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="ts"></param>
	/// <param name="minInstances"></param>
	/// <returns></returns>
	public static bool ContainsMinInstances<T>(this IList<T> ts, int minInstances)
	{
		Dictionary<T, int> b = new();

		for (int i = 0; i < ts.Count; i++)
		{
			T key = ts[i];
			if (!b.ContainsKey(key))
			{
				b.Add(key, 0);
			}

			b[key]++;
		}

		foreach (KeyValuePair<T, int> pair in b)
		{
			if (pair.Value >= minInstances)
			{
				return true;
			}
		}

		return false;
	}

	/// <summary>
	/// Returns a random element of the IList <paramref name="ts"/> this is called on.
	/// </summary>
	/// <typeparam name="T">The type of the collection.</typeparam>
	/// <param name="ts">Any class that implements the IList interface.</param>
	/// <returns>A randomly select element of the collection.</returns>
	public static T GetRandomElement<T>(this IList<T> ts)
	{
		return ts[UnityEngine.Random.Range(0, ts.Count)];
	}

	public static string PrintValues(this IList<UnityEngine.Vector3> ts)
	{
		string s = "";

		foreach (Vector3 v in ts)
		{
			s += $"{{ x: {v.x}, y: {v.y}, z: {v.z} }}, \n";
		}

		return s;
	}
}