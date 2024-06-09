using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum ItemRarity
{
    Common = 0,
    Uncommon,
    Rare,
    Mythic,
}

[Serializable]
public class PureItem : Item
{
    public ItemRarity Rarity;

    public static readonly Dictionary<ItemRarity, string> RarityColors = new Dictionary<ItemRarity, string>()
    {
        {ItemRarity.Common, "white"},
        {ItemRarity.Uncommon, "green"},
        {ItemRarity.Rare, "blue"},
        {ItemRarity.Mythic, "yellow"},
    };

    public PureItem(ItemObject itemObject, float[] rarityProbs) : base(itemObject) 
    {
        GenerateRarities(rarityProbs);
    }

    public override string Tooltip()
    {
        string str = $"<color={RarityColors[Rarity]}>{Object.Name}</color><size=20>\n";
        str += "\n";
        str += $"{Rarity}";
        return str;
    }

    public ItemRarity GenerateRarities(float[] probs)
    {
        ItemRarity[] rarities = RarityColors.Keys.Take(probs.Length).ToArray();
        return GenerateRarities(probs, rarities);
    }

    public ItemRarity GenerateRarities(float[] probs, ItemRarity[] rarities)
    {
        this.Rarity = ItemRarity.Common;
        
        float r = UnityEngine.Random.value;

        float sum = 0;
        for (int i = 0; i < probs.Length; i++)
        {
            if (r > sum && r < sum + probs[i])
            {
                int purity = UnityEngine.Random.Range(0, 100);
                this.Rarity = rarities[i];
            }

            sum += probs[i];
        }

        return (this.Rarity);
    }
}
