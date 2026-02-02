using UnityEngine;

namespace MVsToolkit.Favorites
{
    public static class FactoryFavoritesCacheElement
    {
        public static IFavoritesCacheElement Create(IFavoritesElement element)
        {
            Debug.Assert(element != null && element.IsValid());
            Object obj = element.GetObject();
            
            IFavoritesCacheElement cachedElement = new DefaultFavoritesCacheElement
            {
                Name = obj.name,
                Object = obj,
            };
            
            return cachedElement;
        }
    }
}