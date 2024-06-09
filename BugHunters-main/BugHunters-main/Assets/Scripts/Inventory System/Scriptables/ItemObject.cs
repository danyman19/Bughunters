using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum ItemType
{
    None,
    Weapon,
    Clothes,
    Trinkets,
    Consumable,
}

[Serializable]
[CreateAssetMenu(fileName = "Item Object", menuName = "Inventory System/Item/Generic")]
public class ItemObject : ScriptableObject
{
    public string Name;
    public Sprite Sprite;
    public GameObject BaseObject;
    public GameObject LightPrefab;
    public ItemType Type;
    public EquipmentSlotType EquipmentSlotType;
    [TextArea(15, 20)]
    public string Description;
}
