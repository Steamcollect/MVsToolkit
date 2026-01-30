using System;
using UnityEditor;
using UnityEngine;

public class WrapperCreator : EditorWindow
{
    string scriptName;
    string valueType;
    string scriptPath = "App/Scripts/Wrappers/";
    string lastScriptPath;
    string assetPath = "App/Datas/";
    string lastAssetPath;

    public bool isScriptPathModify = false;
    public bool isAssetPathModify = false;

    static float margin = 8;
    static float fieldWidth = 250;
    static float fieldHeight = 18;
    static float spacing = 15;

    WrapperType wrapperType;
    enum WrapperType { RSO, RSE, RSF}

    bool isDragging;
    Vector2 dragStartScreen;
    Vector2 windowStartPos;
    const float HeaderHeight = 22f;

    int dragControlId;

    public Rect newPosition;

    [MenuItem("Tools/MVsToolkit/WrapperCreator")]
    [MenuItem("Assets/Create/MVsToolkit/WrapperCreator")]
    public static void ShowWindow()
    {
        var window = CreateInstance<WrapperCreator>();
        window.titleContent = new GUIContent("Wrapper Creator");

        window.isScriptPathModify = false;
        window.isAssetPathModify = false;

        float initialWidth = fieldWidth + margin * 2;
        float initialHeight = HeaderHeight + margin * 2 + fieldHeight * 5 + spacing * 2 + 8;

        Rect rect = new Rect(
            (Screen.currentResolution.width - initialWidth) / 2f,
            (Screen.currentResolution.height - initialHeight) / 2f,
            initialWidth,
            initialHeight
        );

        window.newPosition = rect;
        window.ShowPopup();
    }

    private void OnGUI()
    {
        DrawHeaderWithCloseAndDrag();

        GUILayout.Space(margin);
        scriptName = StringVariable("Name", scriptName);

        GetFirstWordIfSplitByUppercase(scriptName, out string output);
        lastScriptPath = NormalizePath(scriptPath + (isScriptPathModify || output == string.Empty ? "" : output));
        lastAssetPath = NormalizePath(assetPath + (isAssetPathModify || output == string.Empty ? "" : output));

        valueType = StringVariable("Type", valueType);

        GUILayout.Space(spacing);

        DrawPathButton(DisplayPath(lastScriptPath), "Folder Icon", () =>
        {
            string path = GetFolderPath("Select Script Folder", scriptPath);
            if (path != scriptPath)
            {
                isScriptPathModify = true;
            }
            scriptPath = NormalizePath(path);
        });
        DrawPathButton(DisplayPath(lastAssetPath), "Folder Icon", () =>
        {
            string path = GetFolderPath("Select Asset Folder", assetPath);
            if (path != assetPath)
            {
                isAssetPathModify = true;
            }
            assetPath = NormalizePath(path);
        });

        GUILayout.Space(spacing);

        DrawValidationButton();
        GUILayout.Space(margin);

        position = newPosition;
    }

    string StringVariable(string label, string value)
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(margin);

        GUILayout.Label(label, GUILayout.Width(fieldWidth * .3f), GUILayout.Height(fieldHeight));
        value = GUILayout.TextField(value, GUILayout.Width(fieldWidth * .7f), GUILayout.Height(fieldHeight));

        GUILayout.Space(margin);
        EditorGUILayout.EndHorizontal();

