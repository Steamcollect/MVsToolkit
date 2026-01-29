using System;
using UnityEditor;
using Object = UnityEngine.Object;

namespace MVsToolkit.Favorites
{
    [Serializable]
    public class FavoritesAsset : IFavoritesElement
    {
        public GUID ItemGuid;

        public string Name => GetObject() != null ? GetObject().name : "Missing Asset";

        public bool IsValid()
        {
            return !ItemGuid.Equals(null) && AssetDatabase.GUIDToAssetPath(ItemGuid) != string.Empty;
        }

        public Object GetObject()
        {
            return !IsValid() ? null : AssetDatabase.LoadAssetByGUID<Object>(ItemGuid);
        }
    }
}