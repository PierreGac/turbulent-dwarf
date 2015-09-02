using UnityEngine;
using System.Collections;

public class Inventory : MonoBehaviour
{
    private static InventoryUI InventoryCaneva;
    public int InventorySpace = 40;
    public static Item[] Items;
    private static float _money;
    public static float Money
    {
        get
        {
            return _money;
        }
        set
        {
            _money = value;
        }
    }


    private float _totalMass = 0;

    void Awake()
    {
        InventoryCaneva = GameObject.FindGameObjectWithTag("Inventory").GetComponent<InventoryUI>();
        _money = 0;
        Items = new Item[InventorySpace];
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            if (!InventoryCaneva.panel.gameObject.activeInHierarchy)
                InventoryCaneva.OpenInventory(Items);
            else
                InventoryCaneva.Exit();
        }
        if(Input.GetKeyDown(KeyCode.C))
        {
            CraftUI.OpenCraftingUI();
        }
    }

    public static void ReorderInventory()
    {
        Item[] tmp = new Item[Items.Length];
        int index = 0;
        for(int i = 0; i < Items.Length; i++)
        {
            if(Items[i] != null)
            {
                tmp[index] = Items[i];
                index++;
            }
        }

        tmp.CopyTo(Items, 0);
    }

    public static bool AddItemToInventory(Item item)
    {
        //Looking for the same item
        if (item.isStackable) //If the item is stackable
        {
            for (int i = 0; i < Items.Length; i++)
            {
                if (Items[i] != null)
                {
                    if (Items[i].Name == item.Name)
                    {
                        Items[i].Count += item.Count;
                        GlobalEventText.AddMessage(string.Format("You picked up \"{0}\" (x{1})", item.Name, item.Count));
                        if (InventoryCaneva.panel.gameObject.activeInHierarchy)
                            InventoryCaneva.RefreshUI();
                        return true;
                    }
                }
            }
        }

        //Trying to add the item to the inventory
        for (int i = 0; i < Items.Length; i++)
        {
            if (Items[i] == null)
            {
                Items[i] = item;
                Debug.Log("cccc: " + Items[i].Count);
                GlobalEventText.AddMessage(string.Format("You picked up \"{0}\" (x{1})", item.Name, item.Count));
                if (InventoryCaneva.panel.gameObject.activeInHierarchy)
                    InventoryCaneva.RefreshUI();
                return true;
            }
        }

        //Inventory full
        Debug.Log("Inventory full!");
        GlobalEventText.AddMessage(string.Format("You can't pickup \"{0}\", because your inventory is full", item.Name));
        return false;
    }

    public static void RefreshUI()
    {
        InventoryCaneva.RefreshUI();
    }

    /// <summary>
    /// Removes an item from the inventory
    /// </summary>
    /// <param name="item"></param>
    public static void RemoveItemFromInventory(Item item)
    {
        for(int i = 0; i < Items.Length; i++)
        {
            if(item == Items[i])
            {
                Items[i] = null;
                if (InventoryCaneva.panel.gameObject.activeInHierarchy)
                    InventoryCaneva.RefreshUI(true);
                return;
            }
        }
    }

    public static void SwitchItems(GameObject obj1, GameObject obj2)
    {
        int index1 = int.Parse(obj1.transform.parent.name);
        int index2 = int.Parse(obj2.transform.parent.name);
        Debug.Log(string.Format("Switching items: {0} => {1}", Items[index1], Items[index2]));
        Item tmp = Items[index1];
        Items[index1] = Items[index2];
        Items[index2] = tmp;
        Debug.Log(string.Format("===>>>Switching items: {0} => {1}", Items[index1], Items[index2]));
        RefreshUI();
        
    }

    public static void DecreaseCount(Item item, int value)
    {
        for (int i = 0; i < Items.Length; i++)
        {
            if (item == Items[i])
            {
                Debug.Log("count: " + Items[i].Count + "  " + item.Count);
                Items[i].Count -= value;
                //Make sure that the item is not with a negative count
                if (Items[i].Count <= 0)
                {
                    Debug.Log("Remove from inventory");
                    RemoveItemFromInventory(item);
                }
                else
                    RefreshUI();
                return;
            }
        }
    }

    public static void DecreaseCount(string itemName, int value)
    {
        for (int i = 0; i < Items.Length; i++)
        {
            if (Items[i] != null)
            {
                if (itemName == Items[i].Name)
                {
                    Items[i].Count -= value;
                    //Make sure that the item is not with a negative count
                    if (Items[i].Count <= 0)
                    {
                        value -= Items[i].Count;
                        RemoveItemFromInventory(Items[i]);
                        if (value == 0)
                            return;
                    }
                    else
                        return;
                }
            }
        }
    }
}
