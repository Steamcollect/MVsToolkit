using System;
using System.Collections.Generic;

namespace MVsToolkit.Favorites
{
    [Serializable]
    public class FavoritesData 
    {
        #region Fields

        public List<FavoritesFolder> Folders = new();

        #endregion
    }
}

