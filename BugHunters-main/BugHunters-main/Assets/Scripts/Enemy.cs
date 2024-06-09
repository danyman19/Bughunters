using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public enum EnemyType
{
    Semicolon,
    Bracket,
    Overflow,
    Spawn0,
    Spawn1,
    Boss
}

[Serializable]
public class DropChance
{
    [SerializeField]
    public double Chance;
    [SerializeField]
    public ItemObject Drop;
}

public class Enemy : MonoBehaviour
{
    public float damage, speed, knockback, selfKnock, hitKnock;
    public float health;
    public EnemyType type;

    public Enemy parent;
    public int numSpawned;
    private Vector2 addVector;
    private Vector2 addVector2;

    private bool touch;
    private float time;
    private float lTime;

    private bool hit;
    private bool got;

    [SerializeField]
    public DropChance[] MobDrops;

    //public Dictionary<ItemObject, double> MobDrops = new Dictionary<ItemObject, double>();

    public Rigidbody2D _rb;

    private GameObject player;

    public void RemoveHealth(float h)
    {
        health -= h;
        if (health < 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Spawny guy minion die

        if (type == EnemyType.Spawn1 || type == EnemyType.Spawn0)
        {
            this.parent.numSpawned--;
        }

        if (type == EnemyType.Boss)
        {
            DungeonGeneration.Instance.last.addChest(DungeonGeneration.Instance.chestC);
            DungeonGeneration.Instance.last.addChest(DungeonGeneration.Instance.chestC);
            DungeonGeneration.Instance.last.addChest(DungeonGeneration.Instance.chestC);
            DungeonGeneration.Instance.last.addChest(DungeonGeneration.Instance.chestC);
            DungeonGeneration.Instance.last.addChest(DungeonGeneration.Instance.chestC);
        }

        float rand = UnityEngine.Random.Range(0f, 1f);
        foreach (DropChance dp in MobDrops)
        {
            if (rand < dp.Chance)
            {
                Vector2 spawnLoc = new Vector2(transform.position.x + UnityEngine.Random.Range(-0.5f, 0.5f), transform.position.y + UnityEngine.Random.Range(-0.5f, 0.5f));
                float[] rarityProbs = new float[] { 0.5f, 0.35f, 0.1f, 0.5f };

                Item item = new Item(dp.Drop) { Amount = 1 };
                if (dp.Drop is WeaponObject)
                {
                    item = new WeaponItem((WeaponObject)dp.Drop, rarityProbs);
                }
                else if (dp.Drop is ClothesObject)
                {
                    item = new ClothesItem((ClothesObject)dp.Drop, rarityProbs);
                }
                else if (dp.Drop is TrinketObject)
                {
                    item = new TrinketItem((TrinketObject)dp.Drop, rarityProbs);
                }
                else if (dp.Drop is HealingObject)
                {
                    item = new HealingItem((HealingObject)dp.Drop);
                }

                ItemWorld.SpawnItemWorld(spawnLoc, item);
            }
        }
        Destroy(gameObject);
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        float dev = UnityEngine.Random.Range(0.8f, 1.2f);
        speed *= dev;
    }

    void Update()
    {
        if ((player.transform.position - transform.position).magnitude < 10)
        {
            Swarm();

            if (type == EnemyType.Boss)
            {
                Horde();
            }
        }

        if (type == EnemyType.Overflow)
        {
            Spawn();
        }

        addVector *= 0.99f;
        addVector2 *= 0.99f;

        if (touch)
        {
            if (time > lTime + 0.25)
            {
                Damage();
            }
        }

        if (Player.Instance.attacking && !got && hit)
        {
            //register the hit
            RemoveHealth(PlayerStats.Instance.Damage);

            Vector2 difference = player.transform.position - transform.position;
            difference = Vector2.ClampMagnitude(difference, 1);

            addVector2 = difference * hitKnock * -1;

            got = true;
        }

        if (!Player.Instance.attacking) got = false;

        time += Time.deltaTime;
    }

    private void Horde()
    {
        if (UnityEngine.Random.Range(0, 1000) > 1) return;

        int rand = UnityEngine.Random.Range(0, 3);
        int num = 5;
        GameObject obj = EnemyGeneration.Instance.enemySemicolon;

        if (rand == 1)
        {
            obj = EnemyGeneration.Instance.enemyBracket;
            num = 3;
        }
        if (rand == 2)
        {
            obj = EnemyGeneration.Instance.enemyOverflow;
            num = 1;
        }

        for (int i = 0; i < num; i++) EnemyGeneration.Instance.generate(transform.position.x + UnityEngine.Random.Range(-1.0f, 1.0f), transform.position.y + UnityEngine.Random.Range(-1.0f, 1.0f), obj);
    }

    private void Spawn()
    {
        if (numSpawned >= 5) return;
        if (UnityEngine.Random.Range(0, 1000) > 3) return;

        GameObject obj = EnemyGeneration.Instance.spawn0;
        if (UnityEngine.Random.Range(0, 2) == 0) obj = EnemyGeneration.Instance.spawn1;
        GameObject baby = EnemyGeneration.Instance.generate(transform.position.x + UnityEngine.Random.Range(-1.0f, 1.0f), transform.position.y + UnityEngine.Random.Range(-1.0f, 1.0f), obj);
        baby.GetComponent<Enemy>().parent = this;
        numSpawned++;
    }

    private void Damage()
    {
        PlayerStats.Instance.TakeDamage(damage);

        if (type == EnemyType.Spawn0 || type == EnemyType.Spawn1)
        {
            parent.numSpawned--;

            Destroy(this.gameObject);

            return;
        }

        Vector2 difference = player.transform.position - transform.position;
        difference = Vector2.ClampMagnitude(difference, 1);

        player.GetComponent<PlayerMovement>().addVector = difference * knockback;

        addVector = difference * selfKnock * -1;

        lTime = time;
    }

    private void Swarm()
    {
        Vector2 inputVector = player.transform.position - transform.position;
        inputVector = Vector2.ClampMagnitude(inputVector, 1);


        Vector2 movementVector = inputVector * speed * Time.fixedDeltaTime + addVector + addVector2;
        _rb.MovePosition(_rb.position + movementVector);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            touch = true;
            time = 0;
            Damage();
        }

        if (collider.CompareTag("Attack"))
        {
            hit = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            touch = false;
        }

        if (collider.CompareTag("Attack"))
        {
            hit = false;
        }
    }
}
