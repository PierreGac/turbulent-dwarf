using UnityEngine;
using System.Collections;

public class CrystalRaw : Item
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
    public static string PentaniteName = "Raw pentanite";
    public static string BwazariteName = "Raw bwazarite";

    public static string PentaniteDescription = "A shiny raw part of pentanite";
    public static string BwazariteDescription = "A shiny raw part of bwazarite";

    #endregion

    public static byte[] RawValues = {ItemValues.PentaniteRaw };


    public CrystalRaw(GameObject gameObject, byte itemValue)
    {
        this.GType = GlobalType.Crystals;
        this.Mass = 0.7f;
        this.Count = 1;
        this.gameObject = gameObject;
        this.isUsable = false;
        this.ItemValue = itemValue;
        this.isStackable = true;
        switch(itemValue)
        {
            case ItemValues.PentaniteRaw:
                InGameSprite = CrystalResources.IN_RawPentanite;
                InventorySprite = CrystalResources.IG_RawPentanite;
                this.Name = PentaniteName;
                this.Description = PentaniteDescription;
                this.Type = ItemType.RawPentanite;
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
        CrystalRaw clonedItem = new CrystalRaw(null, this.ItemValue);
        clonedItem.SortingLayer = SortingLayer;
        clonedItem.Count = Count;
        return clonedItem;
    }
}