using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class TrinketItem : PureItem
{
    public TrinketObject TrinketObject;

    public TrinketItem(TrinketObject trinketObject, float[] rarityProbs) : base(trinketObject, rarityProbs)
    {
        TrinketObject = trinketObject;
    }

    public override void StatusEffects(bool enable)
    {
        if (enable)
        {
            foreach (StatusEffectType type in TrinketObject.StatusEffectTypes)
            {
                switch (type)
                {
                    case StatusEffectType.Damage:
                        PlayerStats.Instance.AddDamage(TrinketObject.DamageRanges[(int)Rarity]);
                        break;
                    case StatusEffectType.Shield:
                        PlayerStats.Instance.AddDefense(TrinketObject.DamageRanges[(int)Rarity]);
                        break;
                    case StatusEffectType.Health:
                        PlayerStats.Instance.AddHealth((int)TrinketObject.HealthRanges[(int)Rarity]);
                        break;
                    case StatusEffectType.Speed:
                        PlayerStats.Instance.AddSpeed(TrinketObject.SpeedRanges[(int)Rarity]);
                        break;
                }
            }
        }
        else
        {
            foreach (StatusEffectType type in TrinketObject.StatusEffectTypes)
            {
                switch (type)
                {
                    case StatusEffectType.Damage:
                        PlayerStats.Instance.RemoveDamage(TrinketObject.DamageRanges[(int)Rarity]);
                        break;
                    case StatusEffectType.Shield:
                        PlayerStats.Instance.RemoveDefense(TrinketObject.DamageRanges[(int)Rarity]);
                        break;
                    case StatusEffectType.Health:
                        PlayerStats.Instance.RemoveHealth((int)TrinketObject.HealthRanges[(int)Rarity]);
                        break;
                    case StatusEffectType.Speed:
                        PlayerStats.Instance.RemoveSpeed(TrinketObject.SpeedRanges[(int)Rarity]);
                        break;
                }
            }
        }
    }

    public override string Tooltip()
    {
        string str = base.Tooltip();
        var output = "";
        var index = str.IndexOf('\n', str.IndexOf('\n') + 1);

        string add = "";
        foreach(StatusEffectType type in TrinketObject.StatusEffectTypes)
        {
            switch (type)
            {
                case StatusEffectType.Damage:
                    add += $"+{TrinketObject.DamageRanges[(int)Rarity]} DMG ";
                    break;
                case StatusEffectType.Shield:
                    add += $"+{TrinketObject.ShieldRanges[(int)Rarity]} DEF ";
                    break;
                case StatusEffectType.Health:
                    add += $"+{(int)TrinketObject.HealthRanges[(int)Rarity]} HP ";
                    break;
                case StatusEffectType.Speed:
                    add += $"+{TrinketObject.SpeedRanges[(int)Rarity]} SPD ";
                    break;
            }
        }

        output += string.Concat(str.Substring(0, index), $"{add}\n", str.Substring(index + 1));

        return output;
    }
}
