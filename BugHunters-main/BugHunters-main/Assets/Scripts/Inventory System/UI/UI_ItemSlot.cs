using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ItemSlot : MonoBehaviour
{
    public Item Item;
    public bool TooltipOpen;
    public bool IsEquipped { get; private set; }

    public void SetEquipped(bool isEquipped)
    {
        IsEquipped = isEquipped;

        Item.StatusEffects(isEquipped);

        Refresh();
    }

    public void Refresh()
    {
        Color color = GetComponent<SpriteRenderer>().color;
        color.a = IsEquipped ? 0.3f : 1f;
        GetComponent<SpriteRenderer>().color = color;
    }

    public void RemoveItem()
    {
        TooltipOpen = false;
    }
}
