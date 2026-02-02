using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;

namespace MVsToolkit.Favorites
{
    public class FavoritesWindow: EditorWindow
    {
        private FavoritesService m_FavoritesService;
        private Vector2 m_ScrollPos;
        private int m_SelectedFolderIndex;

        [MenuItem("Tools/MVsToolkit/Favorites")]
        public static void Open()
        {
            GetWindow<FavoritesWindow>("Favorites");
        }

        private void OnEnable()
        {
            m_FavoritesService = new FavoritesService();
            m_FavoritesService.Init();
        }
        
        private void OnDisable()
        {
            m_FavoritesService.Shutdown();
            m_FavoritesService = null;
        }

        private void OnGUI()
        {
            using (new EditorGUILayout.VerticalScope())
            {
                DrawFolderTabs();

                using (new EditorGUILayout.ScrollViewScope(m_ScrollPos))
                {
                    if (m_FavoritesService.Storage.FavoritesGroups.Count > 0 && m_SelectedFolderIndex >= 0 && m_SelectedFolderIndex < m_FavoritesService.Storage.FavoritesGroups.Count)
                    {
                        DrawFolderSelected();
                    }
                    else
                    {
                        EditorGUILayout.LabelField("No folders. Create one using the button below.");
                    }
                }
                
                // DrawBottomBar();

                HandleDragAndDrop();
                
            }
        }

        private void DrawFolderTabs()
        {
            List<FavoritesGroup> favoritesGroups = m_FavoritesService.Storage.FavoritesGroups;

            using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                for (int i = 0; i < favoritesGroups.Count; i++)
                {
                    FavoritesGroup folder = favoritesGroups[i];
                    GUI.backgroundColor = folder.Color;
                    if (GUILayout.Toggle(i == m_SelectedFolderIndex, folder.Name, EditorStyles.toolbarButton))
                    {
                        m_SelectedFolderIndex = i;
                        m_FavoritesService.LoadFolder(m_FavoritesService.Storage.FavoritesGroups[m_SelectedFolderIndex]);
                    }
                    GUI.backgroundColor = Color.white;
                }
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("+", EditorStyles.toolbarButton, GUILayout.Width(30)))
                {
                    m_FavoritesService.CreateFolder("New Folder");
                    m_SelectedFolderIndex = m_FavoritesService.Storage.FavoritesGroups.Count - 1;
                }
            }
        }

        private void DrawFolderSelected()
        {
            
            FavoritesGroup group = m_FavoritesService.Storage.FavoritesGroups[m_SelectedFolderIndex];
            
            using (new EditorGUILayout.VerticalScope("box"))
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUI.BeginChangeCheck();
                    string newName = EditorGUILayout.TextField(group.Name);
                    Color newColor = EditorGUILayout.ColorField(group.Color, GUILayout.Width(50));
                    if (EditorGUI.EndChangeCheck())
                    {
                        group.Name = newName;
                        group.Color = newColor;
                        m_FavoritesService.Storage.Save();
                    }
                }
                
                EditorGUILayout.Space();
                
                foreach (IFavoritesElement item in group.Elements.ToList())
                {
                    IFavoritesCacheElement itemCache = m_FavoritesService.Storage.Resolve(item);

                    using (new EditorGUILayout.HorizontalScope("box"))
                    {
                        Texture2D preview = itemCache.Preview;
                        GUILayout.Label(preview ? preview : Texture2D.grayTexture, GUILayout.Width(64), GUILayout.Height(64));
                        using (new EditorGUILayout.VerticalScope())
                        {
                            EditorGUILayout.LabelField(itemCache.Name);
                            using (new EditorGUILayout.HorizontalScope())
                            {
                                if (GUILayout.Button("Focus", GUILayout.Width(60)))
                                {
                                    m_FavoritesService.FocusElement(item);
                                }
                                if (GUILayout.Button("Delete", GUILayout.Width(60)))
                                {
                                    m_FavoritesService.DeleteItem(group, item);
                                }
                            }
                        }
                    }
                }
                
            }
        }

        private void DrawBottomBar()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }

        private void HandleDragAndDrop()
        {
            Event evt = Event.current;
            Rect dropArea = GUILayoutUtility.GetLastRect();
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
                        foreach (Object obj in DragAndDrop.objectReferences)
                        {
                            if (m_FavoritesService.Storage.FavoritesGroups.Count == 0)
                            {
                                m_FavoritesService.CreateFolder(null);
                                m_SelectedFolderIndex = 0;
                                m_FavoritesService.AddItem(m_FavoritesService.Storage.FavoritesGroups[m_SelectedFolderIndex],obj);
                            }
                            else
                            {
                                FavoritesGroup group = m_FavoritesService.Storage.FavoritesGroups[m_SelectedFolderIndex];
                                m_FavoritesService.AddItem(group, obj);
                            }
                            
                        }
                    }
                    Event.current.Use();
                    break;
            }
        }
    }
}