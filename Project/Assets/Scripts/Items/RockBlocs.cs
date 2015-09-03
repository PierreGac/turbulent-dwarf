using UnityEngine;
using System.Collections;

public class RockBlocks : Item
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
    public static string GranitName = "Granit bloc";
    public static string GabbroName = "Gabbro bloc";
    public static string DioriteName = "Diorite bloc";
    public static string BasaltName = "Basalt bloc";
    #endregion


    public RockBlocks(GameObject gameObject, string name, byte itemValue)
    {
        this.Name = name;
        this.GType = GlobalType.Blocs;
        this.Mass = 10.0f;
        this.Description = string.Format("A {0}, it's heavy!", name);
        this.Count = 1;
        this.gameObject = gameObject;
        this.isUsable = false;
        this.ItemValue = itemValue;
        this.isStackable = true;
        switch(itemValue)
        {
            case ItemValues.BlocGranit:
                InGameSprite = ResourcesManager.instance.IN_Bloc;
                InventorySprite = ResourcesManager.instance.IG_Bloc;
                break;
            case ItemValues.BlocGabbro:
                InGameSprite = ResourcesManager.instance.IN_Bloc;
                InventorySprite = ResourcesManager.instance.IG_Bloc;
                break;
            case ItemValues.BlocBasalt:
                InGameSprite = ResourcesManager.instance.IN_Bloc;
                InventorySprite = ResourcesManager.instance.IG_Bloc;
                break;
            case ItemValues.BlocDiorite:
                InGameSprite = ResourcesManager.instance.IN_Bloc;
                InventorySprite = ResourcesManager.instance.IG_Bloc;
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
        RockBlocks clonedItem = new RockBlocks(null, this.Name, this.ItemValue);
        clonedItem.Type = Type;
        clonedItem.SortingLayer = SortingLayer;
        clonedItem.Count = Count;
        return clonedItem;
    }
}