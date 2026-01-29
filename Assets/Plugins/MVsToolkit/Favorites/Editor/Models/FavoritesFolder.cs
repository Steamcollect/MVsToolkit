using System;
using System.Collections.Generic;
using UnityEngine;

namespace MVsToolkit.Favorites
{
    [Serializable]
    public class FavoritesFolder
    {
        #region Fields

        public string Name = "New Folder";
        public Color Color = Color.white;
        public List<IFavoritesElement> Elements = new();

        #endregion
    }
}