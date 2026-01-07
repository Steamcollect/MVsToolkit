using MVsToolkit.Dev;
using UnityEngine;

namespace MVsToolkit.Demo
{
    public class MVsToolkitInspectorDemo : MonoBehaviour
    {
        [Tab("Dropdowns")]
        public string[] dropdownValues;

        [Space(10)]
        [Dropdown(16, 32, 64, 128, 256, 512)] public int dropdownDemo_1;
        [Dropdown("dropdownValues")] public string dropdownDemo_2;

        [Tab("Toggle")]
        public bool showVariable;

        [ShowIf("showVariable", true)] public float variableToShow;

        [HideIf("showVariable", false)] public int variableToHide;

        [Tab("Fodlout")]
        [Foldout("Foldout")]
        public int foldoutIntA;
        public int foldoutIntB;
        public int foldoutIntC;

        [CloseTab, Tab("Class")]
        [Inline] public InlineClass inlineClass;
        public InterfaceReference<IDemoInterface> demoInterface;

        [Tab("Handles")]
        [Handle] public Vector3 pointA;
        [Handle(Space.Self, HandleDrawType.Sphere, ColorPreset.Red)] public Vector3 pointB;
        [Handle(Space.World, HandleDrawType.Cube, ColorPreset.Cyan)] public Vector3 pointC;

        [Tab("Others")]
        [SceneName] public string sceneName;
        [TagName] public string tagName;

        [Space(10)]
        [Watch] public int valueWatched;

        [Button]
        void DebugButtonA()
        {
            Debug.Log("Debug_A");
        }

        [Button("Debug_B")] // Direct string parameter
        void DebugButtonB(string debugText)
        {
            Debug.Log(debugText);
        }

        [Button("dropdownDemo_1")] // Refers to variable name
        void DebugButtonC(int debugValue)
        {
            Debug.Log(debugValue);
        }
    }
}