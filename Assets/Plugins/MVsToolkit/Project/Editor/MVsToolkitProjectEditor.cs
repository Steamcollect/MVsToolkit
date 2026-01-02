using MVsToolkit.MVsEditorUtility;
using UnityEditor;
using UnityEngine;

namespace MVsToolkit.BetterInterface
{
    [InitializeOnLoad]
    public static class MVsToolkitProjectEditor
    {
        static MVsToolkitProjectEditor()
        {
            EditorApplication.projectWindowItemOnGUI += OnProjectItemGUI;
        }

        static void OnProjectItemGUI(string guid, Rect selectionRect)
        {
            selectionRect.xMin -= 8;
            selectionRect.x -= 8;

            MVsGradientUtility.DrawHorizontalGradient(selectionRect, 
                new Color[] {
                Color.clear,
                Color.blue,
                Color.blue,
                Color.clear
            }, new float[] { 0, .05f, .3f, .7f });
        }

        class ProjectItem
        {
            public string Path;
            public Color Color;
        }
    }
}