using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

[Serializable]
public class HealingItem : ConsumableItem
{
    public HealingObject HealingObject;

    public HealingItem(HealingObject healingObject) : base(healingObject)
    {
        HealingObject = healingObject;
    }

    public override void Consume()
    {
        base.Consume();
        PlayerStats.Instance.Heal(HealingObject.HealAmount);
    }
    public override string Tooltip()
    {
        string str = base.Tooltip();

        var index = str.IndexOf('\n', str.IndexOf('\n') + 1);
        var output = string.Concat(str.Substring(0, index), $"+{HealingObject.HealAmount} HP\n", str.Substring(index + 1));
        
        return output;
    }
}
