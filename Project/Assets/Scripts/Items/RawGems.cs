using UnityEngine;
using System.Collections;

public class RawGems : Item
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
    public RawGems(GameObject gameObject)
    {
        this.Name = "Raw gem";
        this.GType = GlobalType.RawGems;
        this.Type = ItemType.RawGem;
        this.Mass = 1.0f;
        this.Description = "A raw gem. You have to cut it to find if it's valuable";
        this.Count = 1;
        this.Value = 12;
        this.gameObject = gameObject;
        this.isUsable = false;
        this.ItemValue = ItemValues.RawGem;
        this.isStackable = true;
        this.InGameSprite = ResourcesManager.instance.IG_RawGem;
        this.InventorySprite = ResourcesManager.instance.IN_RawGem;
    }

    public void PickupItem()
    {
        if (Inventory.AddItemToInventory(this))
            MonoBehaviour.Destroy(gameObject);
    }
    public void Use() { }

    public object Clone()
    {
        RawGems clonedItem = new RawGems(null);
        clonedItem.Type = Type;
        clonedItem.SortingLayer = SortingLayer;
        clonedItem.Count = Count;
        return clonedItem;
    }
}