using System;
using UnityEditor;
using UnityEngine;

namespace MVsToolkit.Favorites
{
    public static class FactoryFavoritesElement
    {
        public static IFavoritesElement Create(FavoritesElementContext context)
        {
            IFavoritesElement element;
            
            switch (context.ObjectTarget)
            {
                case GameObject go:
                {
                    string scenePath = go.scene.path;
                    element = new FavoritesSceneObject(scenePath, String.Empty);
                }  
                    break;
                default:
                    element = new FavoritesAsset(context.Guid);
                    break;
            }
            
            Debug.Assert(element != null);
            return element;
        }
        
        public struct FavoritesElementContext
        {
            public UnityEngine.Object ObjectTarget;
            public string Guid;
        }
    }
}