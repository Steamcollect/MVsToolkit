using UnityEditor;
using UnityEngine;

namespace MVsToolkit.Preferences
{
    [InitializeOnLoad]
    public static class MVsToolkitPreferences
    {
        public static MVsToolkitPreferencesValues Values;

        static MVsToolkitPreferences()
        {
            Values = MVsToolkitPreferencesStorage.Load();
        }

        [SettingsProvider]
        public static SettingsProvider CreatePreferencesProvider()
        {
            SettingsProvider provider = new SettingsProvider("Preferences/MVs Toolkit", SettingsScope.User)
            {
                label = "MV's Toolkit",

                guiHandler = (searchContext) =>
                {
                    EditorGUI.BeginChangeCheck();
                    MVsToolkitPreferencesDrawer.Draw(Values);

                    if (EditorGUI.EndChangeCheck())
                    {
                        MVsToolkitPreferencesStorage.Save(Values);
                    }

                    if (GUILayout.Button("Reset Values"))
                    {
                        bool confirm = EditorUtility.DisplayDialog(
                            "Reset Preferences",
                            "Are you sure you want to reset all MV's Toolkit preferences to their default values?",
                            "Reset",
                            "Cancel"
                        );

                        if (confirm)
                        {
                            Values = new MVsToolkitPreferencesValues();
                            MVsToolkitPreferencesStorage.Save(Values);
                            Debug.Log("MV's Toolkit preferences reset to default values.");
                        }
                    }
                },
                keywords = new System.Collections.Generic.HashSet<string>(new[] {
                    "mvstoolkit", 
                    "toolkit", 
                    "toolbox", 
                    "hierarchy", 
                    "icon" })
            };

            return provider;
        }
    }
}