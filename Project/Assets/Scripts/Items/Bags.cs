using UnityEngine;
using System.Collections;

public class Bag01 : ItemContainer
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
    public Item[] Content { get; set; }
    public GlobalType[] AllowedContent { get; set; }

    public Bag01(GameObject gameObject)
    {
        this.Name = "Furniture bag";
        this.GType = GlobalType.Container;
        this.Mass = 8.0f;
        this.Description = "A textile bag with some stuff inside";
        this.Count = 1;
        this.gameObject = gameObject;
        this.isUsable = true;
        this.ItemValue = ItemValues.Bag01;
        this.isStackable = false;
        AllowedContent = new GlobalType[] { GlobalType.Gems, GlobalType.Money, GlobalType.Fruits };
    }

    public void PickupItem()
    {
        //Set a random stuff inside the bag:
        Item[] items = new Item[Random.Range(2, 12)];
        GlobalType type;
        GameObject tmp = null;
        for (int i = 0; i < items.Length; i++)
        {
            //Get a random item type:
            type = AllowedContent[Random.Range(0, AllowedContent.Length)];
            //Creating the item
            switch (type)
            {
                case GlobalType.Gems:
                    tmp = MonoBehaviour.Instantiate(HexTileManager.instance.Gems[Random.Range(0, HexTileManager.instance.Gems.Length)]) as GameObject;
                    items[i] = tmp.GetComponent<MonoItem>().GetItem();
                    items[i].Count = Random.Range(1, 3);
                    break;
                case GlobalType.Money:
                    tmp = MonoBehaviour.Instantiate(HexTileManager.instance.MoneyTiles[Random.Range(0, HexTileManager.instance.MoneyTiles.Length)]) as GameObject;
                    items[i] = tmp.GetComponent<MonoItem>().GetItem();
                    items[i].Value = Random.Range(20, 250);
                    break;
                case GlobalType.Fruits:
                    tmp = MonoBehaviour.Instantiate(HexTileManager.instance.Fruits[Random.Range(0, HexTileManager.instance.Fruits.Length)]) as GameObject;
                    items[i] = tmp.GetComponent<MonoItem>().GetItem();
                    items[i].Count = Random.Range(1, 5);
                    break;
            }
            MonoBehaviour.Destroy(tmp);
        }
        SetContent(items);


        if (Inventory.AddItemToInventory(this))
            MonoBehaviour.Destroy(gameObject);
    }
    public void Use()
    { 
        //Open and get the stuff inside the bag
        PanelContainerUI.OpenPanelContainerUI(this);
    }

    public void SetContent(params Item[] items)
    {
        Content = new Item[items.Length];
        this.Mass = 0;
        for(int i = 0; i < items.Length; i++)
        {
            Content[i] = items[i];
            this.Mass += items[i].Mass;
        }
    }

    public object Clone()
    {
        Bag01 clonedItem = new Bag01(null);

        //Set the new content items:
        clonedItem.Content = new Item[Content.Length];
        for (int i = 0; i < Content.Length; i++ )
        {
            clonedItem.Content[i] = (Item)Content[i].Clone();
        }
        clonedItem.Mass = Mass;

        clonedItem.Type = Type;
        clonedItem.SortingLayer = SortingLayer;
        clonedItem.InventorySprite = InventorySprite;
        clonedItem.InGameSprite = InGameSprite;
        clonedItem.Count = Count;
        return clonedItem;
    }
}
