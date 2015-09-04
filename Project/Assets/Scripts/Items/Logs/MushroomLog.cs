using UnityEngine;
using System.Collections;

public class MushroomLog : Item
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
    public MushroomLog(GameObject gameObject)
    {
        this.Name = "Mushroom log";
        this.GType = GlobalType.Logs;
        this.Mass = 3.0f;
        this.Description = "A mushroom log. It smells good";
        this.Count = 1;
        this.gameObject = gameObject;
        this.isUsable = false;
        this.ItemValue = ItemValues.MushroomLog;
        this.isStackable = true;
        this.InGameSprite = ResourcesManager.instance.IG_MushroomLog;
        this.InventorySprite = ResourcesManager.instance.IN_MushroomLog;
    }

    public void PickupItem()
    {
        if (Inventory.AddItemToInventory(this))
            MonoBehaviour.Destroy(gameObject);
    }
    public void Use() { }

    public object Clone()
    {
        MushroomLog clonedItem = new MushroomLog(null);
        clonedItem.Type = Type;
        clonedItem.SortingLayer = SortingLayer;
        clonedItem.Count = Count;
        return clonedItem;
    }
}