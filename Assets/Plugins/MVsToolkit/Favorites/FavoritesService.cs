using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace MVsToolkit.Favorites
{
    public class FavoritesService
    {
        public Texture GetPreview(FavoriteItemData data)
        {
            var asset = AssetDatabase.LoadAssetAtPath<Object>(AssetDatabase.GUIDToAssetPath(data.ItemGuid));
            return AssetPreview.GetAssetPreview(asset);
        }
    }
}