using UnityEngine;
using System.Collections;

public class WhiteGemItem : Item
{
    public string Name { get; set; }
    public float Mass { get; set; }
    public Sprite InGameSprite { get; set; }
    public Sprite InventorySprite { get; set; }
    public int Count { get; set; }
    public GameObject gameObject { get; set; }
    public string Description { get; set; }

    public float Value = 150;

    public WhiteGemItem(GameObject gameObject)
    {
        this.Name = "WhiteGem";
        this.Mass = 1.0f;
        this.Description = "A huge white gem";
        this.Count = 1;
        this.gameObject = gameObject;
    }

    public void PickupItem()
    {
        if(Inventory.AddItemToInventory(this))
            MonoBehaviour.Destroy(gameObject);
    }
}

public class RedGemItem : Item
{
    public string Name { get; set; }
    public float Mass { get; set; }
    public Sprite InGameSprite { get; set; }
    public Sprite InventorySprite { get; set; }
    public int Count { get; set; }
    public GameObject gameObject { get; set; }
    public string Description { get; set; }

    public float Value = 150;

    public RedGemItem(GameObject gameObject)
    {
        this.Name = "RedGem";
        this.Mass = 1.0f;
        this.Description = "A huge red gem";
        this.Count = 1;
        this.gameObject = gameObject;
    }

    public void PickupItem()
    {
        if (Inventory.AddItemToInventory(this))
            MonoBehaviour.Destroy(gameObject);
    }
}

public class YellowGemItem : Item
{
    public string Name { get; set; }
    public float Mass { get; set; }
    public Sprite InGameSprite { get; set; }
    public Sprite InventorySprite { get; set; }
    public int Count { get; set; }
    public GameObject gameObject { get; set; }
    public string Description { get; set; }

    public float Value = 150;

    public YellowGemItem(GameObject gameObject)
    {
        this.Name = "YellowGem";
        this.Mass = 1.0f;
        this.Description = "A huge yellow gem";
        this.Count = 1;
        this.gameObject = gameObject;
    }

    public void PickupItem()
    {
        if (Inventory.AddItemToInventory(this))
            MonoBehaviour.Destroy(gameObject);
    }
}
