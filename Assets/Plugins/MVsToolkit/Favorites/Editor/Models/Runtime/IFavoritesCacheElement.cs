using UnityEngine;

namespace MVsToolkit.Favorites
{
    public interface IFavoritesCacheElement
    {
        string Name { get; }
        Object Object { get;}
        Texture2D Preview { get; }
    }
}