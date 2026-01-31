using System;
using MVsToolkit.Dev;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public string scriptName;
    public string exportPath;

    [Button]
    void CreateAsset()
    {
        Type type = FindType(scriptName);

        if (type == null)
        {
            Debug.LogError("Impossible de trouver le type : " + scriptName);
            return;
        }

        ScriptableObject asset = ScriptableObject.CreateInstance(type);

        string fullPath = "Assets/" + exportPath + scriptName + ".asset";

        string dir = System.IO.Path.GetDirectoryName(fullPath);
        if (!System.IO.Directory.Exists(dir))
            System.IO.Directory.CreateDirectory(dir);

        AssetDatabase.CreateAsset(asset, fullPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    System.Type FindType(string typeName)
    {
        foreach (var asm in System.AppDomain.CurrentDomain.GetAssemblies())
        {
            var type = asm.GetType(typeName);
            if (type != null)
                return type;
        }
        return null;
    }
}