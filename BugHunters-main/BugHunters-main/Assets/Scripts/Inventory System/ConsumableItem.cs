using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableItem : Item
{
    public ConsumableObject ConsumableObject;

    public ConsumableItem(ConsumableObject consumableObject) : base(consumableObject) 
    {
        ConsumableObject = consumableObject;
    }

    public virtual void Consume()
    {
        StatusEffects(true);
    }

    public override string Tooltip()
    {
        string str = $"{Object.Name}<size=20>\n";
        str += "\n";
        str += $"Consumable";
        return str;
    }
}
