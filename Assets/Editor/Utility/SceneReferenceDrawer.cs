using UnityEditor;
using UnityEngine;

//TODO make it worked inside ScopedValue & GlobalValue
//[CustomPropertyDrawer(typeof(SceneReference))]
public class SceneReferenceDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) =>
        EditorGUI.GetPropertyHeight(property.FindPropertyRelative("sceneID"));

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var sceneID = property.FindPropertyRelative("sceneID");

        EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.PrefixLabel(position, label);
        EditorGUI.PropertyField(position, sceneID, GUIContent.none);
        EditorGUI.EndProperty();
    }
}