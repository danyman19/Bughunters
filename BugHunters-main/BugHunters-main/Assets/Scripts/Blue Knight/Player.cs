using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using static UnityEditor.Progress;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    public bool IsInventoryOpen { get; private set; }

    [SerializeField] private Inventory inventory;

    public UI_Inventory UIInventory;
    public UI_EquipmentManager UIEquipment;
    public PlayerMovement PlayerMovement;
    public Animator Animator;

    public Transform UIInventorySprite;

    public bool attacking;
    public Transform AttackZone;
    private List<GameObject> AttackTargets = new();

    [SerializeField] private GameObject pickupIndicator;
    [SerializeField] private Transform Tooltip;
    [SerializeField] private RectTransform TooltipCanvas;
    public RectTransform Canvas;

    public event EventHandler OnInventoryUIOpen;
    public event EventHandler OnInventoryUIClose;

    private List<ItemWorld> nearestItems = new List<ItemWorld>();
    private ItemWorld nearestItem;

    public void HandleItemEquip(UI_ItemSlot itemSlot)
    {
        if (itemSlot.Item.Object.EquipmentSlotType == EquipmentSlotType.None) return;

        if (itemSlot.IsEquipped)
        {
            UIEquipment.UnequipItem(itemSlot.Item.Object.EquipmentSlotType);
        }
        else
        {
            // If target already has item
            if (UIEquipment.Slots.ContainsKey(itemSlot.Item.Object.EquipmentSlotType))
            {
                //itemSlot.Item.StatusEffects(false);
                UIEquipment.UnequipItem(itemSlot.Item.Object.EquipmentSlotType);
            }
            UIEquipment.EquipItem(itemSlot, itemSlot.Item.Object.EquipmentSlotType);
        }
    }

    public void HandleItemConsume(UI_ItemSlot itemSlot)
    {
        ConsumableItem consumable = (ConsumableItem)itemSlot.Item;
        DeleteItemFromSlot(itemSlot);

        consumable.Consume();
    }

    public void DropItem(UI_ItemSlot itemSlot)
    {
        UIEquipment.UnequipItem(itemSlot.Item.Object.EquipmentSlotType);
        DeleteItemFromSlot(itemSlot);

        ItemWorld.SpawnItemWorld(new Vector2(transform.position.x + UnityEngine.Random.Range(-0.75f, 0.75f), transform.position.y + UnityEngine.Random.Range(-0.75f, 0.75f)), itemSlot.Item);
    }

    private void DeleteItemFromSlot(UI_ItemSlot itemSlot)
    {
        if (itemSlot.TooltipOpen)
        {
            itemSlot.TooltipOpen = false;
            TooltipUpdate(null);
        }
        UIInventory.Inventory.RemoveItem(itemSlot.Item);
    }

    public void TooltipUpdate(UI_ItemMouseHandler handler)
    {
        if (handler == null)
        {
            Tooltip.gameObject.SetActive(false);
            return;
        }
        if (!Tooltip.gameObject.activeInHierarchy) 
        { 
            Tooltip.gameObject.SetActive(true);
            UI_ItemSlot itemSlot = handler.GetComponent<UI_ItemSlot>();
            itemSlot.TooltipOpen = true;
            TextMeshProUGUI text = Tooltip.GetChild(0).GetComponent<TextMeshProUGUI>();
            
            text.text = itemSlot.Item.Tooltip();
        }

        Tooltip.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void InventoryClose(object sender, EventArgs eventArgs)
    {
        Tooltip.gameObject.SetActive(false);
    }

    private void Awake()
    {
        Instance = this;

        inventory = new Inventory(16, 8, transform);
        UIInventory.SetInventory(inventory);

        OnInventoryUIOpen += UIInventory.UI_OnInventoryOpen;
        OnInventoryUIClose += UIInventory.UI_OnInventoryClose;
        OnInventoryUIClose += InventoryClose;

        PlayerStats.Instance.RefreshStatDisplay();
    }

    private void Update()
    {
        UpdateAttackZone();

        if (Input.GetKeyDown(KeyCode.E))
        {
            IsInventoryOpen = !UIInventory.gameObject.activeInHierarchy;
            UIInventorySprite.gameObject.SetActive(IsInventoryOpen);
            UIInventory.gameObject.SetActive(IsInventoryOpen);
            if(IsInventoryOpen)
            {
                OnInventoryUIOpen?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                OnInventoryUIClose?.Invoke(this, EventArgs.Empty);
            }
        }
        if (Input.GetKeyDown(KeyCode.F) && nearestItem != null)
        {
            GetComponent<AudioSource>().clip = UIInventory.PickupSound;
            GetComponent<AudioSource>().Play();

            pickupIndicator.transform.parent = null;
            inventory.AddItem(nearestItem.Item);
            nearestItem.DestroySelf();
        }

        if (Animator.GetCurrentAnimatorStateInfo(0).IsName("Attack Blend Tree"))
        {
            attacking = true;
        }
        else {
            attacking = false;
        }

        if ((Input.GetMouseButtonDown(0) || Input.GetKey(KeyCode.Space)) && !UIInventory.IsOpen && UIEquipment.Slots.ContainsKey(EquipmentSlotType.Sword))
        {
            GetComponent<AudioSource>().clip = UIInventory.SwingSound;
            GetComponent<AudioSource>().Play();
            Animator.Play("Attack Blend Tree");
        }
    }

    private void UpdateAttackZone()
    {
        float h = Animator.GetFloat("Horizontal");
        float v = Animator.GetFloat("Vertical");
        Vector3 rot = AttackZone.transform.rotation.eulerAngles;
        if (v > 0.1f)
        {
            rot.z = 0;
        }
        else if (v < -0.1f)
        {
            rot.z = 180;
        }
        else if (h < -0.1f)
        {
            rot.z = 90;
        }
        else if (h > 0.1f)
        {
            rot.z = 270;
        }
        AttackZone.transform.rotation = Quaternion.Euler(rot);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        ItemWorld itemWorld = other.GetComponent<ItemWorld>();
        if (itemWorld != null)
        {
            nearestItems.Add(itemWorld);
            CheckNearestItems();
        }
        else if(other.CompareTag("Enemy") && !AttackTargets.Contains(other.gameObject))
        {
            AttackTargets.Add(other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        ItemWorld itemWorld = other.GetComponent<ItemWorld>();
        if (itemWorld != null)
        {
            nearestItems.Remove(itemWorld);
            CheckNearestItems();
        }
        else if (other.CompareTag("Enemy"))
        {
            AttackTargets.Remove(other.gameObject);
        }
    }

    private void CheckNearestItems()
    {
        nearestItem = nearestItems.Count > 0 ? nearestItems[0] : null;
        foreach (ItemWorld item in nearestItems)
        {
            if (Vector2.Distance(transform.position, nearestItem.transform.position) - Vector2.Distance(transform.position, item.transform.position) > 0.001f)
            {
                nearestItem = item;
            }
        }

        if (nearestItem == null)
        {
            pickupIndicator.gameObject.SetActive(false);
            pickupIndicator.transform.parent = null;
        }
        else
        {
            pickupIndicator.gameObject.SetActive(true);
            pickupIndicator.transform.parent = nearestItem.transform;
            pickupIndicator.transform.localPosition = Vector2.zero;
        }
    }
}
