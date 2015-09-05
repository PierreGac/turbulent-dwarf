using UnityEngine;
using System.Collections;

public class CrystalPowder : Item
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
    public static string PentaniteName = "Pentanite powder";
    public static string BwazariteName = "Bwazarite powder";

    public static string PentaniteDescription = "A pile of pentanite powder";
    public static string BwazariteDescription = "A pile of bwazarite powder";

    #endregion

    public static byte[] PowderValues = {ItemValues.PentanitePowder };


    public CrystalPowder(GameObject gameObject, byte itemValue)
    {
        this.GType = GlobalType.Powders;
        this.Mass = 0.2f;
        this.Count = 1;
        this.gameObject = gameObject;
        this.isUsable = false;
        this.ItemValue = itemValue;
        this.isStackable = true;
        switch(itemValue)
        {
            case ItemValues.PentanitePowder:
                InGameSprite = PowderResources.IG_PentanitePowder;
                InventorySprite = PowderResources.IN_PentanitePowder;
                this.Name = PentaniteName;
                this.Description = PentaniteDescription;
                this.Type = ItemType.PentanitePowder;
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
        CrystalPowder clonedItem = new CrystalPowder(null, this.ItemValue);
        clonedItem.SortingLayer = SortingLayer;
        clonedItem.Count = Count;
        return clonedItem;
    }
}