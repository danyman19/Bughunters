using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item
{
    [SerializeField]
    public ItemObject Object;
    public int Amount = 1;
    
    public Item(ItemObject itemObject)
    {
        Object = itemObject;
    }

    public virtual string Tooltip()
    {
        return Object.Name;
    }

    public virtual void StatusEffects(bool enable)
    {

    }
}
