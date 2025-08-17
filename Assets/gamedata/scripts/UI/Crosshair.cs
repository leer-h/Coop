using UnityEngine;

public class Crosshair : MonoBehaviour
{
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
        DrawCircle(center, radius, thickness);
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
