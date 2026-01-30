using UnityEditor;
using UnityEngine;

public class WrapperCreator : EditorWindow
{
    private bool isDragging;
    private Vector2 dragStartScreen;
    private Vector2 windowStartPos;
    private const float HeaderHeight = 22f;

    private int dragControlId;

    public Rect newPosition;

    [MenuItem("Tools/MVsToolkit/WrapperCreator")]
    [MenuItem("Assets/Create/MVsToolkit/WrapperCreator")]
    public static void ShowWindow()
    {
        var window = CreateInstance<WrapperCreator>();
        window.titleContent = new GUIContent("Wrapper Creator");

        float initialWidth = 400;
        float initialHeight = 200;

        Rect rect = new Rect(
            (Screen.currentResolution.width - initialWidth) / 2f,
            (Screen.currentResolution.height - initialHeight) / 2f,
            initialWidth,
            initialHeight
        );

        window.newPosition = rect;

        window.minSize = new Vector2(300, 150);
        window.maxSize = new Vector2(500, 800);

        window.ShowPopup();
    }

    private void OnGUI()
    {
        DrawHeaderWithCloseAndDrag();
        DrawRedBox();

        position = newPosition;
    }

    private void DrawRedBox()
    {
        GUILayout.Space(10);
        Rect box = GUILayoutUtility.GetRect(200, 200, GUILayout.ExpandWidth(false));
        EditorGUI.DrawRect(box, Color.red);
    }

    private void DrawHeaderWithCloseAndDrag()
    {
        Rect headerRect = new Rect(0, 0, position.width, HeaderHeight);
        EditorGUI.DrawRect(headerRect, new Color(0.18f, 0.18f, 0.18f));

        GUI.Label(new Rect(5, 2, position.width - 40, 20), "    Wrapper Creator", EditorStyles.boldLabel);

        // Close button
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
}