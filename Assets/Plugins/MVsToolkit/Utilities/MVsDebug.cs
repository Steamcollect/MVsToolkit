using UnityEngine;

namespace MVsToolkit.Utilities
{
    public static class MVsDebug
    {
        public static void Draw2DCapsule(Vector2 center, Vector2 size, Color color, float duration = 0f)
        {
            float radius = Mathf.Min(size.x, size.y) * 0.5f;
            float height = Mathf.Max(size.x, size.y);
            float cylinderLength = height - radius * 2f;

            bool vertical = size.y > size.x;

            if (vertical)
            {
                Vector2 top = center + Vector2.up * (cylinderLength * 0.5f);
                Vector2 bottom = center + Vector2.down * (cylinderLength * 0.5f);

                // Lignes verticales
                Debug.DrawLine(top + Vector2.left * radius, bottom + Vector2.left * radius, color, duration);
                Debug.DrawLine(top + Vector2.right * radius, bottom + Vector2.right * radius, color, duration);

                // Demi-cercles
                DrawCircle(top, radius, Vector3.forward, color, duration);
                DrawCircle(bottom, radius, Vector3.forward, color, duration);
            }
            else
            {
                Vector2 right = center + Vector2.right * (cylinderLength * 0.5f);
                Vector2 left = center + Vector2.left * (cylinderLength * 0.5f);

                Debug.DrawLine(left + Vector2.up * radius, right + Vector2.up * radius, color, duration);
                Debug.DrawLine(left + Vector2.down * radius, right + Vector2.down * radius, color, duration);

                DrawCircle(left, radius, Vector3.forward, color, duration);
                DrawCircle(right, radius, Vector3.forward, color, duration);
            }
        }

        public static void DrawCircle(Vector3 center, float radius, Vector3 normal, Color color, float duration = 0f, int segments = 32)
        {
            // Assure une normale valide
            if (normal == Vector3.zero)
                normal = Vector3.up;

            normal.Normalize();

            // Trouve deux axes orthogonaux au plan du cercle
            Vector3 tangent = Vector3.Cross(normal, Vector3.right);
            if (tangent == Vector3.zero)
                tangent = Vector3.Cross(normal, Vector3.up);

            tangent.Normalize();
            Vector3 bitangent = Vector3.Cross(normal, tangent);

            float step = Mathf.PI * 2f / segments;
            Vector3 prev = center + tangent * radius;

            for (int i = 1; i <= segments; i++)
            {
                float angle = step * i;
                Vector3 next =
                    center +
                    (tangent * Mathf.Cos(angle) + bitangent * Mathf.Sin(angle)) * radius;

                Debug.DrawLine(prev, next, color, duration);
                prev = next;
            }
        }
    }
}