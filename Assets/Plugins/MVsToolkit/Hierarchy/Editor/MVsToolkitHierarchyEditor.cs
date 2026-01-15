using System.Linq;
using System.Reflection;
using MVsToolkit.Preferences;
using UnityEditor;
using UnityEngine;

namespace MVsToolkit.Hierarchy
{
    [InitializeOnLoad]
    public static class MVsToolkitHierarchyEditor
    {
        static int iconSize = 16;
        static float compIconSizeMultiplier = 0.75f;
        static int iconXOffset = 4;
        static int iconsSpacing = 0;

        static GUIStyle iconStyle;

        static Texture icon;
        static GUIContent content;

        // --- GameObject infos
        static GameObject go;
        static Color bgColor;

        static bool isSelected;
        static int parentCount;

        static bool isHover;
        static bool activeSelf;
        static bool isGameObjectExpand;

        static GameObjectState state;
        // ---

        static Color gray6 = new Color(.6f, .6f, .6f, 1);
        static Color gray7 = new Color(.7f, .7f, .7f, 1);

        static MVsToolkitHierarchyEditor()
        {
            EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyGUI;
        }

        #region Draw
        static void OnHierarchyGUI(int instanceID, Rect rect)
        {
            Object obj = EditorUtility.InstanceIDToObject(instanceID); // Check if the object is a GameObject
            if (obj == null) return;
            
            go = obj as GameObject;
            Event e = Event.current;

            InitializeStyles();

            // --- Setup GameObject infos
            isSelected = Selection.instanceIDs.Contains(instanceID);
            parentCount = GetParentCount(go);

            isHover = rect.Contains(e.mousePosition);
            activeSelf = GetHierarchyActiveSelf(go);
            isGameObjectExpand = IsGameObjectExpand(instanceID);
            // ---

            GUI.color = Color.white;

            DrawBackground(rect);
            DrawChildLine(rect);
            DrawSetActiveToggle(rect, e);

            DrawComponentsIcon(rect, go, out Component[] comps, out bool haveMissingComponent);

            state = GetState(haveMissingComponent);

            DrawGameObjectName(rect);
            DrawGameObjectIcon(rect, comps.Length > 1 ? comps[1] : null);
            
            GUI.color = Color.white;
        }

        static void DrawBackground(Rect rect)
        {
            bgColor = HierarchyBackgroundColor(((int)rect.y / (int)rect.height) % 2 == 1);
            if (isHover) bgColor = HierarchyHoverColor;
            if (isSelected) bgColor = HierarchySelectionColor;
            EditorGUI.DrawRect(new Rect(rect.x - 21 - (14 * parentCount), rect.y, rect.width + 22 + (14 * parentCount), rect.height), bgColor);
        }

        static void DrawGameObjectName(Rect rect)
        {
            if (isSelected) GUI.color = Color.white;
            else
            {
                switch(state)
                {
                    case GameObjectState.Normal:
                        GUI.color = activeSelf ? Color.white : gray6;
                        break;

                    case GameObjectState.ErrorFromMissingPrefab:
                    case GameObjectState.ErrorFromMissingComponent:
                        GUI.color = StrToColor(MVsToolkitPreferences.Values.MissingPrefabColor) * (activeSelf ? Color.white : gray7);
                        break;

                    case GameObjectState.Prefab:
                    case GameObjectState.PrefabVariant:
                    case GameObjectState.PrefabChildren:
                        GUI.color = StrToColor(MVsToolkitPreferences.Values.PrefabColor) * (activeSelf ? Color.white : gray7);
                        break;
                }
            }

            EditorGUI.LabelField(new Rect(rect.x + iconSize + iconXOffset + 1, rect.y, rect.width + 44, rect.height), go.name);
        }

