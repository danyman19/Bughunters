using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ClothesItemType
{
    Helmet,
    Chestplate,
    Boots,
}

[CreateAssetMenu(fileName = "Clothes Object", menuName = "Inventory System/Item/Clothes")]
[Serializable]
public class ClothesObject : ItemObject
{
    public int[] ShieldRanges = new int[4];
    public ClothesItemType ClothesItemType;
}
