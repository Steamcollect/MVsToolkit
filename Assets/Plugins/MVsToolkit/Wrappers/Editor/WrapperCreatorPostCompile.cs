using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;
using UnityEngine.Analytics;

[InitializeOnLoad]
public static class WrapperCreatorPostCompile
{
    static WrapperCreatorPostCompile()
    {
        CompilationPipeline.compilationFinished += OnCompilationFinished;
    }

    static void OnCompilationFinished(object obj)
    {
        AssemblyReloadEvents.afterAssemblyReload += CreateAssetAfterReload;
    }

    static void CreateAssetAfterReload()
    {
        Debug.Log("Assembly reloaded");

        string scriptName = EditorPrefs.GetString("CurrentWrapperNameCreated");
        string assetPath = EditorPrefs.GetString("CurrentWrapperPathCreated");

        if (scriptName != null && scriptName != "")
        {
            Debug.Log(scriptName);
            Debug.Log(assetPath);

            var type = FindType(scriptName);

            if (type == null)
            {
                Debug.LogError("Impossible de trouver le type après reload : " + scriptName);
                return;
            }

            ScriptableObject asset = ScriptableObject.CreateInstance(type);

            string fullPath = "Assets/" + assetPath + scriptName + ".asset";

            string dir = System.IO.Path.GetDirectoryName(fullPath);
            if (!System.IO.Directory.Exists(dir))
                System.IO.Directory.CreateDirectory(dir);

            AssetDatabase.CreateAsset(asset, fullPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            EditorPrefs.SetString("CurrentWrapperNameCreated", "");
        }
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