using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public interface Item
{
    byte ItemValue { get; set; }
    ItemType Type { get; set; }
    GameObject gameObject { get; set; }
    string Name { get; set; }
    string Description { get; set; }
    float Mass { get; set; }
    bool isUsable { get; set; }
    Sprite InGameSprite { get; set; }
    Sprite InventorySprite { get; set; }
    float Value { get; set; }
    int Count { get; set; }
    int SortingLayer { get; set; }

    void PickupItem();

    void Use();
}

public interface ItemContainer : Item
{
    ItemType[] AllowedContent { get; set; }
    Item[] Content { get; set; }
    void SetContent(params Item[] items);
}

public enum ItemType { Money, WhiteGem, YellowGem, RedGem, RockBoulder, Bag01};

public class MonoItem : MonoBehaviour
{
    public ItemType Type;
    public Item thisItem;
    [HideInInspector]
    public bool isJustSpawned = false;

    public Sprite InventorySprite;

    void Awake()
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
            case ItemType.RockBoulder:
                thisItem = new Boulders(gameObject);
            break;
            case ItemType.Bag01:
                thisItem = new Bag01(gameObject);
            break;
        }
        thisItem.Type = Type;
        thisItem.SortingLayer = GetComponent<SpriteRenderer>().sortingLayerID;
        thisItem.InventorySprite = InventorySprite;
        thisItem.InGameSprite = GetComponent<SpriteRenderer>().sprite;
    }

    public Item GetItem()
    {
        switch (Type)
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
            case ItemType.RockBoulder:
                thisItem = new Boulders(gameObject);
                break;
        }

        thisItem.InventorySprite = InventorySprite;
        return thisItem;
    }

    public static GameObject CreateGameObjectFromItem(Item item)
    {
        GameObject obj = new GameObject(item.Name);

        MonoItem monoItem = obj.AddComponent<MonoItem>();
        monoItem.thisItem = item;
        monoItem.InventorySprite = item.InventorySprite;
        monoItem.Type = item.Type;
        monoItem.isJustSpawned = true;

        //Sprite renderer
        SpriteRenderer sprite = obj.AddComponent<SpriteRenderer>();
        sprite.sprite = item.InGameSprite;
        sprite.sortingLayerID = item.SortingLayer;

        //BonCollider
        BoxCollider2D collider = obj.AddComponent<BoxCollider2D>();
        collider.size = sprite.sprite.bounds.size;

        return obj;
    }
}
