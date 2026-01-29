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
            VerifyData();
        }
        
        public void Shutdown()
        {
            m_Storage.Save();
        }

        private void VerifyData()
        {
            foreach (var folder in m_Storage.FavoritesData.Folders)
            {
                foreach (IFavoritesElement element in folder.Elements.ToArray())
                {
                    if (!element.IsValid())
                    {
                        folder.Elements.Remove(element);
                    }
                }
            }
            m_Storage.Save();
        }

        public void FocusElement(IFavoritesElement element)
        {
            Object obj = element.GetObject();
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
        
        public void DeleteFolder(FavoritesFolder folder)
        {
            throw new NotImplementedException();
            m_Storage.FavoritesData.Folders.Remove(folder);
            m_Storage.Save();
        }
        
        public void LoadFolder(FavoritesFolder folder)
        {
            throw new NotImplementedException();
        }

        public void AddItem(FavoritesFolder folder, FavoritesAsset resources)
        {
            throw new NotImplementedException();
            folder.Elements.Add(resources);
            m_Storage.Save();
        }
        
        public void RemoveItem()
        {
            throw new NotImplementedException();
        }

        
    }
}

