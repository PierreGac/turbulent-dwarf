using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections;

public interface Item : ICloneable
{
    byte ItemValue { get; set; }
    ItemType Type { get; set; }
    GlobalType GType { get; set; }
    GameObject gameObject { get; set; }
    string Name { get; set; }
    string Description { get; set; }
    float Mass { get; set; }
    bool isUsable { get; set; }
    bool isStackable { get; set; }
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
    GlobalType[] AllowedContent { get; set; }
    Item[] Content { get; set; }
    void SetContent(params Item[] items);
}
/// <summary>
/// Enum for item categories
/// </summary>
public enum GlobalType { Money, Gems, Fruits, Vegetables, Wood, Stone, Container};
/// <summary>
/// Detailed enum about items
/// </summary>
public enum ItemType { Money, WhiteGem, YellowGem, RedGem, RockBoulder, Bag01, Orange, Banana, Apple, Coconut, Pear};

public class MonoItem : MonoBehaviour
{
    public ItemType Type;
    public Item thisItem;
    [HideInInspector]
    public bool isJustSpawned = false;

    public SpriteRenderer spriteRenderer;
    public Sprite InventorySprite;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
        GetItem();
    }

    public Item GetItem()
    {
        switch (Type)
        {
            case ItemType.Money:
                thisItem = new MoneyItem(gameObject);
                break;
            case ItemType.WhiteGem:
                thisItem = new Gems(gameObject, "White gem", "A very bright white gem", ItemValues.WhiteGem);
                break;
            case ItemType.YellowGem:
                thisItem = new Gems(gameObject, "Yellow gem", "A yellow gem", ItemValues.YellowGem);
                break;
            case ItemType.RedGem:
                thisItem = new Gems(gameObject, "Red gem", "A blood red gem", ItemValues.RedGem);
                break;
            case ItemType.RockBoulder:
                thisItem = new Boulders(gameObject);
                break;
            case ItemType.Bag01:
                thisItem = new Bag01(gameObject);
                break;
            #region Fruits
            case ItemType.Pear:
                thisItem = new Fruits(gameObject, 0.2f, "A tasty pear", ItemValues.Pear, FruitsStatistics.PearStats);
                break;
            case ItemType.Apple:
                thisItem = new Fruits(gameObject, 0.2f, "A shiny apple", ItemValues.Apple, FruitsStatistics.AppleStats);
                break;
            case ItemType.Coconut:
                thisItem = new Fruits(gameObject, 0.8f, "A heavy coconut", ItemValues.Coconut, FruitsStatistics.CoconutStats);
                break;
            case ItemType.Orange:
                thisItem = new Fruits(gameObject, 0.2f, "A juicy orange", ItemValues.Orange, FruitsStatistics.OrangeStats);
                break;
            case ItemType.Banana:
                thisItem = new Fruits(gameObject, 0.2f, "A sweaty banana", ItemValues.Banana, FruitsStatistics.BananaStats);
                break;
            #endregion
        }
        thisItem.Type = Type;
        thisItem.SortingLayer = GetComponent<SpriteRenderer>().sortingLayerID;
        thisItem.InventorySprite = InventorySprite;
        thisItem.InGameSprite = GetComponent<SpriteRenderer>().sprite;

        return thisItem;
    }

    public static GameObject CreateGameObjectFromItem(Item item)
    {
        GameObject obj = new GameObject(item.Name);
        //Sprite renderer
        SpriteRenderer sprite = obj.AddComponent<SpriteRenderer>();
        sprite.sprite = item.InGameSprite;
        sprite.sortingLayerID = item.SortingLayer;

        //BoxCollider
        BoxCollider2D collider = obj.AddComponent<BoxCollider2D>();
        collider.size = sprite.sprite.bounds.size;

        MonoItem monoItem = obj.AddComponent<MonoItem>();
        monoItem.thisItem = (Item)item.Clone();
        monoItem.thisItem.gameObject = obj;
        monoItem.InventorySprite = item.InventorySprite;
        monoItem.Type = item.Type;
        monoItem.isJustSpawned = true;
        monoItem.spriteRenderer.enabled = true;

        return obj;
    }
}
