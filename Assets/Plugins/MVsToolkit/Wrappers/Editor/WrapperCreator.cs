using System;
using UnityEditor;
using UnityEngine;
using MVsToolkit.Utilities;
using System.Linq;

public class WrapperCreator : EditorWindow
{
    string scriptName = "";
    string scriptPath = "App/Scripts/";
    string lastScriptPath = "";
    string assetPath = "App/Datas/";
    string lastAssetPath = "";

    string valueA = "";
    string valueB = "";
    string valueC = "";

    public bool isScriptPathModify = false;
    public bool isAssetPathModify = false;

    static float margin = 8;
    static float fieldWidth = 250;
    static float fieldHeight = 18;

    string[] wrapperPrefixMaskName = new string[]
    {
    // Unity event-like prefixes
    "On",
    "Before",
    "After",

    // Common action verbs
    "Set",
    "Get",
    "Try",
    "Do",
    "Can",
    "Has",
    "Is",

    // Lifecycle / state verbs
    "Init",
    "Initialize",
    "Load",
    "Save",
    "Reset",
    "Clear",
    "Update",
    "Refresh",

    // Creation / destruction
    "Create",
    "Build",
    "Generate",
    "Destroy",
    "Remove",
    "Add",

    // Execution / invocation
    "Invoke",
    "Call",
    "Execute",
    "Apply",

    // Checks / validation
    "Validate",
    "Check",
    "Ensure",

    // Unity-specific lifecycle names (souvent utilisés comme préfixes)
    "Awake",
    "Start",
    "Fixed",
    "Late",
    "Enable",
    "Disable"
    };

    WrapperType wrapperType;
    enum WrapperType { RSO, RSE, RSF}

    ProblemType problemType;
    enum ProblemType { None, NameMissing, NameExist, Type, ScriptPath, AssetPath}

    bool isDragging;
    Vector2 dragStartScreen;
    Vector2 windowStartPos;
    const float HeaderHeight = 22f;

    int dragControlId;

    public Rect newPosition;

    [MenuItem("Tools/MVsToolkit/WrapperCreator %#m")]
    [MenuItem("Assets/Create/MVsToolkit/WrapperCreator")]
    public static void ShowWindow()
    {
        var window = CreateInstance<WrapperCreator>();
        window.titleContent = new GUIContent("Wrapper Creator");

        window.isScriptPathModify = false;
        window.isAssetPathModify = false;

        float initialWidth = fieldWidth + margin * 2;
        float initialHeight = HeaderHeight + margin * 2 + fieldHeight * 7 + 8;

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

        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(margin);

        wrapperType = (WrapperType)GUILayout.Toolbar(
            (int)wrapperType,
            Enum.GetNames(typeof(WrapperType)),
            GUILayout.Height(fieldHeight)
        );

        GUILayout.Space(margin);
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(margin);


        GUILayout.Space(margin);
        scriptName = StringVariable("Name", scriptName);

        GetFirstWordIfSplitByUppercase(scriptName, out string output);
        lastScriptPath = NormalizePath(scriptPath + (isScriptPathModify || output == string.Empty ? "" : output + GetWrapperOutputType()));
        lastAssetPath = NormalizePath(assetPath + (isAssetPathModify || output == string.Empty ? "" : output + GetWrapperOutputType()));

        GUILayout.Space(margin);
        
        switch (wrapperType)
        {
            case WrapperType.RSO:
                valueA = StringVariable("Value", valueA);
                break;

            case WrapperType.RSE:
                valueA = StringVariable("Value A", valueA);
                valueB = StringVariable("Value B", valueB);
                valueC = StringVariable("Value C", valueC);
                break;

            case WrapperType.RSF:
                valueA = StringVariable("Return", valueA);
                GUILayout.Space(margin * .5f);
                valueB = StringVariable("Value A", valueB);
                valueC = StringVariable("Value B", valueC);
                break;
        }


        GUI.color = Color.red;
        switch (problemType)
        {
            case ProblemType.None:
                GUI.color = Color.white;
                GUILayout.Space(fieldHeight); 
                break;

            case ProblemType.NameMissing:
                GUILayout.Label("The name can't be empty");
                break;

            case ProblemType.NameExist:
                GUILayout.Label("The name given already exist");
                break;

            case ProblemType.Type:
                GUILayout.Label("The Type can't be empty");
                break;

            case ProblemType.ScriptPath:
                GUILayout.Label("The script path can't be empty");
                break;

            case ProblemType.AssetPath:
                GUILayout.Label("The asset path can't be empty");
                break;
        }
        GUI.color = Color.white;

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

        GUILayout.Space(margin);

        DrawValidationButton();
        GUILayout.Space(margin);

        float dynamicFields = GetFieldsHeight();

        float baseHeight = HeaderHeight
                           + margin * 2
                           + fieldHeight * 7
                           + dynamicFields;

        newPosition.height = baseHeight;

        position = newPosition;
        HandleShortcuts();
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
            Close();

        if (GUILayout.Button("Create"))
        {
            CreatesScript();
        }

        GUILayout.Space(margin);
        EditorGUILayout.EndHorizontal();
    }

