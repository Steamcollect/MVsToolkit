using System;
using System.Collections.Generic;

namespace MVsToolkit.Favorites
{
    [Serializable]
    public class FavoritesData 
    {
        public List<FavoritesFolder> Folders = new();
    }
}

