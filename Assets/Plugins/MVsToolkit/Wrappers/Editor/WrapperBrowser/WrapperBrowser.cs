using MVsToolkit.SceneBrowser;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WrapperBrowser : EditorWindow
{
    int margin = 5;
    int fieldHeight = 20;

    string basePath = "Assets/App/";
    string searchTxt = "";

    List<ScriptableObject> wrappers = new();

    [MenuItem("Tools/MVsToolkit/Wrapper Browser")]
    public static void Open()
    {
        GetWindow<WrapperBrowser>("Wrapper Browser");
    }

    private void OnGUI()
    {
        Rect rect = new Rect(0, 0, position.width, position.height);

        GUILayout.Space(margin);
        GUILayout.BeginHorizontal();
        GUILayout.Space(margin);

        GUILayout.Label(basePath, GUILayout.Height(fieldHeight));

        GUIContent content = new GUIContent(EditorGUIUtility.IconContent("Folder Icon"))
        { tooltip = "Select base folder" };

        if (GUILayout.Button(content, GUILayout.Width(fieldHeight), GUILayout.Height(fieldHeight)))
        {

        }

        GUILayout.Space(margin);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Space(margin);

        GUIStyle searchStyle = GUI.skin.FindStyle("SearchTextField");
        searchTxt = GUILayout.TextField(searchTxt, searchStyle, GUILayout.Height(fieldHeight));

        content = new GUIContent(EditorGUIUtility.IconContent("Refresh"))
        { tooltip = "Refresh wrapper list" };

        if (GUILayout.Button(content, GUILayout.Width(fieldHeight), GUILayout.Height(fieldHeight)))
        {
            RefreshList();
        }

        GUILayout.Space(margin);
        GUILayout.EndHorizontal();
    }

    void RefreshList()
    {

    }
}