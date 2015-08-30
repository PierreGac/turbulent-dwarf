using UnityEngine;
using System.Collections;

public class MoneyItem : Item
{
    public string Name { get; set; }
    public float Mass { get; set; }
    public Sprite InGameSprite { get; set; }
    public Sprite InventorySprite { get; set; }
    public int Count { get; set; }
    public GameObject gameObject { get; set; }
    public string Description { get; set; }

    public float Value = 50;

    public MoneyItem(GameObject gameObject)
    {
        this.Name = "Money";
        this.Mass = 0.0f;
        this.Description = "Money, used for trading";
        this.gameObject = gameObject;
    }

    public void PickupItem()
    {
        Inventory.Money += Value;
        GlobalEventText.AddMessage(string.Format("You picked up {0} gold !", Value));
        MonoBehaviour.Destroy(gameObject);
    }
}
