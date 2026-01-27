using UnityEngine;

namespace MVsToolkit.Utilities
{
    public static class MVsGizmos
    {
        public static void Draw2DCapsule(Vector2 center, Vector2 size, Color color)
        {
            Gizmos.color = color;

            float radius = Mathf.Min(size.x, size.y) * 0.5f;
            float height = Mathf.Max(size.x, size.y);
            float cylinderLength = height - radius * 2f;

            bool vertical = size.y > size.x;

            if (vertical)
            {
                Vector2 top = center + Vector2.up * (cylinderLength * 0.5f);
                Vector2 bottom = center + Vector2.down * (cylinderLength * 0.5f);

                Gizmos.DrawLine(top + Vector2.left * radius, bottom + Vector2.left * radius);
                Gizmos.DrawLine(top + Vector2.right * radius, bottom + Vector2.right * radius);

                DrawCircle(top, radius, Vector3.forward, color);
                DrawCircle(bottom, radius, Vector3.forward, color);
            }
            else
            {
                Vector2 right = center + Vector2.right * (cylinderLength * 0.5f);
                Vector2 left = center + Vector2.left * (cylinderLength * 0.5f);

                Gizmos.DrawLine(left + Vector2.up * radius, right + Vector2.up * radius);
                Gizmos.DrawLine(left + Vector2.down * radius, right + Vector2.down * radius);

                DrawCircle(left, radius, Vector3.forward, color);
                DrawCircle(right, radius, Vector3.forward, color);
            }
        }
        public static void DrawCircle(Vector3 center, float radius, Vector3 normal, Color color, int segments = 32)
        {
            if (normal == Vector3.zero)
                normal = Vector3.up;

            Gizmos.color = color;
            normal.Normalize();

            // Axes du plan
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

                Gizmos.DrawLine(prev, next);
                prev = next;
            }
        }
    }
}