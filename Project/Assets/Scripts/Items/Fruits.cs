using UnityEngine;
using System.Collections;

public class Fruits : Item
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

    private PlayerStatistics _stats;

    public Fruits(GameObject gameObject, float mass, string description, byte itemValue, PlayerStatistics stats)
    {
        this.Name = stats.Name;
        this.GType = GlobalType.Fruits;
        this.Mass = mass;
        this.Description = description;
        this.Count = 1;
        this.gameObject = gameObject;
        this.isUsable = true;
        this.ItemValue = itemValue;
        this._stats = stats;
        this.isStackable = true;
    }

    public void PickupItem()
    {
        if (Inventory.AddItemToInventory(this))
            MonoBehaviour.Destroy(gameObject);
    }
    public void Use()
    { 
        //Open and get the stuff inside the bag
        Player.Stats.Eat += _stats.Eat;
        Player.Stats.Drink += _stats.Drink;
        Player.Stats.Health += _stats.Health;
        Player.Stats.Mana += _stats.Mana;
        Player.Stats.Stamina += _stats.Stamina;
        Player.Stats.Experience += _stats.Experience;
        GlobalEventText.AddMessage(string.Format("You just ate a {0}", this.Name));
        if (this.Count == 1)
            Inventory.RemoveItemFromInventory(this);
        else
        {
            this.Count--;
            Inventory.RefreshUI();
        }
    }

    public object Clone()
    {
        Fruits clonedItem = new Fruits(null, Mass, Description, ItemValue, _stats);
        clonedItem.Type = Type;
        clonedItem.SortingLayer = SortingLayer;
        clonedItem.InventorySprite = InventorySprite;
        clonedItem.InGameSprite = InGameSprite;
        clonedItem.Count = Count;
        return clonedItem;
    }
}


public class FruitsStatistics : StatisticValues
{
    public static PlayerStatistics PearStats;
    public static PlayerStatistics AppleStats;
    public static PlayerStatistics CoconutStats;
    public static PlayerStatistics BananaStats;
    public static PlayerStatistics OrangeStats;
    public static PlayerStatistics Mushroom01Stats;

    public void Init()
    {
        //NAME//HEALTH//STAMINA//MANA//ARMOR//EXP//DRINK//EAT//STRENGTH//AGILITY//CHARISMA
        PearStats = new PlayerStatistics("Pear", 0.2f, 0.2f, 0.2f, 0, 0.1f, 1.7f, 4.0f, 0, 0, 0);
        AppleStats = new PlayerStatistics("Apple", 0.2f, 0.2f, 0.2f, 0, 0.1f, 0.7f, 5.0f, 0, 0, 0);
        CoconutStats = new PlayerStatistics("Coconut", 0.2f, 0.2f, 0.4f, 0, 0.1f, 7.0f, 0.2f, 0, 0, 0);
        BananaStats = new PlayerStatistics("Banana", 0.4f, 0.2f, 0.2f, 0, 0.1f, 0.2f, 7.0f, 0, 0, 0);
        OrangeStats = new PlayerStatistics("Orange", 0.2f, 0.4f, 0.2f, 0, 0.1f, 3.0f, 2.0f, 0, 0, 0);
        Mushroom01Stats = new PlayerStatistics("Green mushroom", 3f, 3f, -2f, 0, 0.1f, 0.5f, -2.0f, 0, 0, 0);
    }
}

