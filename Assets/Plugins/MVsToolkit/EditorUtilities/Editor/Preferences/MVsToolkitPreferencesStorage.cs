using System.IO;
using UnityEngine;

namespace MVsToolkit.Preferences
{
    public static class MVsToolkitPreferencesStorage
    {
        private static readonly string FilePath =
            Path.Combine("ProjectSettings", "MVsToolkitPreferences.json");

        public static void Save(MVsToolkitPreferencesValues values)
        {
            string json = JsonUtility.ToJson(values, true);
            File.WriteAllText(FilePath, json);
        }

        public static MVsToolkitPreferencesValues Load()
        {
            string json;

            if (!File.Exists(FilePath))
            {
                json = JsonUtility.ToJson(new(), true);
                File.WriteAllText(FilePath, json);

                Debug.Log("[MV's Toolkit] No preferences file found, creating new save");
                return new MVsToolkitPreferencesValues();
            }

            json = File.ReadAllText(FilePath);
            return JsonUtility.FromJson<MVsToolkitPreferencesValues>(json);
        }
    }
}