using UnityEngine;
using System.Collections;

public class WhiteGemItem : Item
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
    public WhiteGemItem(GameObject gameObject)
    {
        this.Name = "WhiteGem";
        this.Mass = 1.0f;
        this.Description = "A huge white gem";
        this.Count = 1;
        this.gameObject = gameObject;
        this.isUsable = false;
        this.ItemValue = ItemValues.WhiteGem;
    }

    public void PickupItem()
    {
        if(Inventory.AddItemToInventory(this))
            MonoBehaviour.Destroy(gameObject);
    }
    public void Use() { }
}

public class RedGemItem : Item
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
    public RedGemItem(GameObject gameObject)
    {
        this.Name = "RedGem";
        this.Mass = 1.0f;
        this.Description = "A huge red gem";
        this.Count = 1;
        this.gameObject = gameObject;
        this.isUsable = false;
        this.ItemValue = ItemValues.RedGem;
    }

    public void PickupItem()
    {
        if (Inventory.AddItemToInventory(this))
            MonoBehaviour.Destroy(gameObject);
    }
    public void Use() { }
}

public class YellowGemItem : Item
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

    public YellowGemItem(GameObject gameObject)
    {
        this.Name = "YellowGem";
        this.Mass = 1.0f;
        this.Description = "A huge yellow gem";
        this.Count = 1;
        this.gameObject = gameObject;
        this.isUsable = false;
        this.ItemValue = ItemValues.YellowGem;
    }

    public void PickupItem()
    {
        if (Inventory.AddItemToInventory(this))
            MonoBehaviour.Destroy(gameObject);
    }
    public void Use() { }
}
