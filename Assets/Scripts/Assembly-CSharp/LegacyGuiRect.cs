using UnityEngine;
public static class LegacyGuiRect
{
    public static Rect Get(GameObject obj)
    {
        var t = obj.GetComponent<RectTransform>();
        return t == null ? new Rect() :
            new Rect(t.anchoredPosition.x, t.anchoredPosition.y,
                     t.sizeDelta.x, t.sizeDelta.y);
    }
    public static void Set(GameObject obj, Rect rect)
    {
        var t = obj.GetComponent<RectTransform>();
        if (t == null) return;
        t.anchoredPosition = new Vector2(rect.x, rect.y);
        t.sizeDelta = new Vector2(rect.width, rect.height);
    }
}
