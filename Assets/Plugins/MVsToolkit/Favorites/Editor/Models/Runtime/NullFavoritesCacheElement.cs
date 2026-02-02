using UnityEngine;

namespace MVsToolkit.Favorites
{
    public class NullFavoritesCacheElement : IFavoritesCacheElement
    {
        public string Name => string.Empty;
        public Object Object => null;
        public Texture2D Preview => Texture2D.blackTexture;
    }
}