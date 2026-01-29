using System;

namespace MVsToolkit.Favorites
{
    public class FactoryFavoritesElement
    {
        public static IFavoritesElement Create(FavoritesElementContext context)
        {

            switch (context.ObjectTarget)
            {
                case UnityEngine.GameObject go:
                {
                    string scenePath = go.scene.path;
                    return new FavoritesSceneObject()
                    {
                        ScenePath = scenePath,
                        //TODO: Inject GameObject Path inside FavoritesSceneObjectInstance
                        ObjectPathInScene = String.Empty,
                    };
                }   
                default:
                    return new FavoritesAsset()
                    {
                        ItemGuid = context.Guid
                    };
            }
        }
        
        public struct FavoritesElementContext
        {
            public UnityEngine.Object ObjectTarget;
            public UnityEditor.GUID Guid;
        }
    }
}