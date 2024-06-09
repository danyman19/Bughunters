using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_ItemMouseHandler : MonoBehaviour, IPointerClickHandler
{
    public RectTransform Transform;

    private bool hadHoverLastFrame = false;

    private void Update()
    {
        Vector2 point;
        Vector2 mousePos = Input.mousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(Transform, mousePos, Camera.main, out point);

        if (Transform.rect.Contains(point))
        {
            hadHoverLastFrame = true;
            Player.Instance.TooltipUpdate(this);
        }
        else if (hadHoverLastFrame)
        {
            hadHoverLastFrame = false;
            Player.Instance.TooltipUpdate(null);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Player.Instance.UIInventory.HandleUIItemClick(this, eventData);
    }
}
