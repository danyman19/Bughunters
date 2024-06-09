using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;

public class UI_Inventory : MonoBehaviour
{
    [SerializeField] private Transform itemSlotContainer;
    [SerializeField] private Transform itemSlotTemplate;

    public Inventory Inventory;
    public List<UI_ItemSlot> ItemSlots;

    public bool IsOpen;

    public static readonly float ItemSlotOffset = 96f;

    public AudioClip ConsumeSound;
    public AudioClip EquipSound;
    public AudioClip DropSound;
    public AudioClip PickupSound;
    public AudioClip SwingSound;

    public void HandleUIItemClick(UI_ItemMouseHandler handler, PointerEventData eventData)
    {
        UI_ItemSlot itemSlot = handler.GetComponent<UI_ItemSlot>();
        
        // Left click (Equip)
        if (eventData.pointerId == -1)
        {
            if (Input.GetKey(KeyCode.LeftShift) && (itemSlot.Item is ConsumableItem)) // Consume
            {
                Player.Instance.GetComponent<AudioSource>().clip = ConsumeSound;
                Player.Instance.GetComponent<AudioSource>().Play();
                Player.Instance.HandleItemConsume(itemSlot);
            }
            else // Equip
            {
                Player.Instance.GetComponent<AudioSource>().clip = EquipSound;
                Player.Instance.GetComponent<AudioSource>().Play();
                Player.Instance.HandleItemEquip(itemSlot);
            }
            
        }
        else if (eventData.pointerId == -2)
        {
            Player.Instance.GetComponent<AudioSource>().clip = DropSound;
            Player.Instance.GetComponent<AudioSource>().Play();
            Player.Instance.DropItem(itemSlot);
        }
    }

    public void SetInventory(Inventory inventory)
    {
        inventory.OnItemListAdd += UI_Additem;
        inventory.OnItemListRemove += UI_RemoveItem;

        Inventory = inventory;
    }

    public void UI_Additem(object sender, (int, Item) slot)
    {
        int x = slot.Item1 % Inventory.RowSize;
        int y = -(int)(slot.Item1 / Inventory.RowSize);

        RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
        itemSlotRectTransform.gameObject.SetActive(true);
        itemSlotRectTransform.anchoredPosition = new Vector2(x * ItemSlotOffset, y * ItemSlotOffset);

        UI_ItemSlot itemSlot = itemSlotRectTransform.GetComponent<UI_ItemSlot>();
        itemSlot.Item = slot.Item2;

        itemSlot.Refresh();
        ItemSlots.Add(itemSlot);

        SpriteRenderer uiItemSlotRenderer = itemSlotRectTransform.GetComponent<SpriteRenderer>();
        Sprite uiItemSprite = slot.Item2.Object.Sprite;

        uiItemSlotRenderer.sprite = uiItemSprite;
    }

    public void UI_RemoveItem(object sender, (int, Item) slot)
    {
        UI_ItemSlot removedSlot = ItemSlots[slot.Item1];
        
        ItemSlots.RemoveAt(slot.Item1);
        for (int i = slot.Item1; i < ItemSlots.Count; i++)
        {
            int x = i % Inventory.RowSize;
            int y = -(int)(i / Inventory.RowSize);

            RectTransform itemSlotRectTransform = ItemSlots[i].GetComponent<RectTransform>();
            itemSlotRectTransform.anchoredPosition = new Vector2(x * ItemSlotOffset, y * ItemSlotOffset);
        }

        Destroy(removedSlot.gameObject);
    }

    public void UI_OnInventoryOpen(object sender, EventArgs e)
    {
        PlayerStats.Instance.StatDisplay.gameObject.SetActive(true);
        IsOpen = true;
    }

    public void UI_OnInventoryClose(object sender, EventArgs e)
    {
        PlayerStats.Instance.StatDisplay.gameObject.SetActive(false);
        IsOpen = false;
    }
}
