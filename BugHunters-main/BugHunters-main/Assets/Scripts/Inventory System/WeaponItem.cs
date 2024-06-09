using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WeaponItem : PureItem
{
    public WeaponObject WeaponObject;

    public WeaponItem(WeaponObject weaponObject, float[] rarityProbs) : base(weaponObject, rarityProbs)
    {
        WeaponObject = weaponObject;
    }

    public override void StatusEffects(bool enable)
    {
        if (enable)
        {
            PlayerStats.Instance.AddDamage(WeaponObject.DamageRanges[(int)Rarity]);
        }
        else
        {
            PlayerStats.Instance.RemoveDamage(WeaponObject.DamageRanges[(int)Rarity]);
        }
    }

    public override string Tooltip()
    {
        string str = base.Tooltip();

        var index = str.IndexOf('\n', str.IndexOf('\n') + 1);
        var output = string.Concat(str.Substring(0, index), $"+{WeaponObject.DamageRanges[(int)Rarity]} DMG\n", str.Substring(index + 1));

        return output;
    }
}
