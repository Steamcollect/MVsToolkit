using UnityEditor;
using UnityEngine;

namespace MVsToolkit.BetterInterface
{
    public static class MVsToolkitPreferences
    {
        #region Hierarchy Settings

        #region Boolean
        const string k_DrawFolderIconInHierarchyKey = "ToolBox_DrawFolderIconInHierarchyKey";
        public static bool s_DrawFolderIconInHierarchy
        {
            get => EditorPrefs.GetBool(k_DrawFolderIconInHierarchyKey, true);
            set => EditorPrefs.SetBool(k_DrawFolderIconInHierarchyKey, value);
        }

        //-----------------------------

        const string k_OverrideGameObjectIconKey = "ToolBox_OverrideGameObjectIconKey";
        public static bool s_OverrideGameObjectIcon
        {
            get => EditorPrefs.GetBool(k_OverrideGameObjectIconKey, true);
            set => EditorPrefs.SetBool(k_OverrideGameObjectIconKey, value);
        }

        //-----------------------------

        const string k_ShowComponentsIconsKey = "k_ShowComponentsIconsKey";
        public static bool s_ShowComponentsIcon
        {
            get => EditorPrefs.GetBool(k_ShowComponentsIconsKey, true);
            set => EditorPrefs.SetBool(k_ShowComponentsIconsKey, value);
        }

        //-----------------------------

        const string k_IsZebraModeKey = "k_ZebraModeKey";
        public static bool s_IsZebraMode
        {
            get => EditorPrefs.GetBool(k_IsZebraModeKey, true);
            set => EditorPrefs.SetBool(k_IsZebraModeKey, value);
        }

        //-----------------------------

        const string k_IsChildLineKey = "k_IsChildLineKey";
        public static bool s_IsChildLine
        {
            get => EditorPrefs.GetBool(k_IsChildLineKey, true);
            set => EditorPrefs.SetBool(k_IsChildLineKey, value);
        }
        #endregion

        #region Colors
        
        // ZEBRA
        
        const string k_ZebraSecondColor = "k_ZebraSecondColorKey";
        public static Color s_ZebraSecondColor
        {
            get
            {
                string colorString = EditorPrefs.GetString(k_ZebraSecondColor, "353535"); // valeur par défaut
                if (ColorUtility.TryParseHtmlString("#" + colorString, out Color c))
                    return c;

                return Color.white;
            }
            set
            {
                string colorString = ColorUtility.ToHtmlStringRGBA(value);
                EditorPrefs.SetString(k_ZebraSecondColor, colorString);
            }
        }

        // PREFAB
        
        const string k_PrefabColorKey = "k_EnablePrefabColorKey";
        public static Color s_PrefabColor
        {
            get
            {
                string colorString = EditorPrefs.GetString(k_PrefabColorKey, "8CC7FF"); // valeur par défaut
                if (ColorUtility.TryParseHtmlString("#" + colorString, out Color c))
                    return c;

                return Color.white;
            }
            set
            {
                string colorString = ColorUtility.ToHtmlStringRGBA(value);
                EditorPrefs.SetString(k_PrefabColorKey, colorString);
            }
        }

        // MISSING PREFAB

        const string k_MissingPrefabColorKey = "k_EnableMissingPrefabColorKey";
        public static Color s_MissingPrefabColor
        {
            get
            {
                string colorString = EditorPrefs.GetString(k_MissingPrefabColorKey, "FF6767"); // valeur par défaut
                if (ColorUtility.TryParseHtmlString("#" + colorString, out Color c))
                    return c;

                return Color.white;
            }
            set
            {
                string colorString = ColorUtility.ToHtmlStringRGBA(value);
                EditorPrefs.SetString(k_MissingPrefabColorKey, colorString);
            }
        }
        #endregion

        #endregion

        //==============================

        [SettingsProvider]
        public static SettingsProvider CreatePreferencesProvider()
        {
            SettingsProvider provider = new SettingsProvider("Preferences/MVs Toolkit", SettingsScope.User)
            {
                label = "MV's Toolkit",

                guiHandler = (searchContext) =>
                {
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Hierarchy Settings", EditorStyles.boldLabel);
                    EditorGUILayout.Space();

                    s_DrawFolderIconInHierarchy = EditorGUILayout.Toggle("Draw Folder Icon", s_DrawFolderIconInHierarchy);
                    s_ShowComponentsIcon = EditorGUILayout.Toggle("Show Components Icon", s_ShowComponentsIcon);
                    s_OverrideGameObjectIcon = EditorGUILayout.Toggle("Override GameObject Icon", s_OverrideGameObjectIcon);

                    EditorGUILayout.Space();
                    s_IsZebraMode = EditorGUILayout.Toggle("Zebra Mode", s_IsZebraMode);
                    s_IsChildLine = EditorGUILayout.Toggle("Child Line", s_IsChildLine);
                    
                    EditorGUILayout.Space(30);

                    // Colors
                    s_ZebraSecondColor = EditorGUILayout.ColorField("Zebra Second Color", s_ZebraSecondColor);
                    EditorGUILayout.Space(10);
                    s_PrefabColor = EditorGUILayout.ColorField("Enable Prefab Color", s_PrefabColor);
                    s_MissingPrefabColor = EditorGUILayout.ColorField("Enable Missing Prefab Color", s_MissingPrefabColor);

                    // Reset values button
                    EditorGUILayout.Space(20);
                    if (GUILayout.Button("Reset Values"))
                    {
                        bool confirm = EditorUtility.DisplayDialog(
                            "Reset Preferences",
                            "Are you sure you want to reset all MV's Toolkit preferences to their default values?",
                            "Yes",
                            "Cancel"
                        );

                        if (confirm)
                        {
                            // Reset booleans
                            s_DrawFolderIconInHierarchy = true;
                            s_OverrideGameObjectIcon = true;
                            s_ShowComponentsIcon = true;
                            s_IsZebraMode = true;

                            // Reset colors
                            s_ZebraSecondColor = ColorUtility.TryParseHtmlString("#353535", out var zc) ? zc : Color.white;

                            s_PrefabColor = ColorUtility.TryParseHtmlString("#8CC7FF", out var epc) ? epc : Color.white;
                            s_MissingPrefabColor = ColorUtility.TryParseHtmlString("#FF6767", out var emc) ? emc : Color.white;

                            Debug.Log("MV's Toolkit preferences reset to default values.");
                        }
                    }

                },

                // recherche
                keywords = new System.Collections.Generic.HashSet<string>(new[] { "toolbox", "hierarchy", "gameobject", "icon" })
            };

            return provider;
        }
    }
}