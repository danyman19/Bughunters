/*
 *  Author: ariel oliveira [o.arielg@gmail.com]
 */

using TMPro;
using UnityEngine;

using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    public delegate void OnHealthChangedDelegate();
    public OnHealthChangedDelegate onHealthChangedCallback;

    #region Sigleton
    private static PlayerStats instance;
    public static PlayerStats Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<PlayerStats>();
            return instance;
        }
    }
    #endregion

    [SerializeField]
    private float health;
    [SerializeField]
    private float maxHealth;
    [SerializeField]
    private float maxTotalHealth;
    [SerializeField]
    private float defense;
    [SerializeField]
    private float damage;
    [SerializeField]
    private float speed;

    public TextMeshProUGUI StatDisplay;

    public float Health { get { return health; } }
    public float MaxHealth { get { return maxHealth; } }
    public float MaxTotalHealth { get { return maxTotalHealth; } }
    public float Defense { get { return defense; } }
    public float Damage { get { return damage; } }
    public float Speed { get { return speed; } }

    public void Heal(float health)
    {
        this.health += health;
        ClampHealth();
    }

    public void TakeDamage(float dmg)
    {
        dmg *= (1 - (0.04f * defense));
        health -= dmg;

        if(health <=0) SceneManager.LoadScene(2);

        ClampHealth();
    }

    public void AddHealth(int add)
    {
        if (maxHealth + add < maxTotalHealth)
        {
            maxHealth += add;

            ClampHealth();

            if (onHealthChangedCallback != null)
                onHealthChangedCallback.Invoke();
        }   
    }

    public void RemoveHealth(int rem)
    {
        if (maxHealth - rem >= 0)
        {
            maxHealth -= rem;

            ClampHealth();

            if (onHealthChangedCallback != null)
                onHealthChangedCallback.Invoke();
        }
    }

    public void AddSpeed(float add)
    {
        speed += add;
        RefreshStatDisplay();
    }

    public void RemoveSpeed(float add)
    {
        speed -= add;
        RefreshStatDisplay();
    }

    public void AddDamage(float add)
    {
        damage += add;
        RefreshStatDisplay();
    }
    public void RemoveDamage(float rem)
    {
        damage -= rem;
        RefreshStatDisplay();
    }
    public void AddDefense(float add)
    {
        defense += add;
        RefreshStatDisplay();
    }

    public void RemoveDefense(float rem)
    {
        defense -= rem;
        RefreshStatDisplay();
    }

    void ClampHealth()
    {
        health = Mathf.Clamp(health, 0, maxHealth);

        if (onHealthChangedCallback != null)
            onHealthChangedCallback.Invoke();
    }

    public void RefreshStatDisplay()
    {
        StatDisplay.text =  $"{damage} DMG\n{defense} DEF\n{speed} SPD\n{maxHealth} MAX HP";
    }
}
