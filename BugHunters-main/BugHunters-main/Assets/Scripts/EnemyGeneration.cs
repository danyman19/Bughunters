using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGeneration : MonoBehaviour
{
    public static EnemyGeneration Instance { get; private set; }

    public GameObject enemySemicolon, enemyBracket, enemyOverflow, spawn0, spawn1, boss;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        //generate(0, 2, enemySemicolon);
    }

    void Update()
    {

    }

    public GameObject generate(float x, float y, GameObject type)
    {
        GameObject o = Instantiate(type);

        o.transform.position = new Vector2(x, y);

        return o;
    }

    public void generateEnemies()
    {
        foreach (DungeonGeneration.Room r in DungeonGeneration.Instance.rooms)
        {
            if (r.treasure) continue;

            DungeonGeneration.RoomTemplate template = r.template;

            for (int i = 0; i < template.width; i++)
            {
                for (int j = 0; j < template.height; j++)
                {
                    if (template.pixels[i, j] == 0)
                    {
                        int rand = Random.Range(1, 100);
                        int gx = r.x + i + 3;
                        int gy = r.y + j + 2;

                        if (rand > 2 + r.danger * 0.25) continue;

                        rand = Random.Range(1, 4);
                        if (r.danger < 7) rand = Random.Range(1, 3);

                        if (rand == 1) generate(gx, gy, enemySemicolon);
                        if (rand == 2) generate(gx, gy, enemyBracket);
                        if (rand == 3) generate(gx, gy, enemyOverflow);
                    }
                }
            }
        }
    }
}
