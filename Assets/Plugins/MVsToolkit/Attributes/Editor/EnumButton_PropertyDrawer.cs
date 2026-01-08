using UnityEditor;
using UnityEngine;

namespace MVsToolkit.Dev
{
    [CustomPropertyDrawer(typeof(EnumButtonAttribute))]
    public class EnumButton_PropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.Enum)
            {
                EditorGUI.LabelField(position, "Error : EnumButton only works on enums");
                return;
            }

            EnumButtonAttribute attr = (EnumButtonAttribute)attribute;

            EditorGUI.BeginProperty(position, label, property);

            // --- Label handling ---
            if (attr.ShowVariableName)
            {
                // Use PrefixLabel normally
                position = EditorGUI.PrefixLabel(position, label);
            }
            else
            {
                // Remove label entirely
                // Just indent a bit to align with other fields
                float indent = EditorGUI.indentLevel * 15f;
                position = new Rect(position.x + indent, position.y, position.width - indent, position.height);
            }

            // --- Enum buttons ---
            string[] names = property.enumDisplayNames;
            int currentIndex = property.enumValueIndex;

            float buttonWidth = position.width / names.Length;
            Rect buttonRect = new Rect(position.x, position.y, buttonWidth, position.height);

            EditorGUI.BeginChangeCheck();

            for (int i = 0; i < names.Length; i++)
            {
                bool selected = (i == currentIndex);

                if (GUI.Toggle(buttonRect, selected, names[i], EditorStyles.toolbarButton))
                    currentIndex = i;

                buttonRect.x += buttonWidth;
            }

            if (EditorGUI.EndChangeCheck())
                property.enumValueIndex = currentIndex;

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight + 4;
        }
    }
}
