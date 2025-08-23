using UnityEngine;

public class Crosshair : MonoBehaviour
{
    public enum CrosshairType
    {
        Circle,
        Cross,
        Dot
    }

    public CrosshairType crosshairType = CrosshairType.Circle;
    public float radius = 10f;
    public Color color = Color.white;
    public int thickness = 2;

    private Texture2D tex;

    private static bool visible = true;

    void Start()
    {
        tex = new Texture2D(1, 1);
        tex.SetPixel(0, 0, color);
        tex.Apply();
    }

    void OnGUI()
    {
        if (!visible) return;

        Vector2 center = new Vector2(Screen.width / 2f, Screen.height / 2f);

        switch (crosshairType)
        {
            case CrosshairType.Circle:
                DrawCircle(center, radius, thickness);
                break;
            case CrosshairType.Cross:
                DrawCross(center, radius, thickness);
                break;
            case CrosshairType.Dot:
                DrawDot(center, thickness);
                break;
        }
    }

    void DrawCircle(Vector2 center, float radius, int thickness)
    {
        int segments = Mathf.CeilToInt(2 * Mathf.PI * radius);
        for (int i = 0; i < segments; i++)
        {
            float angle = i * 2 * Mathf.PI / segments;
            Vector2 point = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius + center;

            GUI.DrawTexture(new Rect(point.x, point.y, thickness, thickness), tex);
        }
    }

    void DrawCross(Vector2 center, float size, int thickness)
    {

        GUI.DrawTexture(new Rect(center.x - size, center.y - thickness / 2, size * 2, thickness), tex);

        GUI.DrawTexture(new Rect(center.x - thickness / 2, center.y - size, thickness, size * 2), tex);
    }

    void DrawDot(Vector2 center, int size)
    {
        GUI.DrawTexture(new Rect(center.x - size / 2, center.y - size / 2, size, size), tex);
    }

    public static void Show()
    {
        visible = true;
    }

    public static void Hide()
    {
        visible = false;
    }

    public static void Toggle()
    {
        visible = !visible;
    }
}