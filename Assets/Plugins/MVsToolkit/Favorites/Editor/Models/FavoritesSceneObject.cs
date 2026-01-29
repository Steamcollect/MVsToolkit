using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace MVsToolkit.Favorites
{
    [Serializable]
    public class FavoritesSceneObject : IFavoritesElement
    {
        public string ObjectPathInScene;
        public string ScenePath;

        public string Name => GetObject() != null ? GetObject().name : "Missing Scene Object";

        public bool IsValid()
        {
            Scene scene = SceneManager.GetSceneByPath(ScenePath);
            if (!scene.IsValid()) return false;
            GameObject[] rootObjects = scene.GetRootGameObjects();
            foreach (var rootObj in rootObjects)
            {
                Transform targetObj = rootObj.transform.Find(ObjectPathInScene);
                if (targetObj == null) continue;
                return true;
            }
            return false;
        }

        public Object GetObject()
        {
            Scene scene = SceneManager.GetSceneByPath(ScenePath);
            if (!scene.IsValid()) return null;
            GameObject[] rootObjects = scene.GetRootGameObjects();
            foreach (var rootObj in rootObjects)
            {
                Transform targetObj = rootObj.transform.Find(ObjectPathInScene);
                if (targetObj == null) continue;
                return targetObj.gameObject;
            }
            return null;
        }
    }
}