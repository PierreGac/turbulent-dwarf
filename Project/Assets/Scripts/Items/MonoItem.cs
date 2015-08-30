using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public interface Item
{
    GameObject gameObject { get; set; }
    string Name { get; set; }
    string Description { get; set; }
    float Mass { get; set; }

    Sprite InGameSprite { get; set; }
    Sprite InventorySprite { get; set; }

    int Count { get; set; }

    void PickupItem();
}

public enum ItemType { Money, WhiteGem, YellowGem, RedGem};

public class MonoItem : MonoBehaviour
{
    public ItemType Type;
    public Item thisItem;

    public Sprite InventorySprite;

    void Start()
    {
        switch(Type)
        {
            case ItemType.Money:
                thisItem = new MoneyItem(gameObject);
            break;
            case ItemType.WhiteGem:
                thisItem = new WhiteGemItem(gameObject);
            break;
            case ItemType.YellowGem:
            thisItem = new YellowGemItem(gameObject);
            break;
            case ItemType.RedGem:
            thisItem = new RedGemItem(gameObject);
            break;
        }

        thisItem.InventorySprite = InventorySprite;
    }
}
