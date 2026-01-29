using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MVsToolkit.Favorites
{
    public class FavoritesStorage
    {
        #region Fields

        private static readonly string s_FavoriteKeyEditorPrefs = "MVsToolkit_FavoritesData";
        
        private readonly Dictionary<IFavoritesElement, IFavoritesCacheElement> m_Cache = new();
        private FavoritesData m_FavoritesData;
        #endregion
        
        #region Properties
        public FavoritesData FavoritesData => m_FavoritesData;
        #endregion

        #region Serialization

        public void Load()
        {
            m_FavoritesData = new FavoritesData();
            
            if (EditorPrefs.HasKey(s_FavoriteKeyEditorPrefs)) 
            {
                string jsonData = EditorPrefs.GetString(s_FavoriteKeyEditorPrefs);
                EditorJsonUtility.FromJsonOverwrite(jsonData, m_FavoritesData);
                
                Debug.Log(jsonData);
            }
        }
        
        public void Save()
        {
            if (m_FavoritesData == null) return;
            string jsonData = EditorJsonUtility.ToJson(m_FavoritesData, true);
            EditorPrefs.SetString(s_FavoriteKeyEditorPrefs, jsonData);
        }

        public void Reset()
        {
            m_FavoritesData = new FavoritesData();
            if (EditorPrefs.HasKey(s_FavoriteKeyEditorPrefs))
                EditorPrefs.DeleteKey(s_FavoriteKeyEditorPrefs);
        }

        #endregion

        #region Cache

        public IFavoritesCacheElement Resolve(IFavoritesElement element)
        {
            return m_Cache.GetValueOrDefault(element);
        }

        #endregion
    }
}