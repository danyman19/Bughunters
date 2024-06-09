using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Tilemaps;

public class ChestLoot : MonoBehaviour
{
    public static ChestLoot Instance { get; private set; }

    public WeaponObject[] weapons;
    public ClothesObject[] clothes;
    public HealingObject[] heals;
    public TrinketObject[] trinkets;

    public float[] a, b, c;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        //rt = new int[,] { { 50, 3, 95, 100 }, { 20, 65, 90, 100 }, { 0, 10, 70, 100 } };
        a = new float[] { 0.5f, 0.35f, 0.1f, 0.5f };
        b = new float[]{0.2f, 0.45f, 0.25f, 0.1f};
        c = new float[] { 0f, 0.1f, 0.6f, 0.3f };
    }

    void Update()
    {
        
    }

    public void generateLoot(Tile chest, int x, int y) {
        if (chest == DungeonGeneration.Instance.chestA) generateLoot(x,y,0);
        else if (chest == DungeonGeneration.Instance.chestB) generateLoot(x,y,1);
        else if (chest == DungeonGeneration.Instance.chestC) generateLoot(x,y,2);
    }

    public void generateLoot(int x, int y, int q)
    {
        int num = 0;

        if (q == 0) num = 2;
        if (q == 1) num = 4;
        if (q == 2) num = 7;

        //ItemRarity rarity = ItemRarity.Common;
        //int gen = Random.Range(0, 100);

        //if (gen < rt[q,0]) rarity = ItemRarity.Common;
        //else if (gen < rt[q,1]) rarity = ItemRarity.Uncommon;
        //else if (gen < rt[q,2]) rarity = ItemRarity.Rare;
        //else if (gen < rt[q,3]) rarity = ItemRarity.Mythic;

        for (int i = 0; i < num; i++) generateRand(x, y, ihateandrey(q), Random.Range(1,4));
    }

    public float[] ihateandrey(int q) {
        if (q == 0) return a;
        if (q == 1) return b;
        return c;
    }

    public void generateRand(int x, int y, float[] r, int type) {
        if (type == 1) {
            int rand = Random.Range(0, weapons.Length);

            generateItem(x, y, new WeaponItem(weapons[rand], r));
        }
        if (type == 2)
        {
            int rand = Random.Range(0, clothes.Length);

            generateItem(x, y, new ClothesItem(clothes[rand], r));
        }
        if (type == 3)
        {
            int rand = Random.Range(0, heals.Length);

            generateItem(x, y, new HealingItem(heals[rand]));
        }

        if (type == 4)
        {
            int rand = Random.Range(0, trinkets.Length);

            generateItem(x, y, new TrinketItem(trinkets[rand], r));
        }
    }

    public void generateItem(int x, int y, Item i) {
        Debug.Log(i.Tooltip());

        ItemWorld.SpawnItemWorld(new Vector2(x + UnityEngine.Random.Range(-0.75f, 0.75f), y + UnityEngine.Random.Range(-0.75f, 0.75f)), i);
    }
}
