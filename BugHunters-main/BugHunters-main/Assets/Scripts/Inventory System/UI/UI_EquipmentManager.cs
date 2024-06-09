using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentSlotType
{
    None = 0,
    Helmet,
    Shield,
    Armor,
    Boots,
    Sword,
    Trincket
}

public class UI_EquipmentManager : MonoBehaviour
{
    private readonly Dictionary<EquipmentSlotType, Vector2> slotOffsets = new Dictionary<EquipmentSlotType, Vector2>
    {
        { EquipmentSlotType.Helmet, new(0, 0) },
        { EquipmentSlotType.Shield, new(1, 0) },
        { EquipmentSlotType.Armor, new(0, -1) },
        { EquipmentSlotType.Boots, new(1, -1) },
        { EquipmentSlotType.Sword, new(0, -2) },
        { EquipmentSlotType.Trincket, new(1, -2) },
    };

    public Dictionary<EquipmentSlotType, (UI_ItemSlot, RectTransform)> Slots = new Dictionary<EquipmentSlotType, (UI_ItemSlot, RectTransform)>();

    [SerializeField] private Transform itemSlotTemplate;
    [SerializeField] private Transform blankOutSlotTemplate;

    public void EquipItem(UI_ItemSlot inventoryItemSlot, EquipmentSlotType slotType)
    {
        inventoryItemSlot.SetEquipped(true);

        RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplate, transform).GetComponent<RectTransform>();

        Instantiate(blankOutSlotTemplate, itemSlotRectTransform).GetComponent<RectTransform>();

        itemSlotRectTransform.anchoredPosition = slotOffsets[slotType] * UI_Inventory.ItemSlotOffset;

        // Should be dummy item, so remove UI_ItemSlot and UI_ItemClickHandler
        Destroy(itemSlotRectTransform.GetComponent<UI_ItemSlot>());
        Destroy(itemSlotRectTransform.GetComponent<UI_ItemMouseHandler>());

        SpriteRenderer uiItemSlotRenderer = itemSlotRectTransform.GetComponent<SpriteRenderer>();
        Sprite uiItemSprite = inventoryItemSlot.Item.Object.Sprite;

        uiItemSlotRenderer.sprite = uiItemSprite;

        Slots.Add(slotType, (inventoryItemSlot, itemSlotRectTransform));
    }

    public void UnequipItem(EquipmentSlotType slotType)
    {
        if (!Slots.ContainsKey(slotType)) return;
        
        Slots[slotType].Item1.SetEquipped(false);
        
        Destroy(Slots[slotType].Item2.gameObject);
        Slots.Remove(slotType);
    }
}
