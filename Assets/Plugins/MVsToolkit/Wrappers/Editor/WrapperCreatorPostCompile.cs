using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;

[InitializeOnLoad]
public static class WrapperCreatorPostCompile
{
    static string pendingScriptName;
    static string pendingAssetPath;

    static WrapperCreatorPostCompile()
    {
        CompilationPipeline.compilationFinished += OnCompilationFinished;
    }

    public static void Register(string scriptName, string assetPath)
    {
        pendingScriptName = scriptName;
        pendingAssetPath = assetPath;
    }

    static void OnCompilationFinished(object obj)
    {
        if (string.IsNullOrEmpty(pendingScriptName))
            return;

        // Attendre que Unity recharge les assemblies
        EditorApplication.delayCall += CreateAssetAfterReload;
    }

    static void CreateAssetAfterReload()
    {
        string finalName = "RSO_" + pendingScriptName;

        // Trouver le type dans toutes les assemblies
        var type = FindType(finalName);

        if (type == null)
        {
            Debug.LogError("Impossible de trouver le type après reload : " + finalName);
            return;
        }

        ScriptableObject asset = ScriptableObject.CreateInstance(type);

        string fullPath = "Assets/" + pendingAssetPath + finalName + ".asset";

        string dir = System.IO.Path.GetDirectoryName(fullPath);
        if (!System.IO.Directory.Exists(dir))
            System.IO.Directory.CreateDirectory(dir);

        AssetDatabase.CreateAsset(asset, fullPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("Asset créé : " + fullPath);

        pendingScriptName = null;
        pendingAssetPath = null;
    }
    static System.Type FindType(string typeName)
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