using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace MVsToolkit.Favorites
{
    public class FavoritesStorage
    {
        #region Fields

        private static readonly string s_FavoriteKeyEditorPrefs = "MVsToolkit_FavoritesData";
        
        private readonly Dictionary<IFavoritesElement, IFavoritesCacheElement> m_Cache = new();
        private static readonly IFavoritesCacheElement s_DefaultCacheElement = new NullFavoritesCacheElement();
        private FavoritesData m_FavoritesData;
        #endregion
        
        #region Properties
        public List<FavoritesGroup> FavoritesGroups => m_FavoritesData.FavoritesGroups;
        #endregion

        #region Serialization

        public void Load()
        {
            m_FavoritesData = new FavoritesData();
            if (EditorPrefs.HasKey(s_FavoriteKeyEditorPrefs)) 
            {
                string jsonData = EditorPrefs.GetString(s_FavoriteKeyEditorPrefs);
                EditorJsonUtility.FromJsonOverwrite(jsonData, m_FavoritesData);
            }
        }
        
        public void Save()
        {
            Debug.Assert(m_FavoritesData != null);
            string jsonData = EditorJsonUtility.ToJson(m_FavoritesData,true);
            EditorPrefs.SetString(s_FavoriteKeyEditorPrefs, jsonData);
        }

        public void Reset()
        {
            Debug.Assert(m_FavoritesData != null);
            m_FavoritesData.FavoritesGroups.Clear();
            if (EditorPrefs.HasKey(s_FavoriteKeyEditorPrefs))
                EditorPrefs.DeleteKey(s_FavoriteKeyEditorPrefs);
        }

        #endregion
        
        public bool ContainElement(FavoritesGroup group, Object obj)
        {
            Debug.Assert(group != null && obj != null);

            foreach (var element in group.Elements)
            {
                IFavoritesCacheElement elementCache = Resolve(element);
                if (elementCache == s_DefaultCacheElement)
                {
                    if (element.GetObject() == obj)
                        return true;
                }
                else if (elementCache.Object == obj)
                    return true;
            }

            return false;
        }
        
        #region Cache

        public IFavoritesCacheElement Resolve(IFavoritesElement element)
        {
            return m_Cache.GetValueOrDefault(element, s_DefaultCacheElement);
        }
        
        public void ClearCache()
        {
            m_Cache.Clear();
        }
        
        public void CacheGroup(FavoritesGroup group)
        {
            foreach (IFavoritesElement element in group.Elements)
            {
                if (element == null) continue;
                if (!m_Cache.ContainsKey(element))
                {
                    m_Cache.Add(element, FactoryFavoritesCacheElement.Create(element));
                }
            }
        }

        #endregion
    }
}