        return value;
    }

    void DrawPathButton(string label, string iconName, Action callback)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Space(margin);

        GUILayout.Label(label, GUILayout.Height(fieldHeight));
        if (GUILayout.Button(EditorGUIUtility.IconContent(iconName).image, GUILayout.Width(fieldHeight), GUILayout.Height(fieldHeight)))
        {
            callback.Invoke();
        }
        
        GUILayout.Space(margin);
        GUILayout.EndHorizontal();
    }

    void DrawValidationButton()
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(margin);

        if (GUILayout.Button("Cancel"))
        {
            Close();
            return;
        }
        GUILayout.Button("Create");

        GUILayout.Space(margin);
        EditorGUILayout.EndHorizontal();
    }

    string GetFolderPath(string label, string startingPath)
    {
        string absolutePath = EditorUtility.OpenFolderPanel(label, "Assets", "");

        if (!string.IsNullOrEmpty(absolutePath))
        {
            string projectPath = Application.dataPath.Substring(0, Application.dataPath.Length - "Assets".Length);

            string relative = absolutePath.Replace(projectPath, "");
            return NormalizePath(relative);
        }

        return startingPath;
    }

    private void DrawHeaderWithCloseAndDrag()
    {
        Rect headerRect = new Rect(0, 0, position.width, HeaderHeight);
        EditorGUI.DrawRect(headerRect, new Color(0.18f, 0.18f, 0.18f));

        GUI.Label(new Rect(5, 2, position.width - 40, 20), "    Wrapper Creator", EditorStyles.boldLabel);

        Rect closeRect = new Rect(position.width - 22, 2, 18, 18);
        var closeStyle = new GUIStyle(EditorStyles.boldLabel)
        {
            alignment = TextAnchor.MiddleCenter,
            fontSize = 16
        };

        EditorGUIUtility.AddCursorRect(closeRect, MouseCursor.Link);
        if (GUI.Button(closeRect, "×", closeStyle))
        {
            Close();
            GUIUtility.ExitGUI();
        }

        HandleDrag(closeRect, headerRect);

        GUILayout.Space(HeaderHeight);
    }

    void HandleDrag(Rect closeRect, Rect headerRect)
    {
        dragControlId = GUIUtility.GetControlID(FocusType.Passive);
        Event e = Event.current;
        EventType typeForCtrl = e.GetTypeForControl(dragControlId);

        if (!closeRect.Contains(e.mousePosition))
            EditorGUIUtility.AddCursorRect(headerRect, MouseCursor.MoveArrow);

        switch (typeForCtrl)
        {
            case EventType.MouseDown:
                if (headerRect.Contains(e.mousePosition) && !closeRect.Contains(e.mousePosition) && e.button == 0)
                {
                    GUIUtility.hotControl = dragControlId;
                    isDragging = true;

                    dragStartScreen = GUIUtility.GUIToScreenPoint(e.mousePosition);
                    windowStartPos = position.position;
                    e.Use();
                }
                break;

            case EventType.MouseDrag:
                if (GUIUtility.hotControl == dragControlId && isDragging)
                {
                    Vector2 curScreen = GUIUtility.GUIToScreenPoint(e.mousePosition);
                    Vector2 delta = curScreen - dragStartScreen;
                    newPosition = new Rect(windowStartPos.x + delta.x, windowStartPos.y + delta.y, position.width, position.height);
                    e.Use();
                }
                break;

            case EventType.MouseUp:
                if (GUIUtility.hotControl == dragControlId)
                {
                    GUIUtility.hotControl = 0;
                    isDragging = false;
                    e.Use();
                }
                break;
        }
    }

    public static void GetFirstWordIfSplitByUppercase(string input, out string output)
    {
        output = string.Empty;

        if (string.IsNullOrEmpty(input))
            return;

        var parts = System.Text.RegularExpressions.Regex
            .Split(input, @"(?=[A-Z])");

        if (parts.Length <= 1)
            return;

        foreach (var p in parts)
        {
            if (!string.IsNullOrEmpty(p))
            {
                output = p;
                return;
            }
        }
    }
    private static string NormalizePath(string path)
    {
        if (string.IsNullOrEmpty(path))
            return "";

        path = path.Replace("\\", "/");

        // Supprime les doubles slash
        while (path.Contains("//"))
            path = path.Replace("//", "/");

        // Ajoute un slash final si absent
        if (!path.EndsWith("/"))
            path += "/";

        return path;
    }
    private static string DisplayPath(string fullPath)
    {
        if (fullPath.StartsWith("Assets/"))
            return fullPath.Substring("Assets/".Length);
        return fullPath;
    }
}