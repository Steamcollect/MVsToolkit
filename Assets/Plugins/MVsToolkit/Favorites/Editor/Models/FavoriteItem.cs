using System;
using UnityEditor;

namespace MVsToolkit.Favorites
{
    [Serializable]
    public class FavoriteItem
    {
        public GUID ItemGuid;
        
        public bool IsValid()
        {
            return !ItemGuid.Equals(null) && AssetDatabase.GUIDToAssetPath(ItemGuid) != string.Empty;
        }

        public UnityEngine.Object GetObject()
        {
            return !IsValid() ? null : AssetDatabase.LoadAssetByGUID<UnityEngine.Object>(ItemGuid);
        }
        
    }
}