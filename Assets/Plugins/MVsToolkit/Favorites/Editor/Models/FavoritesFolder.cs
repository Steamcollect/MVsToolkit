using System;
using System.Collections.Generic;
using UnityEngine;

namespace MVsToolkit.Favorites
{
    [Serializable]
    public class FavoritesFolder
    {
        public string Name = "New Folder";
        public Color Color = Color.white;
        public HashSet<FavoriteItem> Items = new();
    }
}