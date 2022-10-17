using UnityEngine;
using UnityEditor;
using System.Reflection;
using NaughtyAttributes.Editor;

[CustomPropertyDrawer(typeof(VariableBoundsRangeAttribute))]
internal sealed class VariableBoundsRangeDrawer : PropertyDrawer
{
    private float value;

	public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
	{
        EditorGUI.BeginProperty(rect, label, property);

		if (!IsNumber(property))
		{
            string message = $"Field {property.name} is not a number";
            DrawDefaultPropertyAndHelpBox(rect, property, message, MessageType.Warning);
            return;
		}

        VariableBoundsRangeAttribute rangeAttribute = PropertyUtility.GetAttribute<VariableBoundsRangeAttribute>(property);
        var minValue = GetMinValue(property, rangeAttribute);
        var maxValue = GetMaxValue(property, rangeAttribute);

		if ((maxValue != null && IsNumber(maxValue) && (minValue != null && IsNumber(minValue))))
		{
			if (property.propertyType == SerializedPropertyType.Integer)
			{
                property.intValue = EditorGUI.IntSlider(rect, label, property.intValue, (int)minValue, (int)maxValue);
			}
			else if (property.propertyType == SerializedPropertyType.Float)
			{
				property.floatValue = EditorGUI.Slider(rect, label, property.floatValue, (float)minValue, (float)maxValue);
			}
		}
        else
        {
            string message = $"The provided dynamic max value for the progress bar is not correct. Please check if the '{nameof(rangeAttribute.MaxValueName)}' is correct, or the return type is float/int";

            DrawDefaultPropertyAndHelpBox(rect, property, message, MessageType.Warning);
        }

        EditorGUI.EndProperty();
    }

    private object GetMaxValue(SerializedProperty property, VariableBoundsRangeAttribute attribute)
	{
		if (string.IsNullOrEmpty(attribute.MaxValueName))
		{
            return attribute.MaxValue;
		}
		else
		{
            object target = PropertyUtility.GetTargetObjectWithProperty(property);

            FieldInfo valuesFieldInfo = ReflectionUtility.GetField(target, attribute.MaxValueName);
			if (valuesFieldInfo != null)
			{
                return valuesFieldInfo.GetValue(target);
			}

            PropertyInfo valuesPropertyInfo = ReflectionUtility.GetProperty(target, attribute.MaxValueName);
			if (valuesPropertyInfo != null)
			{
                return valuesPropertyInfo.GetValue(target);
			}

            MethodInfo methodValuesInfo = ReflectionUtility.GetMethod(target, attribute.MaxValueName);
			if (methodValuesInfo != null &&
                (methodValuesInfo.ReturnType == typeof(float) || methodValuesInfo.ReturnType == typeof(int)) &&
                methodValuesInfo.GetParameters().Length == 0)
			{
                return methodValuesInfo.Invoke(target, null);
			}

            return null;
		}
	}

    private object GetMinValue(SerializedProperty property, VariableBoundsRangeAttribute attribute)
    {
        if (string.IsNullOrEmpty(attribute.MinValueName))
        {
            return attribute.MinValue;
        }
        else
        {
            object target = PropertyUtility.GetTargetObjectWithProperty(property);

            FieldInfo valuesFieldInfo = ReflectionUtility.GetField(target, attribute.MinValueName);
            if (valuesFieldInfo != null)
            {
                return valuesFieldInfo.GetValue(target);
            }

            PropertyInfo valuesPropertyInfo = ReflectionUtility.GetProperty(target, attribute.MinValueName);
            if (valuesPropertyInfo != null)
            {
                return valuesPropertyInfo.GetValue(target);
            }

            MethodInfo methodValuesInfo = ReflectionUtility.GetMethod(target, attribute.MinValueName);
            if (methodValuesInfo != null &&
                (methodValuesInfo.ReturnType == typeof(float) || methodValuesInfo.ReturnType == typeof(int)) &&
                methodValuesInfo.GetParameters().Length == 0)
            {
                return methodValuesInfo.Invoke(target, null);
            }

            return null;
        }
    }

    private bool IsNumber(SerializedProperty property)
	{
        bool isNumber = property.propertyType == SerializedPropertyType.Float || property.propertyType == SerializedPropertyType.Integer;
        return isNumber;
	}
    private bool IsNumber(object obj)
    {
        return (obj is float) || (obj is int);
    }

	#region Copied from NaughtyAttributes
	public static float GetIndentLength(Rect sourceRect)
	{
		Rect indentRect = EditorGUI.IndentedRect(sourceRect);
		float indentLength = indentRect.x - sourceRect.x;

		return indentLength;
	}

	public void DrawDefaultPropertyAndHelpBox(Rect rect, SerializedProperty property, string message, MessageType messageType)
	{
		float indentLength = GetIndentLength(rect);
		Rect helpBoxRect = new Rect(
			rect.x + indentLength,
			rect.y,
			rect.width - indentLength,
			GetHelpBoxHeight());

		HelpBox(helpBoxRect, message, messageType);

		Rect propertyRect = new Rect(
			rect.x,
			rect.y + GetHelpBoxHeight(),
			rect.width,
			EditorGUI.GetPropertyHeight(property, includeChildren: true));

		EditorGUI.PropertyField(propertyRect, property, true);
	}

	public float GetHelpBoxHeight()
	{
		return EditorGUIUtility.singleLineHeight * 2.0f;
	}

	public void HelpBox(Rect rect, string message, MessageType type)
	{
		EditorGUI.HelpBox(rect, message, type);
	} 
	#endregion
}