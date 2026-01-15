using UnityEditor;
using UnityEngine;

namespace MVsToolkit.Preferences
{
    public static class MVsToolkitPreferencesDrawer
    {
        static GUIStyle _sectionStyle;
        static GUIStyle _titleStyle;

        static MVsToolkitPreferencesValues values;

        public static void Draw(MVsToolkitPreferencesValues v)
        {
            values = v;

            if (_sectionStyle == null)
            {
                _sectionStyle = new GUIStyle(EditorStyles.helpBox)
                {
                    padding = new RectOffset(10, 10, 10, 10)
                };

                _titleStyle = new GUIStyle(EditorStyles.boldLabel)
                {
                    fontSize = 14,
                    alignment = TextAnchor.UpperLeft
                };
            }

            DrawHierarchy();
            GUILayout.Space(12);
            DrawOthers();
        }

        // ---------------------------------------------------------
        //  UTILITAIRE : ColorField basé sur string hex
        // ---------------------------------------------------------
        static string DrawColorField(string label, string hex)
        {
            Color parsed;
            if (!ColorUtility.TryParseHtmlString(hex, out parsed))
                parsed = Color.white;

            Color newColor = EditorGUILayout.ColorField(label, parsed);

            // Retourne un hex propre (#RRGGBB)
            return "#" + ColorUtility.ToHtmlStringRGB(newColor);
        }

        // ---------------------------------------------------------
        //  HIERARCHY
        // ---------------------------------------------------------
        static void DrawHierarchy()
        {
            GUILayout.BeginVertical(_sectionStyle);
            {
                GUILayout.Label("Hierarchy", _titleStyle);
                GUILayout.Space(6);

                GUILayout.BeginHorizontal();
                GUILayout.BeginVertical();

                GUILayout.BeginVertical(EditorStyles.helpBox);
                values.DrawFolderIcon = EditorGUILayout.Toggle("Draw Folder Icon", values.DrawFolderIcon);
                values.DrawFirstComponentIcon = EditorGUILayout.Toggle("Draw First Component Icon", values.DrawFirstComponentIcon);
                values.DrawComponentsIcon = EditorGUILayout.Toggle("Draw Components Icon", values.DrawComponentsIcon);
                GUILayout.EndVertical();

                GUILayout.BeginVertical(EditorStyles.helpBox);
                values.DrawZebraMod = EditorGUILayout.Toggle("Draw Zebra Mod", values.DrawZebraMod);
                values.DrawChildLines = EditorGUILayout.Toggle("Draw Child Lines", values.DrawChildLines);
                GUILayout.EndVertical();

                GUILayout.EndVertical();

                GUILayout.BeginVertical(EditorStyles.helpBox);
                values.ZebraModColor = DrawColorField("Zebra Mod Color", values.ZebraModColor);

                GUILayout.Space(10);
                values.PrefabColor = DrawColorField("Prefab Color", values.PrefabColor);
                values.MissingPrefabColor = DrawColorField("Missing Prefab Color", values.MissingPrefabColor);
                GUILayout.EndVertical();

                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }

        // ---------------------------------------------------------
        //  OTHERS
        // ---------------------------------------------------------
        static void DrawOthers()
        {
            GUILayout.BeginHorizontal(_sectionStyle);
            {
                GUILayout.BeginVertical();
                GUILayout.Label("Others", _titleStyle);
                GUILayout.Space(6);

                GUILayout.BeginVertical(EditorStyles.helpBox);
                values.CleanComponentHeader = EditorGUILayout.Toggle("Clean Component Header", values.CleanComponentHeader);
                GUILayout.EndVertical();

                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();
        }
    }
}