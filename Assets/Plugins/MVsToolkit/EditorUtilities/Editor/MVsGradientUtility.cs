using UnityEditor;
using UnityEngine;

namespace MVsToolkit.MVsEditorUtility
{
    public static class MVsGradientUtility
    {
        private static Material _glMat;

        private static Material GLMat
        {
            get
            {
                if (_glMat == null)
                {
                    _glMat = new Material(Shader.Find("Hidden/Internal-Colored"));
                    _glMat.hideFlags = HideFlags.HideAndDontSave;
                }
                return _glMat;
            }
        }

        public static void DrawHorizontalGradient(Rect rect, Color[] colors, float[] positions)
        {
            if (colors == null || positions == null || colors.Length != positions.Length || colors.Length < 2)
            {
                Debug.LogError("Parameters for the gradient are incorrect");
                return;
            }

            Handles.BeginGUI();
            GL.PushMatrix();
            GL.LoadPixelMatrix();

            GLMat.SetPass(0);

            GL.Begin(GL.QUADS);

            for (int i = 0; i < colors.Length - 1; i++)
            {
                float x0 = Mathf.Lerp(rect.x, rect.xMax, positions[i]);
                float x1 = Mathf.Lerp(rect.x, rect.xMax, positions[i + 1]);

                Color c0 = colors[i];
                Color c1 = colors[i + 1];

                // Quad vertical (top to bottom)
                GL.Color(c0);
                GL.Vertex3(x0, rect.y, 0);
                GL.Vertex3(x0, rect.yMax, 0);

                GL.Color(c1);
                GL.Vertex3(x1, rect.yMax, 0);
                GL.Vertex3(x1, rect.y, 0);
            }

            GL.End();
            GL.PopMatrix();
            Handles.EndGUI();
        }
    }
}