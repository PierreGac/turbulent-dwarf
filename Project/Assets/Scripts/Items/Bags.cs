using UnityEngine;
using System.Collections;

public class Bag01 : ItemContainer
{
    public byte ItemValue { get; set; }
    public ItemType Type { get; set; }
    public string Name { get; set; }
    public float Mass { get; set; }
    public Sprite InGameSprite { get; set; }
    public Sprite InventorySprite { get; set; }
    public int Count { get; set; }
    public GameObject gameObject { get; set; }
    public string Description { get; set; }
    public bool isUsable { get; set; }
    public float Value { get; set; }
    public int SortingLayer { get; set; }
    public Item[] Content { get; set; }
    public ItemType[] AllowedContent { get; set; }

    public Bag01(GameObject gameObject)
    {
        this.Name = "Bag01";
        this.Mass = 8.0f;
        this.Description = "A textile bag some stuff inside";
        this.Count = 1;
        this.gameObject = gameObject;
        this.isUsable = true;
        this.ItemValue = ItemValues.Bag01;
        AllowedContent = new ItemType[] { ItemType.Money, ItemType.RedGem, ItemType.WhiteGem, ItemType.YellowGem };
    }

    public void PickupItem()
    {
        //Set a random stuff inside the bag:
        Item[] items = new Item[Random.Range(10, 12)];
        ItemType type;
        GameObject tmp = null;
        for (int i = 0; i < items.Length; i++)
        {
            //Get a random item type:
            type = AllowedContent[Random.Range(0, AllowedContent.Length)];
            //Creating the item
            switch (type)
            {
                case ItemType.RedGem:
                case ItemType.WhiteGem:
                case ItemType.YellowGem:
                    tmp = MonoBehaviour.Instantiate(HexTileManager.instance.Gems[Random.Range(0, HexTileManager.instance.Gems.Length)]) as GameObject;
                    items[i] = tmp.GetComponent<MonoItem>().GetItem();
                    items[i].Count = Random.Range(1, 3);
                    break;
                case ItemType.Money:
                    tmp = MonoBehaviour.Instantiate(HexTileManager.instance.MoneyTiles[Random.Range(0, HexTileManager.instance.MoneyTiles.Length)]) as GameObject;
                    items[i] = tmp.GetComponent<MonoItem>().GetItem();
                    items[i].Value = Random.Range(20, 250);
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
}