        static void DrawGameObjectIcon(Rect rect, Component comp)
        {
            GUI.color = !isSelected && !activeSelf ? Color.gray : Color.white;
            
            Rect iconRect = new Rect(rect.x - 1, rect.y, iconSize, iconSize);

            switch(state)
            {
                case GameObjectState.ErrorFromMissingPrefab:
                    DrawIcon(iconRect, "console.erroricon", "Missing Prefab");
                    return;

                case GameObjectState.ErrorFromMissingComponent:
                    DrawIcon(iconRect, "console.erroricon", "Missing Component");
                    return;

                case GameObjectState.Normal:
                case GameObjectState.PrefabChildren:
                    if (MVsToolkitPreferences.Values.DrawFolderIcon && comp == null)
                    {
                        if(go.transform.childCount == 0)
                            DrawIcon(iconRect, "FolderEmpty Icon");
                        else if (isGameObjectExpand)
                            DrawIcon(iconRect, "FolderOpened Icon");
                        else
                            DrawIcon(iconRect, "Folder Icon");
                        
                        return;
                    }
                    DrawIcon(iconRect, "GameObject Icon");
                    break;

                case GameObjectState.Prefab:
                    DrawIcon(iconRect, "Prefab Icon");
                    break;
                
                case GameObjectState.PrefabVariant:
                    DrawIcon(iconRect, "PrefabVariant Icon");
                    break;
            }

            if (MVsToolkitPreferences.Values.DrawFirstComponentIcon && comp != null)
            {
                icon = EditorGUIUtility.ObjectContent(null, comp.GetType()).image as Texture2D;
                if (icon == null) return;

                Rect compRect = new Rect(
                    iconRect.x + iconRect.width - iconRect.width * compIconSizeMultiplier + iconXOffset,
                    iconRect.y + iconRect.height - iconRect.height * compIconSizeMultiplier,
                    iconRect.width * compIconSizeMultiplier,
                    iconRect.height * compIconSizeMultiplier
                );

                GUI.color = Color.white;
                EditorGUI.DrawRect(compRect, bgColor);
                GUI.color = !isSelected && !activeSelf ? Color.gray : Color.white;

                compRect.x += 1;
                DrawIcon(compRect, icon);
            }

            GUI.color = Color.white;
        }

        static void DrawComponentsIcon(Rect rect, GameObject go, out Component[] comps, out bool haveMissingComponent)
        {
            comps = go.GetComponents<Component>();
            haveMissingComponent = false;

            GUI.color = !isSelected && !activeSelf ? Color.gray : Color.white;

            for (int i = 1; i < comps.Length; i++)
            {
                Rect compRect = new Rect(
                    rect.x + rect.width - ((comps.Length - i) * iconSize) + ((comps.Length - i) * -iconsSpacing),
                    rect.y,
                    iconSize,
                    iconSize);

                if (comps[i] == null)
                {
                    haveMissingComponent = true;
                    if (MVsToolkitPreferences.Values.DrawComponentsIcon)
                    {
                        DrawIcon(compRect, "console.erroricon", "Missing Component");
                        continue;
                    }
                    else return;
                }

                if (MVsToolkitPreferences.Values.DrawComponentsIcon)
                {
                    icon = EditorGUIUtility.ObjectContent(null, comps[i].GetType()).image as Texture2D;
                    DrawIcon(compRect, icon, comps[i].GetType().Name);
                }
            }
        }
        
        static void DrawSetActiveToggle(Rect rect, Event e)
        {
            Rect toggleRect = new Rect(rect.x - 27 - (14 * parentCount), rect.y - 1, 18, 18);

            if (rect.Contains(e.mousePosition) || toggleRect.Contains(e.mousePosition))
            {
                bool newActive = GUI.Toggle(toggleRect, go.activeSelf, GUIContent.none);

                if (newActive != go.activeSelf)
                {
                    Undo.RecordObject(go, "Toggle Active State");
                    go.SetActive(newActive);
                    EditorUtility.SetDirty(go);
                }
            }
        }
        static void DrawChildLine(Rect rect)
        {
            if (go.transform.childCount > 0)
            {
                Rect foldoutRect = new Rect(rect.x - 14f, rect.y, 14f, rect.height);
                EditorGUI.Foldout(foldoutRect, isGameObjectExpand, GUIContent.none, false);
            }

            if (MVsToolkitPreferences.Values.DrawChildLines)
            {
                for (int i = 0; i < parentCount; i++)
                {
                    EditorGUI.DrawRect(new Rect(rect.x - 22 - (14 * i), rect.y, 1, rect.height), new Color(.3f, .3f, .3f));
                }
            }
        }

        static void DrawIcon(Rect rect, string textureName, string tooltip = "")
        {
            icon = EditorGUIUtility.IconContent(textureName).image;
            content = new GUIContent(icon, tooltip);
            GUI.Label(rect, content, iconStyle);
        }
        static void DrawIcon(Rect rect, Texture icon, string tooltip = "")
        {
            content = new GUIContent(icon, tooltip);
            GUI.Label(rect, content, iconStyle);
        }
        #endregion

