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
        
        public void Save(FavoritesData data)
        {
            string jsonData = EditorJsonUtility.ToJson(data);
            EditorPrefs.SetString(s_FavoriteKeyEditorPrefs, jsonData);
        }

        public void Clear()
        {
            m_FavoritesData = new FavoritesData();
            EditorPrefs.DeleteKey(s_FavoriteKeyEditorPrefs);
        }
        
        public void Load()
        {
            if (EditorPrefs.HasKey(s_FavoriteKeyEditorPrefs)) 
            {
                string jsonData = EditorPrefs.GetString(s_FavoriteKeyEditorPrefs);
                m_FavoritesData = JsonUtility.FromJson<FavoritesData>(jsonData);


                foreach (var folder in FavoritesData.Folders)
                {
                    foreach (var item in folder.Items.ToList())
                    {
                        if (AssetDatabase.LoadAssetByGUID(item.ItemGuid) == null)
                        {
                            // Remove invalid items
                            folder.Items.Remove(item);
                        }
                    }
                }
                
            }
            else
            {
                m_FavoritesData = new FavoritesData();
            }
        }
    }
}