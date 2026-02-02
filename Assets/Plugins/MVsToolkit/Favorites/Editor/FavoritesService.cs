using System;
using Object = UnityEngine.Object;
using UnityEditor;

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
            foreach (FavoritesGroup folder in m_Storage.FavoritesGroups)
            {
                foreach (IFavoritesElement element in folder.Elements.ToArray())
                {
                    if (element == null || !element.IsValid())
                    {
                        folder.Elements.Remove(element);
                    }
                }
            }
            m_Storage.Save();
        }

        public void FocusElement(IFavoritesElement element)
        {
            Object obj = m_Storage.Resolve(element).Object;
            if (!obj) return;
            //TODO: Test in prefab mode && scene objects
            Selection.activeObject = obj;
            EditorGUIUtility.PingObject(obj);
        }

        public void CreateFolder(string name)
        {
            if (String.IsNullOrEmpty(name)) name = "Default";

            FavoritesGroup group = new()
            {
                Name = name,
                Color = UnityEngine.Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.5f, 1f)
            };
            m_Storage.FavoritesGroups.Add(group);
            m_Storage.Save();
        }
        
        public void LoadFolder(FavoritesGroup group)
        {
            // Currently no special loading is needed as all data is in memory.
            // This method is a placeholder for potential future functionality.
            m_Storage.ClearCache();
            m_Storage.CacheGroup(group);
        }
        
        public void DeleteFolder(FavoritesGroup group)
        {
            m_Storage.FavoritesGroups.Remove(group);
            m_Storage.Save();
        }

        public void AddItem(FavoritesGroup group, Object obj)
        {
            
            if (group == null)
            {
                CreateFolder(null);
                group = m_Storage.FavoritesGroups[^1];
            }
            
            if (m_Storage.ContainElement(group, obj)) return;
            
            FactoryFavoritesElement.FavoritesElementContext ctx = new()
            {
                ObjectTarget = obj,
                Guid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(obj))
            };

            IFavoritesElement element = FactoryFavoritesElement.Create(ctx);

            group.Elements.Add(element);
            
            m_Storage.CacheGroup(group);
            
            m_Storage.Save();
        }

        public void DeleteItem(FavoritesGroup group, IFavoritesElement element)
        {
            group.Elements.Remove(element);
            m_Storage.Save();
        }
        
    }
}

