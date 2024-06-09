using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingItemSpawner : MonoBehaviour
{
    public Item[] item;
    public ClothesItem[] clothes;
    public HealingItem[] healings;
    public WeaponItem[] weapons;
    public TrinketItem[] trinkets;

    void Start()
    {
        foreach (var item in item)
        {
            Player.Instance.UIInventory.Inventory.AddItem(item);
        }
        foreach (var item in clothes)
        {
            Player.Instance.UIInventory.Inventory.AddItem(item);
        }
        foreach (var item in healings)
        {
            Player.Instance.UIInventory.Inventory.AddItem(item);
        }
        foreach (var item in weapons)
        {
            Player.Instance.UIInventory.Inventory.AddItem(item);
        }
        foreach (var item in trinkets)
        {
            Player.Instance.UIInventory.Inventory.AddItem(item);
        }
    }
}
