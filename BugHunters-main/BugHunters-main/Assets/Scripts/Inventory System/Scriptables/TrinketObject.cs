using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TrinketItemType
{
    Necklace,
    Ring,
    Braclet,
}

public enum StatusEffectType
{
    Damage,
    Health,
    Shield,
    Speed
}

[CreateAssetMenu(fileName = "Trinket Object", menuName = "Inventory System/Item/Trinket")]
public class TrinketObject : ItemObject
{
    public float[] DamageRanges = new float[4];
    public float[] HealthRanges = new float[4];
    public float[] ShieldRanges = new float[4];
    public float[] SpeedRanges = new float[4];
    public TrinketItemType TrinketItemType;
    public StatusEffectType[] StatusEffectTypes;
}
