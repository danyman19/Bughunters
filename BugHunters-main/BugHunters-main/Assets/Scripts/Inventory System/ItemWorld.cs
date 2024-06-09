using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWorld : MonoBehaviour
{
    [SerializeField]
    private Item item;
    public Item Item
    {
        get
        {
            return item;
        }
        set
        {
            item = value;
            spriteRenderer.sprite = item.Object.Sprite;
        }
    }
    
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    public static ItemWorld SpawnItemWorld(Vector3 position, Item item)
    {
        GameObject itemObject = Instantiate(item.Object.BaseObject, position, Quaternion.identity);
        
        if (item.Object.LightPrefab != null) Instantiate(item.Object.LightPrefab.transform, itemObject.transform);

        itemObject.GetComponent<SpriteRenderer>().sprite = item.Object.Sprite;

        ItemWorld itemWorld = itemObject.AddComponent<ItemWorld>();
        itemWorld.Item = item;

        return itemWorld;
    }
}
