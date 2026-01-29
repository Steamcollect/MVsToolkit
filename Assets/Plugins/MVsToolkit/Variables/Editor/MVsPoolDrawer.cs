using UnityEditor;
using UnityEngine;

namespace MVsToolkit.Pool
{
    [CustomPropertyDrawer(typeof(PoolObject<>))]
    public class MVsPoolDrawer : PropertyDrawer
    {
        // Visual padding inside the helpBox
        private readonly RectOffset _boxPadding = new RectOffset(16, 8, 4, 4);
        private GUIStyle _helpBox;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            float lineH = EditorGUIUtility.singleLineHeight;
            float vsp = EditorGUIUtility.standardVerticalSpacing;

            // Ensure style
            if (_helpBox == null)
            {
                _helpBox = new GUIStyle(EditorStyles.helpBox)
                {
                    margin = new RectOffset(0, 0, 0, 0),
                    padding = new RectOffset(0, 0, 0, 0)
                };
            }

            float totalHeight = GetPropertyHeight(property, label);

            Rect boxRect = new Rect(position.x, position.y, position.width, totalHeight);
            GUI.Box(boxRect, GUIContent.none, _helpBox);

            Rect headerRect = new Rect(boxRect.x + _boxPadding.left, boxRect.y + _boxPadding.top, boxRect.width - _boxPadding.horizontal, lineH);
            property.isExpanded = EditorGUI.Foldout(headerRect, property.isExpanded, label, true);

            float y = headerRect.y + lineH + vsp;

            // Header foldout

            // If collapsed → draw prefab field inline
            if (!property.isExpanded)
            {
                SerializedProperty prefabProp = property.FindPropertyRelative("prefab");

                if (prefabProp != null)
                {
                    float labelWidth = EditorGUIUtility.labelWidth;
                    Rect labelRect = new Rect(headerRect.x, headerRect.y, labelWidth, headerRect.height);
                    Rect fieldRect = new Rect(headerRect.x + labelWidth - 14, headerRect.y, headerRect.width - labelWidth + 18, headerRect.height);

                    EditorGUI.PropertyField(fieldRect, prefabProp, GUIContent.none);
                }

                EditorGUI.EndProperty();
                return; // stop drawing the expanded content
            }


            if (property.isExpanded)
            {
                EditorGUI.indentLevel++;

                SerializedProperty prefabProp = property.FindPropertyRelative("prefab");
                SerializedProperty setParentProp = property.FindPropertyRelative("m_SetParent");
                SerializedProperty parentProp = property.FindPropertyRelative("parent");
                SerializedProperty limitSizeProp = property.FindPropertyRelative("m_LimitSize");
                SerializedProperty maxSizeProp = property.FindPropertyRelative("MaximumPoolSize");
                SerializedProperty prewarmProp = property.FindPropertyRelative("m_Prewarm");
                SerializedProperty prewarmCountProp = property.FindPropertyRelative("PrewarmCount");

                // Prefab row
                if (prefabProp != null)
                {
                    Rect rowRect = new Rect(position.x - 16 + _boxPadding.left, y, position.width - _boxPadding.horizontal, lineH);
                    rowRect = EditorGUI.IndentedRect(rowRect);
                    EditorGUI.LabelField(new Rect(rowRect.x, rowRect.y, 96, lineH), "Prefab");

                    Rect fieldRect = new Rect(rowRect.x + 124, rowRect.y, rowRect.width - 108, lineH);
                    EditorGUI.PropertyField(fieldRect, prefabProp, GUIContent.none);

                    y += lineH + vsp + 5;
                }

                // Toggles + fields rows
                DrawToggleAndFieldSameLine(ref y, position, lineH, vsp, "Set Parent", setParentProp, parentProp);
                DrawToggleAndFieldSameLine(ref y, position, lineH, vsp, "Limit Count", limitSizeProp, maxSizeProp);
                DrawToggleAndFieldSameLine(ref y, position, lineH, vsp, "Prewarm", prewarmProp, prewarmCountProp);

                EditorGUI.indentLevel--;
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = EditorGUIUtility.singleLineHeight; // header
            float vsp = EditorGUIUtility.standardVerticalSpacing;

            // Add spacing below header
            height += vsp;

            if (property.isExpanded)
            {
                var prefabProp = property.FindPropertyRelative("prefab");
                if (prefabProp != null)
                    height += EditorGUIUtility.singleLineHeight + vsp + 5;

                height += (EditorGUIUtility.singleLineHeight + vsp) * 3f;

                // Only add padding when expanded
                height += _boxPadding.vertical;
            }
            else height += 6;

                return height;
        }


        private void DrawToggleAndFieldSameLine(ref float y, Rect position, float lineH, float vsp, string label, SerializedProperty toggleProp, SerializedProperty fieldProp)
        {
            Rect rowRect = new Rect(position.x - 16 + _boxPadding.left, y, position.width - _boxPadding.horizontal, lineH);
            rowRect = EditorGUI.IndentedRect(rowRect);

            // Left: static label + checkbox right next to it
            var labelRect = new Rect(rowRect.x, rowRect.y, 96, lineH);
            EditorGUI.LabelField(labelRect, label);

            var toggleRect = new Rect(labelRect.xMax, rowRect.y, 16, lineH);
            if (toggleProp != null)
            {
                bool newVal = EditorGUI.Toggle(toggleRect, GUIContent.none, toggleProp.boolValue);
                if (newVal != toggleProp.boolValue)
                    toggleProp.boolValue = newVal;
            }

            // Right: field starts exactly after the toggle
            bool enabled = toggleProp != null && toggleProp.boolValue && fieldProp != null;
            using (new EditorGUI.DisabledScope(!enabled))
            {
                var fieldRect = new Rect(toggleRect.xMax + 12, rowRect.y, rowRect.width - (toggleRect.xMax - rowRect.x - 4), lineH);
                if (fieldProp != null)
                    EditorGUI.PropertyField(fieldRect, fieldProp, GUIContent.none);
            }

            y += lineH + vsp;
        }
    }
}