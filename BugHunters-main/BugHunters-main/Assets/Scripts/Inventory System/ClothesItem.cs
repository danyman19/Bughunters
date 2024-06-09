using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ClothesItem : PureItem
{
    public ClothesObject ClothesObject;

    public ClothesItem(ClothesObject clothesObject, float[] rarityProbs) : base(clothesObject, rarityProbs)
    {
        ClothesObject = clothesObject;
    }

    public override void StatusEffects(bool enable)
    {
        if (enable)
        {
            PlayerStats.Instance.AddDefense(ClothesObject.ShieldRanges[(int)Rarity]);
        }
        else
        {
            PlayerStats.Instance.RemoveDefense(ClothesObject.ShieldRanges[(int)Rarity]);
        }
    }

    public override string Tooltip()
    {
        string str = base.Tooltip();

        var index = str.IndexOf('\n', str.IndexOf('\n') + 1);
        var output = string.Concat(str.Substring(0, index), $"+{ClothesObject.ShieldRanges[(int)Rarity]} DEF\n", str.Substring(index + 1));

        return output;
    }
}
