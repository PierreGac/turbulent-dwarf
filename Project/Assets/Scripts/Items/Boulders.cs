using UnityEngine;
using System;
using Random = UnityEngine.Random;
using System.Collections;

public class Boulders : Item
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
    public float Value { get; set; }
    public bool isUsable { get; set; }
    public int SortingLayer { get; set; }
    
    private ItemType[] _itemsInBoulder;
    public Boulders(GameObject gameObject)
    {
        this.Name = "Boulders";
        this.GType = GlobalType.Container;
        this.Mass = 10.0f;
        this.Description = "A pile of stone and gems. Prospect it to find some interesting stuff !";
        this.Count = 1;
        this.gameObject = gameObject;
        this._itemsInBoulder = new ItemType[] { ItemType.RedGem, ItemType.WhiteGem, ItemType.YellowGem };
        this.isUsable = true;
        this.ItemValue = ItemValues.Boulders;
    }

    public void PickupItem()
    {
        if(Inventory.AddItemToInventory(this))
            MonoBehaviour.Destroy(gameObject);
    }

    public void Use()
    {
        //Prospect action : Will provide items
        int amountOfItems = Random.Range(1, 10);
        ItemType type;
        GameObject tmp = null;
        Item thisItem = null;
        for(int i = 0; i < amountOfItems; i++)
        {
            //Get a random item type:
            type = _itemsInBoulder[Random.Range(0, _itemsInBoulder.Length)];
            //Creating the item
            switch(type)
            {
                case ItemType.RedGem:
                case ItemType.WhiteGem:
                case ItemType.YellowGem:
                    tmp = MonoBehaviour.Instantiate(HexTileManager.instance.Gems[Random.Range(0, HexTileManager.instance.Gems.Length)]) as GameObject;
                    thisItem = tmp.GetComponent<MonoItem>().GetItem();
                    thisItem.Count = Random.Range(1, 3);
                    break;
            }

            //Add the item to the inventory
            Inventory.AddItemToInventory(thisItem);
            MonoBehaviour.Destroy(tmp);
        }

        //The item has been used, so we have to decrease the total amount or destroy it from the inventory
        if (this.Count > 1)
            this.Count--;
        else
            Inventory.RemoveItemFromInventory(this);
    }
}