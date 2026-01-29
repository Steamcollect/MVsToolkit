using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MVsToolkit.Favorites
{
    public class FavoritesService
    {
        public FavoritesStorage Storage => m_Storage;
        private FavoritesStorage m_Storage;

        public void Init()
        {
            m_Storage = new FavoritesStorage();
            Storage.Load();
            ValidateData();
        }

        public void DeleteItem()
        {
            throw new NotImplementedException();
        }

        public void DeleteFolder()
        {
            throw new NotImplementedException();
        }

        public void Reorder()
        {
            throw new NotImplementedException();
        }

        public void ValidateData()
        {
            foreach (var folder in m_Storage.FavoritesData.Folders)
            {
                foreach (FavoriteItem item in folder.Items.ToArray())
                {
                    if (!item.IsValid())
                    {
                        folder.Items.Remove(item);
                    }
                }
            }
            m_Storage.Save();
        }

        public void FocusElement(FavoriteItem item)
        {
            Object obj = item.GetObject();
            if (!obj) return;
            //TODO: Test in prefab mode && scene objects
            Selection.activeObject = obj;
            EditorGUIUtility.PingObject(obj);
        }

        public void CreateFolder(string name)
        {
            FavoritesFolder folder = new()
            {
                Name = name,
                Color = UnityEngine.Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.5f, 1f)
            };
            m_Storage.FavoritesData.Folders.Add(folder);
            m_Storage.Save();
        }

        public void AddItemToFolder(FavoritesFolder folder, FavoriteItem item)
        {
            // throw new NotImplementedException();
            folder.Items.Add(item);
            m_Storage.Save();
        }
    }
}

