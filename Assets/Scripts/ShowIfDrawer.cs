#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Reflection;

[CustomPropertyDrawer(typeof(ShowIfAttribute))]
public class ShowIfDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ShowIfAttribute showIf = (ShowIfAttribute)attribute;
        
        if (ShouldShow(property, showIf.conditionMethod))
        {
            EditorGUI.PropertyField(position, property, label, true);
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        ShowIfAttribute showIf = (ShowIfAttribute)attribute;
        
        if (ShouldShow(property, showIf.conditionMethod))
        {
            return EditorGUI.GetPropertyHeight(property, label);
        }
        return 0;
    }

    private bool ShouldShow(SerializedProperty property, string conditionMethod)
    {
        Object target = property.serializedObject.targetObject;
        System.Type type = target.GetType();

        while (type != null)
        {
            MethodInfo methodInfo = type.GetMethod(conditionMethod, 
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            if (methodInfo != null && methodInfo.ReturnType == typeof(bool))
            {
                return (bool)methodInfo.Invoke(target, null);
            }

            type = type.BaseType;
        }

        return true;
    }
}
#endif