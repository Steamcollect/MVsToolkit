using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace MVsToolkit.Favorites
{
    [Serializable]
    public class FavoritesSceneObject : IFavoritesElement
    {
        [SerializeField] private string m_ObjectPathInScene;
        [SerializeField] private string m_ScenePath;

        public string Name => GetObject() != null ? GetObject().name : "Missing Scene Object";

        public FavoritesSceneObject(string scenePath, string objectPathInScene)
        {
            m_ScenePath = scenePath;
            m_ObjectPathInScene = objectPathInScene;
        }
        
        public override bool IsValid()
        {
            Scene scene = SceneManager.GetSceneByPath(m_ScenePath);
            if (!scene.IsValid()) return false;
            GameObject[] rootObjects = scene.GetRootGameObjects();
            foreach (GameObject rootObj in rootObjects)
            {
                Transform targetObj = rootObj.transform.Find(m_ObjectPathInScene);
                if (targetObj == null) continue;
                return true;
            }
            return false;
        }

        public override Object GetObject()
        {
            //TODO: Handle ref gameobject in unloaded scene
            Scene scene = SceneManager.GetSceneByPath(m_ScenePath);
            if (!scene.IsValid()) return null;
            GameObject[] rootObjects = scene.GetRootGameObjects();
            foreach (var rootObj in rootObjects)
            {
                Transform targetObj = rootObj.transform.Find(m_ObjectPathInScene);
                if (targetObj == null) continue;
                return targetObj.gameObject;
            }
            return null;
        }
    }
}