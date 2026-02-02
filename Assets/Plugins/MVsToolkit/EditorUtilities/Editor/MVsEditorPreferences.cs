using UnityEditor;

namespace MVsToolkit.Utilities
{
    public static class MVsEditorPreferences
    {
        [MenuItem("Edit/Clear All EditorPrefs", false, 15000)]
        public static void ResetEditorPreferences()
        {
            EditorPrefs.DeleteAll();
        }
    }
}