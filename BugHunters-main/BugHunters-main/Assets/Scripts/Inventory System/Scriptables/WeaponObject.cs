using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponItemType
{
    Knife,
    Dagger,
    Sword,
    JaggedKnife
}

[CreateAssetMenu(fileName = "Weapon Object", menuName = "Inventory System/Item/Weapon")]
public class WeaponObject : ItemObject
{
    public int Range;
    public int AttackSpeed;
    public WeaponItemType WeaponItemType;
    public float[] DamageRanges = new float[4];
}
