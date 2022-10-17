using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
public sealed class VariableBoundsRangeAttribute : PropertyAttribute
{
    public int MinValue { get; set; }
    public int MaxValue { get; set; }
    public string MaxValueName { get; private set; }
    public string MinValueName { get; private set; }

    public VariableBoundsRangeAttribute(int minValue, int maxValue)
	{
		MinValue = minValue;
		MaxValue = maxValue;
	}

    public VariableBoundsRangeAttribute(string minValueName, string maxValueName)
	{
		MinValueName = minValueName;
		MaxValueName = maxValueName;
	}

	public VariableBoundsRangeAttribute(int minValue, string maxValueName)
	{
		MinValue = minValue;
		MaxValueName = maxValueName;
	}

	public VariableBoundsRangeAttribute(string minValueName, int maxValue)
	{
		MinValueName = minValueName;
		MaxValue = maxValue;
	}
}