        #region Helpers
        static Color StrToColor(string str)
        {
            if (ColorUtility.TryParseHtmlString(str, out Color color)) return color;
            return Color.white;
        }
        static bool IsGameObjectExpand(int instanceID)
        {
            System.Type sceneHierarchyWindowType = typeof(EditorWindow).Assembly.GetType("UnityEditor.SceneHierarchyWindow");
            System.Reflection.MethodInfo getExpandedIDs = sceneHierarchyWindowType.GetMethod("GetExpandedIDs", BindingFlags.NonPublic | BindingFlags.Instance);

            // Déclaration explicite du type
            PropertyInfo lastInteractedHierarchyWindow =
                sceneHierarchyWindowType.GetProperty("lastInteractedHierarchyWindow", BindingFlags.Public | BindingFlags.Static);

            if (lastInteractedHierarchyWindow == null)
            {
                return false;
            }

            int[] expandedIDs = getExpandedIDs.Invoke(lastInteractedHierarchyWindow.GetValue(null), null) as int[];

            return expandedIDs != null && expandedIDs.Contains(instanceID);
        }
        static int GetParentCount(GameObject go)
        {
            if (go == null) return 0;

            int count = 0;
            Transform t = go.transform.parent;
            while (t != null)
            {
                count++;
                t = t.parent;
            }

            return count;
        }

        static bool IsPartOfMissingPrefab(GameObject go)
        {
            var root = PrefabUtility.GetNearestPrefabInstanceRoot(go);
            if (root == null) return false;

            var status = PrefabUtility.GetPrefabInstanceStatus(root);
            return status != PrefabInstanceStatus.Connected;
        }
        static bool GetHierarchyActiveSelf(GameObject go)
        {
            if (go == null) return false;

            Transform t = go.transform;

            while (t != null)
            {
                if (!t.gameObject.activeSelf)
                    return false;

                t = t.parent;
            }

            return true;
        }

        static GameObjectState GetState(bool haveMissingComponent)
        {
            bool isRootOfPrefab = PrefabUtility.GetNearestPrefabInstanceRoot(go) == go;
            bool isPartOfPrefab = PrefabUtility.IsPartOfAnyPrefab(go);

            if (IsPartOfMissingPrefab(go))
                return GameObjectState.ErrorFromMissingPrefab;

            if (haveMissingComponent)
                return GameObjectState.ErrorFromMissingComponent;

            if(isPartOfPrefab && !isRootOfPrefab)
                return GameObjectState.PrefabChildren;

            if (PrefabUtility.IsPartOfVariantPrefab(go))
                return GameObjectState.PrefabVariant;

            if (isPartOfPrefab && isRootOfPrefab)
                return GameObjectState.Prefab;

            return GameObjectState.Normal;
        }

        static void InitializeStyles()
        {
            if (iconStyle != null) return;
            iconStyle = new GUIStyle();
            iconStyle.padding = new RectOffset(0, 0, 0, 0);
            iconStyle.margin = new RectOffset(0, 0, 0, 0);
            iconStyle.border = new RectOffset(0, 0, 0, 0);
        }

        public static Color HierarchyBackgroundColor(bool isOdd = false)
        {
            Color color = Color.white;
            if (ColorUtility.TryParseHtmlString(MVsToolkitPreferences.Values.ZebraModColor, out Color c)) color = c;

            if (EditorGUIUtility.isProSkin)
                return (MVsToolkitPreferences.Values.DrawZebraMod && isOdd) ?
                    color : new Color(0.219f, 0.219f, 0.219f);
            else
                return (MVsToolkitPreferences.Values.DrawZebraMod && isOdd) ?
                     new Color(0.92f, 0.92f, 0.92f) : new Color(0.76f, 0.76f, 0.76f);
        }

        public static Color HierarchyHoverColor
        {
            get
            {
                if (EditorGUIUtility.isProSkin)
                    return new Color(0.2666667f, 0.2666667f, 0.2666667f);
                else
                    return new Color(0.8f, 0.8f, 0.8f);
            }
        }

        public static Color HierarchySelectionColor
        {
            get
            {
                if (EditorGUIUtility.isProSkin)
                    return new Color(0.172549f, 0.3647059f, 0.5294118f);
                else
                    return new Color(0.24f, 0.49f, 0.90f);
            }
        }
        #endregion

        enum GameObjectState
        {
            Normal,
            Prefab,
            PrefabVariant,
            PrefabChildren,

            ErrorFromMissingPrefab,
            ErrorFromMissingComponent,
        }
    }
}