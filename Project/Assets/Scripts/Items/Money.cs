﻿using UnityEngine;
using System.Collections;

public class MoneyItem : Item
{
    public byte ItemValue { get; set; }
    public ItemType Type { get; set; }
    public string Name { get; set; }
    public float Mass { get; set; }
    public Sprite InGameSprite { get; set; }
    public Sprite InventorySprite { get; set; }
    public int Count { get; set; }
    public GameObject gameObject { get; set; }
    public string Description { get; set; }
    public bool isUsable { get; set; }

    public float Value { get; set; }
    public int SortingLayer { get; set; }
    public MoneyItem(GameObject gameObject)
    {
        this.Name = "Money";
        this.Mass = 0.0f;
        this.Description = "Money, used for trading";
        this.gameObject = gameObject;
        this.isUsable = false;
        this.Value = Random.Range(50, 200);
        this.ItemValue = ItemValues.Money;
    }

    public void PickupItem()
    {
        Inventory.Money += Value;
        GlobalEventText.AddMessage(string.Format("You picked up {0} gold !", Value));
        MonoBehaviour.Destroy(gameObject);
    }

    public void Use()
    {
        
    }
}
