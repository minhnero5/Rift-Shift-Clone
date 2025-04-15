using UnityEngine;
using UnityEngine.EventSystems;

public class DraggablePanel : MonoBehaviour, IDragHandler, IBeginDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Có th? dùng ?? chu?n b? n?u c?n
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (canvas == null) return;

        // Di chuy?n theo chu?t, tính theo scale c?a canvas
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }
}