using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestTrigger : MonoBehaviour
{
    private bool hit;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (hit && Player.Instance.attacking) {
            int x = (int)transform.position.x;
            int y = (int)transform.position.y;

            ChestLoot.Instance.generateLoot(DungeonGeneration.Instance.chestTypes[new Vector3Int(x, y)], x, y);
            DungeonGeneration.Instance.tileMap.SetTile(new Vector3Int(x - 3, y - 2), DungeonGeneration.Instance.ground);

            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Attack"))
        {
            hit = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Attack"))
        {
            hit = false;
        }
    }
}
