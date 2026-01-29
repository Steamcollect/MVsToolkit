using UnityEngine;

namespace MVsToolkit.Favorites
{
    public interface IFavoritesElement
    {
        string Name { get; }
        bool IsValid();
        Object GetObject();
    }
}