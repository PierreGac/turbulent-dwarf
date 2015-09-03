using UnityEngine;
using System.Collections;

public class Rocks : Item
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


    //STATIC NAMES
    #region Static names
    public static string GranitName = "Granit";
    public static string GabbroName = "Gabbro";
    public static string DioriteName = "Diorite";
    public static string BasaltName = "Basalt";
    #endregion
    public Rocks(GameObject gameObject, string name, byte itemValue)
    {
        this.Name = name;
        this.GType = GlobalType.Rocks;
        this.Mass = 2.0f;
        this.Description = string.Format("A {0} rock", name);
        this.Count = 1;
        this.gameObject = gameObject;
        this.isUsable = false;
        this.ItemValue = itemValue;
        this.isStackable = true;
        switch(itemValue)
        {
            case ItemValues.RockGranit:
                InGameSprite = ResourcesManager.instance.IG_Rock;
                InventorySprite = ResourcesManager.instance.IN_Rock;
                break;
            case ItemValues.RockGabbro:
                InGameSprite = ResourcesManager.instance.IG_Rock;
                InventorySprite = ResourcesManager.instance.IN_Rock;
                break;
            case ItemValues.RockBasalt:
                InGameSprite = ResourcesManager.instance.IG_Rock;
                InventorySprite = ResourcesManager.instance.IN_Rock;
                break;
            case ItemValues.RockDiorite:
                InGameSprite = ResourcesManager.instance.IG_Rock;
                InventorySprite = ResourcesManager.instance.IN_Rock;
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
        Rocks clonedItem = new Rocks(null, this.Name, this.ItemValue);
        clonedItem.Type = Type;
        clonedItem.SortingLayer = SortingLayer;
        clonedItem.Count = Count;
        return clonedItem;
    }
}