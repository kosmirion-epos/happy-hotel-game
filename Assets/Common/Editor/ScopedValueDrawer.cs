using UnityEditor;
using UnityEngine;

/// <summary>
/// Custom <see cref="PropertyDrawer"/> for <see cref="ScopedValue{T}"/>
/// </summary>
[CustomPropertyDrawer(typeof(ScopedValue<>))]
public class ScopedValueDrawer : PropertyDrawer
{
    private enum Scope
    {
        Local = 0,
        Global = 1
    }

    private enum RuntimeModifiability
    {
        Constant = 0,
        Variable = 1
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var isGlobal = property.FindPropertyRelative("isGlobal");

        if (isGlobal.boolValue)
        {
            return EditorGUI.GetPropertyHeight(isGlobal) +
                EditorGUIUtility.standardVerticalSpacing +
                EditorGUI.GetPropertyHeight(property.FindPropertyRelative("global"));
        }

        return Mathf.Max(
                EditorGUI.GetPropertyHeight(isGlobal),
                EditorGUI.GetPropertyHeight(property.FindPropertyRelative("isRuntimeConstant"))
            ) + EditorGUIUtility.standardVerticalSpacing +
            EditorGUI.GetPropertyHeight(property.FindPropertyRelative("local"));
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var isGlobal = property.FindPropertyRelative("isGlobal");

        //TODO add margined box around property

        EditorGUI.BeginProperty(position, label, property);

        var afterLabel = EditorGUI.PrefixLabel(position, label);

        isGlobal.boolValue = Scope.Global ==
            (Scope) EditorGUI.EnumPopup(
                new Rect(
                    afterLabel.x,
                    afterLabel.y,
                    isGlobal.boolValue ? afterLabel.width : afterLabel.width / 2,
                    EditorGUIUtility.singleLineHeight
                ),
                isGlobal.boolValue ? Scope.Global : Scope.Local
            );

        if (isGlobal.boolValue)
        {
            var global = property.FindPropertyRelative("global");
            ++EditorGUI.indentLevel;
            EditorGUI.PropertyField(
                new Rect(
                    position.x,
                    afterLabel.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing,
                    position.width,
                    EditorGUI.GetPropertyHeight(global)
                ),
                global
            );
            --EditorGUI.indentLevel;
        }
        else
        {
            var isRuntimeConstant = property.FindPropertyRelative("isRuntimeConstant");
            isRuntimeConstant.boolValue = RuntimeModifiability.Constant ==
            (RuntimeModifiability)EditorGUI.EnumPopup(
                new Rect(
                    afterLabel.x + afterLabel.width / 2,
                    afterLabel.y,
                    afterLabel.width / 2,
                    EditorGUIUtility.singleLineHeight
                ),
                isRuntimeConstant.boolValue ? RuntimeModifiability.Constant : RuntimeModifiability.Variable
            );

            var local = property.FindPropertyRelative("local");
            ++EditorGUI.indentLevel;
            EditorGUI.PropertyField(
                new Rect(
                    position.x,
                    afterLabel.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing,
                    position.width,
                    EditorGUI.GetPropertyHeight(local)
                ),
                local,
                GUIContent.none
            );
            --EditorGUI.indentLevel;
        }

        EditorGUI.EndProperty();
    }
}