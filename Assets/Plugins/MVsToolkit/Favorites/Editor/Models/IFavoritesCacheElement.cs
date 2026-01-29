using UnityEngine;

namespace MVsToolkit.Favorites
{
    public interface IFavoritesCacheElement
    {
        string Name { get; }
        Object Object { get; }
        Texture2D Preview { get; }
    }
    
    public class DummyFavoritesCacheElement : IFavoritesCacheElement
    {
        public string Name => "Missing Element";
        public Object Object => null;
        public Texture2D Preview => null;
    }
}