    void CreatesScript()
    {
        string finalName = GetWrapperPrefixType() + scriptName;
        string fullPath = "Assets/" + lastScriptPath + finalName + ".cs";

        if (scriptName == "")
            problemType = ProblemType.NameMissing;
        else if (valueA == "" && wrapperType != WrapperType.RSE)
            problemType = ProblemType.Type;
        else if (scriptPath == "")
            problemType = ProblemType.ScriptPath;
        else if (assetPath == "")
            problemType = ProblemType.AssetPath;
        else if (System.IO.File.Exists(fullPath))
            problemType = ProblemType.NameExist;
        else
        {
            problemType = ProblemType.None;
            ScriptFileGeneration(finalName, fullPath);
            Close();
        }
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

    void HandleShortcuts()
    {
        Event e = Event.current;
        if (e.type == EventType.KeyDown)
        {
            if (e.keyCode == KeyCode.Return) CreatesScript();
            if (e.keyCode == KeyCode.Escape) Close();
            if (e.keyCode == KeyCode.LeftArrow) wrapperType = wrapperType.Previous();
            if (e.keyCode == KeyCode.RightArrow) wrapperType = wrapperType.Next();
        }
    }

    #region GenerationScript
    void ScriptFileGeneration(string finalName, string fullScriptPath)
    {
        EditorPrefs.SetString("CurrentWrapperNameCreated", finalName);
        EditorPrefs.SetString("CurrentWrapperPathCreated", assetPath);

        string content = GenerateWrapperScriptStr();
        string directory = System.IO.Path.GetDirectoryName(fullScriptPath);

        if (!System.IO.Directory.Exists(directory))
            System.IO.Directory.CreateDirectory(directory);

        System.IO.File.WriteAllText(fullScriptPath, content);
        AssetDatabase.Refresh();

    }

    string GenerateWrapperScriptStr()
    {
        switch (wrapperType)
        {
            case WrapperType.RSO:
                return GenerateRSOScript();
            case WrapperType.RSE:
                return GenerateRSEScript();
            case WrapperType.RSF:
                return GenerateRSFScript();
        }

        return GenerateRSOScript();
    }

    string GenerateRSOScript()
    {
        string finalName = GetWrapperPrefixType() + scriptName;

        GetFirstWordIfSplitByUppercase(scriptName, out string output);
        string soAssetPath = $"RSO/{output}/{scriptName}";

        string scriptContent = WrapperTemplate.rso
            .Replace("#SCRIPTNAME#", finalName)
            .Replace("RuntimeScriptableObject<>", $"RuntimeScriptableObject<{valueA}>")
            .Replace("#FILE_PATH#", soAssetPath);

        return scriptContent;
    }
    string GenerateRSEScript()
    {
        string finalName = GetWrapperPrefixType() + scriptName;

        GetFirstWordIfSplitByUppercase(scriptName, out string output);
        string soAssetPath = $"RSE/{output}/{scriptName}";

        bool noValues = valueA == "" && valueB == "" && valueC == "";

        string values = "";
        if (!noValues)
            values = ParseValues();

        string scriptContent = WrapperTemplate.rso
            .Replace("#SCRIPTNAME#", finalName)
            .Replace("RuntimeScriptableObject<>", $"RuntimeScriptableEvent{values}")
            .Replace("#FILE_PATH#", soAssetPath);

        return scriptContent;
    }
    string GenerateRSFScript()
    {
        string finalName = GetWrapperPrefixType() + scriptName;

        GetFirstWordIfSplitByUppercase(scriptName, out string output);
        string soAssetPath = $"RSF/{output}/{scriptName}";

        bool noValues = valueA == "" && valueB == "" && valueC == "";

        string values = "";
        if (!noValues)
            values = ParseValues();

        string scriptContent = WrapperTemplate.rso
            .Replace("#SCRIPTNAME#", finalName)
            .Replace("RuntimeScriptableObject<>", $"RuntimeScriptableFunction{values}")
            .Replace("#FILE_PATH#", soAssetPath);

        return scriptContent;
    }

    string ParseValues()
    {
        string values = "<";
        values += valueA;
        values += valueB == "" ? "" : ((valueA != "" ? ", " : "") + valueB);
        values += valueC == "" ? "" : ((valueA + valueB != "" ? ", " : "") + valueC);
        values += '>';

        return values;
    }
    #endregion

    #region Helper
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

    float GetFieldsHeight()
    {
        switch (wrapperType)
        {
            case WrapperType.RSO: return fieldHeight * 2 + margin;
            case WrapperType.RSE: return fieldHeight * 4 + margin;
            case WrapperType.RSF: return fieldHeight * 4 + margin * 1.5f;
        }
        return 1;
    }

    public void GetFirstWordIfSplitByUppercase(string input, out string output)
    {
        output = string.Empty;

        if (string.IsNullOrEmpty(input))
            return;

        var parts = System.Text.RegularExpressions.Regex
            .Split(input, @"(?=[A-Z])")
            .Where(p => !string.IsNullOrEmpty(p))
            .ToArray();

        if (parts.Length == 0)
            return;

        string first = parts[0];
        string firstLower = first.ToLower();

        bool isMasked = wrapperPrefixMaskName
            .Any(m => m.ToLower() == firstLower);

        if (isMasked)
        {
            if (parts.Length > 1)
            {
                output = parts[1];
                return;
            }

            output = string.Empty;
            return;
        }

        output = first;
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

    string GetWrapperOutputType()
    {
        return $"/{wrapperType}/";
    }
    string GetWrapperPrefixType()
    {
        return $"{wrapperType}_";
    }
    #endregion
}