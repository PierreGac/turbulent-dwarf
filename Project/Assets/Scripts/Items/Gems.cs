using UnityEngine;
using System.Collections;

public class Gems : Item
{
    public byte ItemValue { get; set; }
    public ItemType Type { get; set; }
    public GlobalType GType { get; set; }
    public string Name { get; set; }
    public float Mass { get; set; }
    public Sprite InGameSprite { get; set; }
    public Sprite InventorySprite { get; set; }
    public int Count { get; set; }
    public GameObject gameObject { get; set; }
    public string Description { get; set; }
    public bool isUsable { get; set; }
    public bool isStackable { get; set; }
    public float Value { get; set; }
    public int SortingLayer { get; set; }
    public Gems(GameObject gameObject, string name, string description, byte itemValue)
    {
        this.Name = name;
        this.GType = GlobalType.Gems;
        this.Mass = 1.0f;
        this.Description = description;
        this.Count = 1;
        this.gameObject = gameObject;
        this.isUsable = false;
        this.ItemValue = itemValue;
        this.isStackable = true;
        switch(itemValue)
        {
            case ItemValues.WhiteGem:
                InGameSprite = ResourcesManager.instance.IN_WhiteGem;
                InventorySprite = ResourcesManager.instance.IG_WhiteGem;
                break;
            case ItemValues.RedGem:
                InGameSprite = ResourcesManager.instance.IN_RedGem;
                InventorySprite = ResourcesManager.instance.IG_RedGem;
                break;
            case ItemValues.YellowGem:
                InGameSprite = ResourcesManager.instance.IN_YellowGem;
                InventorySprite = ResourcesManager.instance.IG_YellowGem;
                break;
        }
    }

    public void PickupItem()
    {
        if (Inventory.AddItemToInventory(this))
            MonoBehaviour.Destroy(gameObject);
    }
    public void Use() { }

    public object Clone()
    {
        Gems clonedItem = new Gems(null, this.Name, this.Description, this.ItemValue);
        clonedItem.Type = Type;
        clonedItem.SortingLayer = SortingLayer;
        clonedItem.Count = Count;
        return clonedItem;
    }
}

/*
public class WhiteGemItem : Item
{
    public byte ItemValue { get; set; }
    public ItemType Type { get; set; }
    public GlobalType GType { get; set; }
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
        this.GType = GlobalType.Gems;
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

    public object Clone()
    {
        WhiteGemItem clonedItem = new WhiteGemItem(null);
        clonedItem.Type = Type;
        clonedItem.SortingLayer = SortingLayer;
        clonedItem.InventorySprite = InventorySprite;
        clonedItem.InGameSprite = InGameSprite;
        clonedItem.Count = Count;
        return clonedItem;
    }
}

public class RedGemItem : Item
{
    public byte ItemValue { get; set; }
    public ItemType Type { get; set; }
    public GlobalType GType { get; set; }
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
        this.GType = GlobalType.Gems;
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

    public object Clone()
    {
        RedGemItem clonedItem = new RedGemItem(null);
        clonedItem.Type = Type;
        clonedItem.SortingLayer = SortingLayer;
        clonedItem.InventorySprite = InventorySprite;
        clonedItem.InGameSprite = InGameSprite;
        clonedItem.Count = Count;
        return clonedItem;
    }
}

public class YellowGemItem : Item
{
    public byte ItemValue { get; set; }
    public ItemType Type { get; set; }
    public GlobalType GType { get; set; }
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
        this.GType = GlobalType.Gems;
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

    public object Clone()
    {
        YellowGemItem clonedItem = new YellowGemItem(null);
        clonedItem.Type = Type;
        clonedItem.SortingLayer = SortingLayer;
        clonedItem.InventorySprite = InventorySprite;
        clonedItem.InGameSprite = InGameSprite;
        clonedItem.Count = Count;
        return clonedItem;
    }
}
*/