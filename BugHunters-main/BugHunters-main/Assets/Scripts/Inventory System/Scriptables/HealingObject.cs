using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HealingObjectType
{
    Semiscale,
}

[CreateAssetMenu(fileName = "Healing Object", menuName = "Inventory System/Item/Healable")]
[Serializable]
public class HealingObject : ConsumableObject
{
    public float HealAmount;
    public HealingObjectType HealingObjectType;
}
