using System.Collections.Generic;
using UnityEngine;

namespace MVsToolkit.SceneBrowser
{
    public class SceneBrowserDatabase : ScriptableObject
    {
        public List<SceneBrowerData> scenes = new();
    }
}