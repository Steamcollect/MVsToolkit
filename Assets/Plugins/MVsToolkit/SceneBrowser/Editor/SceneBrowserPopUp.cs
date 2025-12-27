using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MVsToolkit.SceneBrowser
{
    public class SceneBrowserPopUp : PopupWindowContent
    {
        int searchHeight = 20;
        int buttonsSpacing = 4;
        int margin = 5;

        int maxContentHeight = 300;
        string searchTxt;

        public override void OnOpen()
        {
            SceneBrowserContent.RefreshScenesList();
        }

        public override void OnGUI(Rect rect)
        {
            GUILayout.BeginHorizontal();

            Rect searchRect = new Rect(rect.x + margin, rect.y + margin + 1, rect.width - (margin + (searchHeight * 1.4f + buttonsSpacing) * 2 + buttonsSpacing), searchHeight);

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

            if (GUI.Button(buttonRect, content)) // Create new scene
            {
                SceneBrowserContent.CreateNewScene(searchTxt);
                editorWindow.Close();
            }

            buttonRect = new Rect(
                rect.x + rect.width - (margin + searchHeight * 1.4f),
                rect.y + margin,
                searchHeight * 1.4f,
                searchHeight * .9f
            );

            content = new GUIContent(EditorGUIUtility.IconContent("ScaleTool On"))
            { tooltip = "Open full window" };
            if (GUI.Button(buttonRect, content)) // Open full window
            {
                EditorWindow.GetWindow<SceneBrowserWindow>("Scene Browser");
            }

            GUILayout.EndHorizontal();

            float contentY = searchHeight + 12;
            float contentHeight = Mathf.Min(maxContentHeight, rect.height - contentY);

            Rect contentR = new Rect(
                rect.x,
                rect.y + contentY,
                rect.width,
                contentHeight
            );

            SceneBrowserContent.DrawContent(contentR, searchTxt, maxContentHeight);
        }


        public override Vector2 GetWindowSize()
        {
            Vector2 contentSize = SceneBrowserContent.GetWindowSize();
            return new Vector2(contentSize.x, Mathf.Min(maxContentHeight, contentSize.y) + searchHeight + 16);
        }
    }
}