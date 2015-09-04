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

public interface DestructibleTile
{
    AudioSource Audio { get; set; }

    float HealthPoints { get; set; }

    float DamageCoef { get; set; }


    bool OnDamage(float damagePoints, int index);
}
/// <summary>
/// Enum for item categories
/// </summary>
public enum GlobalType { Money, Gems, Fruits, Vegetables, Wood, Stone, Container, Rocks, Blocs, RawGems, Logs, Planks};
/// <summary>
/// Detailed enum about items
/// </summary>
public enum ItemType
{ 
    Money, 
    WhiteGem, 
    YellowGem, 
    RedGem, 
    RockBoulder, 
    Bag01,
    Orange,
    Banana,
    Apple, 
    Coconut,
    Pear,
    Mushroom01,
    RockGranit,
    RockGabbro,
    RockDiorite,
    RockBasalt,
    BlocGranit,
    BlocGabbro,
    BlocDiorite,
    BlocBasalt,
    RawGem,
    MushroomLog
};

public class MonoItem : MonoBehaviour
{
    public ItemType Type;
    public Item thisItem;
    [HideInInspector]
    public bool isJustSpawned = false;

    public SpriteRenderer spriteRenderer;

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
            case ItemType.RawGem:
                thisItem = new RawGems(gameObject);
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
            case ItemType.Mushroom01:
                thisItem = new Fruits(gameObject, 0.2f, "A green mushroom. Should I eat it?", ItemValues.Mushroom01, FruitsStatistics.Mushroom01Stats);
                break;
            #endregion
            #region Rocks
            case ItemType.RockBasalt:
                thisItem = new Rocks(gameObject, Rocks.BasaltName, ItemValues.RockBasalt);
                break;
            case ItemType.RockDiorite:
                thisItem = new Rocks(gameObject, Rocks.DioriteName, ItemValues.RockDiorite);
                break;
            case ItemType.RockGabbro:
                thisItem = new Rocks(gameObject, Rocks.GabbroName, ItemValues.RockGabbro);
                break;
            case ItemType.RockGranit:
                thisItem = new Rocks(gameObject, Rocks.GranitName, ItemValues.RockGranit);
                break;
            #endregion
            #region Blocs
            case ItemType.BlocBasalt:
                thisItem = new RockBlocks(gameObject, RockBlocks.BasaltName, ItemValues.BlocBasalt);
                break;
            case ItemType.BlocDiorite:
                thisItem = new RockBlocks(gameObject, RockBlocks.DioriteName, ItemValues.BlocDiorite);
                break;
            case ItemType.BlocGabbro:
                thisItem = new RockBlocks(gameObject, RockBlocks.GabbroName, ItemValues.BlocGabbro);
                break;
            case ItemType.BlocGranit:
                thisItem = new RockBlocks(gameObject, RockBlocks.GranitName, ItemValues.BlocGranit);
                break;
            #endregion
            #region Logs
            case ItemType.MushroomLog:
                thisItem = new MushroomLog(gameObject);
                break;
            #endregion
        }
        thisItem.Type = Type;
        thisItem.SortingLayer = GetComponent<SpriteRenderer>().sortingLayerID;

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
        monoItem.Type = item.Type;
        monoItem.isJustSpawned = true;
        monoItem.spriteRenderer.enabled = true;

        return obj;
    }
}
