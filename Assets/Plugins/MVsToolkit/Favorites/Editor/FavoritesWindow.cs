using System;
using UnityEngine;
using UnityEditor;

namespace MVsToolkit.Favorites
{
    public class FavoritesWindow: EditorWindow
    {
        
        private FavoritesService m_FavoritesService;
        private Vector2 m_ScrollPos;
        private int m_SelectedFolderIndex = 0;

        [MenuItem("Tools/MVsToolkit/Favorites")]
        public static void ShowWindow()
        {
            var wnd = GetWindow<FavoritesWindow>("Favorites");
            wnd.minSize = new Vector2(300, 200);
            wnd.Show();
        }

        private void OnEnable()
        {
            m_FavoritesService = new FavoritesService();
            m_FavoritesService.Init();
        }
        
        private void OnDisable()
        {
            m_FavoritesService.Storage.Save();
        }
        
        private void OnGUI()
        {
            FavoritesData data = m_FavoritesService.Storage.FavoritesData;

            EditorGUILayout.BeginVertical();

            // Top: folder tabs
            DrawFolderTabs(data);

            // Main content
            m_ScrollPos = EditorGUILayout.BeginScrollView(m_ScrollPos);
            if (data.Folders.Count > 0 && m_SelectedFolderIndex >= 0 && m_SelectedFolderIndex < data.Folders.Count)
            {
                DrawFolderSelected(data.Folders[m_SelectedFolderIndex]);
            }
            else
            {
                EditorGUILayout.LabelField("No folders. Create one using the button below.");
            }
            EditorGUILayout.EndScrollView();

            // Bottom bar
            DrawBottomBar();

            HandleDragAndDrop();

            EditorGUILayout.EndVertical();
        }

        private void DrawFolderTabs(FavoritesData data)
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            for (int i = 0; i < data.Folders.Count; i++)
            {
                var folder = data.Folders[i];
                GUI.backgroundColor = folder.Color;
                if (GUILayout.Toggle(i == m_SelectedFolderIndex, folder.Name, EditorStyles.toolbarButton))
                {
                    m_SelectedFolderIndex = i;
                }
                GUI.backgroundColor = Color.white;
            }
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("+", EditorStyles.toolbarButton, GUILayout.Width(30)))
            {
                m_FavoritesService.CreateFolder("New Folder");
                m_SelectedFolderIndex = m_FavoritesService.Storage.FavoritesData.Folders.Count - 1;
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawFolderSelected(FavoritesFolder folder)
        {
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.BeginHorizontal();
            EditorGUI.BeginChangeCheck();
            var newName = EditorGUILayout.TextField(folder.Name);
            var newColor = EditorGUILayout.ColorField(folder.Color, GUILayout.Width(50));
            if (EditorGUI.EndChangeCheck())
            {
                folder.Name = newName;
                folder.Color = newColor;
                m_FavoritesService.Storage.Save();
            }
             // if (GUILayout.Button("Delete Folder", GUILayout.Width(100)))
             // {
             //     if (EditorUtility.DisplayDialog("Delete folder?", $"Delete folder '{folder.Name}'?", "Delete", "Cancel"))
             //     {
             //         // m_FavoritesService.DeleteFolder(folder);
             //         m_SelectedFolderIndex = Mathf.Clamp(m_SelectedFolderIndex - 1, 0, Math.Max(0, m_FavoritesService.Storage.UserData.Folders.Count - 1));
             //         return;
             //     }
             // }
             EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();


            foreach (var item in folder.Items)
            {
                EditorGUILayout.BeginHorizontal("box");
                var preview = m_FavoritesService.Storage.GetPreview(item);
                GUILayout.Label(preview ? preview : Texture2D.grayTexture, GUILayout.Width(64), GUILayout.Height(64));
                EditorGUILayout.BeginVertical();
                EditorGUILayout.LabelField(item.ItemGuid.ToString());
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Focus", GUILayout.Width(60)))
                {
                    m_FavoritesService.FocusElement(item);
                }
                if (GUILayout.Button("Delete", GUILayout.Width(60)))
                {
                    // m_FavoritesService.DeleteItem(folder, item);
                    return;
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndVertical();
        }

        private void DrawBottomBar()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
            if (GUILayout.Button("Save"))
            {
                // m_FavoritesService.Save();
            }

            if (GUILayout.Button("Reset"))
            {
                if (EditorUtility.DisplayDialog("Reset favorites?", "This will remove all saved favorites.", "Reset", "Cancel"))
                {
                    // m_FavoritesService.Reset();
                }
            }

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }

        private void HandleDragAndDrop()
        {
            var evt = Event.current;
            var dropArea = GUILayoutUtility.GetLastRect();
            switch (evt.type)
            {
                case EventType.DragUpdated:
                case EventType.DragPerform:
                    if (!dropArea.Contains(evt.mousePosition))
                        return;

                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                    if (evt.type == EventType.DragPerform)
                    {
                        DragAndDrop.AcceptDrag();
                        foreach (var obj in DragAndDrop.objectReferences)
                        {
                            string path = AssetDatabase.GetAssetPath(obj);
                            string guid = AssetDatabase.AssetPathToGUID(path);
                            FavoriteItem item = new()
                            {
                                ItemGuid = new GUID(guid)
                            };
                            if (m_FavoritesService.Storage.FavoritesData.Folders.Count == 0)
                            {
                                m_FavoritesService.CreateFolder("Default");
                                m_SelectedFolderIndex = 0;
                            }
                            var folder = m_FavoritesService.Storage.FavoritesData.Folders[m_SelectedFolderIndex];
                            m_FavoritesService.AddItemToFolder(folder, item);
                        }
                    }
                    Event.current.Use();
                    break;
            }
        }
    }
}