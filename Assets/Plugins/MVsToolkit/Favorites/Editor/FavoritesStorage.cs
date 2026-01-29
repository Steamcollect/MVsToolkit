using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace MVsToolkit.Favorites
{
    public class FavoritesStorage
    {
        private static readonly string s_FavoriteKeyEditorPrefs = "MVsToolkit_FavoritesData";
        
        private FavoritesData m_FavoritesData;
        
        public FavoritesData FavoritesData => m_FavoritesData;
        
        public void Save()
        {
            if (m_FavoritesData == null) return;
            string jsonData = EditorJsonUtility.ToJson(m_FavoritesData, true);
            EditorPrefs.SetString(s_FavoriteKeyEditorPrefs, jsonData);
            
            Debug.Log(jsonData);
        }

        public void Reset()
        {
            m_FavoritesData = new FavoritesData();
            if (EditorPrefs.HasKey(s_FavoriteKeyEditorPrefs))
                EditorPrefs.DeleteKey(s_FavoriteKeyEditorPrefs);
        }
        
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
        
        public Texture GetPreview(FavoriteItem item)
        {
           Object obj = item.GetObject();
           if (obj == null)
                return null;
           Texture2D preview = AssetPreview.GetAssetPreview(obj);
           return preview;
        }
    }
}