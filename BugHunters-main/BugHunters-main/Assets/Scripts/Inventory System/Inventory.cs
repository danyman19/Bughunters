using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public class Inventory
{
    public int Slots { get; private set; }
    public int RowSize { get; private set; }
    public Transform Transform { get; private set; }

    [SerializeField]
    public List<Item> Items { get; private set; }

    [SerializeField]
    public event EventHandler<(int, Item)> OnItemListAdd;
    public event EventHandler<(int, Item)> OnItemListRemove;

    public Inventory(int slots, int rowSize, Transform transform)
    {
        Slots = slots;
        RowSize = rowSize;
        Transform = transform;

        Items = new List<Item>();
    }

    public void RemoveItem(Item item)
    {
        int idx = Items.IndexOf(item);
        if (idx == -1) return;

        Items.RemoveAt(idx);
        OnItemListRemove?.Invoke(this, (idx, item));
    }

    public void AddItem(Item add)
    {
        for (int i = 0; i < add.Amount; i++)
        {
            Item singleItem = add;
            singleItem.Amount = 1;
            if (Items.Count + 1 > Slots)
            {
                ItemWorld.SpawnItemWorld(new Vector2(Transform.position.x + UnityEngine.Random.Range(-0.75f, 0.75f), Transform.position.y + UnityEngine.Random.Range(-0.75f, 0.75f)), singleItem);
            }
            else
            {
                Items.Add(singleItem);
                OnItemListAdd?.Invoke(this, (Items.Count - 1, singleItem));
            }
        }
    }
}
