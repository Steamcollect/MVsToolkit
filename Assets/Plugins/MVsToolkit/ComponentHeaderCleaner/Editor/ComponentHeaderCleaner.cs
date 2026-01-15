using System.Collections;
using System.Reflection;
using MVsToolkit.Preferences;
using UnityEditor;

namespace MVsToolkit.ComponentHeaderCleaner
{
    public class ComponentHeaderCleaner
    {
        [InitializeOnLoadMethod]
        private static void Init()
        {
            if (!MVsToolkitPreferences.Values.CleanComponentHeader) return;
            EditorApplication.update += InitHeader;
        }

        private static void InitHeader()
        {
            BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Static;

            FieldInfo fieldInfo = typeof(EditorGUIUtility).GetField("s_EditorHeaderItemsMethods", flags);
            IList value = (IList)fieldInfo.GetValue(null);
            if (value == null) return;

            value.Clear();

            EditorApplication.update -= InitHeader;
        }
    }
}