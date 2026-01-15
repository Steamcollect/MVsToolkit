using UnityEditor;
using UnityEngine;

namespace MVsToolkit.SceneBrowser
{
    public class SceneBrowserWindow : EditorWindow
    {
        int searchHeight = 20;
        int buttonsSpacing = 4;
        int margin = 5;

        string searchTxt;

        [MenuItem("Tools/MVsToolkit/Scene Browser")]
        public static void ShowWindow()
        {
            GetWindow<SceneBrowserWindow>("Scene Browser");
        }

        private void OnEnable()
        {
            SceneBrowserContent.RefreshScenesList();
        }

        private void OnGUI()
        {
            Rect rect = new Rect(0, 0, position.width, position.height);

            GUILayout.BeginHorizontal();

            Rect searchRect = new Rect(
                rect.x + margin,
                rect.y + margin + 1,
                rect.width - (margin + (searchHeight * 1.4f + buttonsSpacing) * 2 + buttonsSpacing),
                searchHeight
            );

            GUIStyle searchStyle = GUI.skin.FindStyle("SearchTextField");
            searchTxt = GUI.TextField(searchRect, searchTxt, searchStyle);

            Rect buttonRect = new Rect(
                rect.x + rect.width - (margin + (searchHeight * 1.4f) * 2 + buttonsSpacing),
                rect.y + margin,
                searchHeight * 1.4f,
                searchHeight * .9f
            );

            GUIContent content = new GUIContent(EditorGUIUtility.IconContent("d_Toolbar Plus More"))
            { tooltip = "Create new scene" };

            if (GUI.Button(buttonRect, content))
            {
                string nameCopy = searchTxt;
                EditorApplication.delayCall += () =>
                {
                    SceneBrowserContent.CreateNewScene(nameCopy);
                    SceneBrowserContent.RefreshScenesList();
                };
            }

            buttonRect = new Rect(
                rect.x + rect.width - (margin + searchHeight * 1.4f),
                rect.y + margin,
                searchHeight * 1.4f,
                searchHeight * .9f
            );

            content = new GUIContent(EditorGUIUtility.IconContent("Refresh"))
            { tooltip = "Refresh scenes list" };

            if (GUI.Button(buttonRect, content))
            {
                SceneBrowserContent.RefreshScenesList();
            }

            GUILayout.EndHorizontal();

            float contentY = searchHeight + 12;
            float contentHeight = position.height - contentY;

            Rect contentR = new Rect(
                rect.x,
                rect.y + contentY,
                rect.width,
                contentHeight
            );

            SceneBrowserContent.DrawContent(contentR, searchTxt, contentHeight);
        }
    }
}