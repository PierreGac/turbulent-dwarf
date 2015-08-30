using UnityEngine;
using System.Collections;

public class Inventory : MonoBehaviour
{
    private InventoryUI InventoryCaneva;
    public int InventorySpace = 32;
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
            UserInterface.MoneyText.text = string.Format("Money: {0}", value);
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
            InventoryCaneva.OpenInventory(Items);
            /*
            Debug.Log("Open inventory");
            string str = "Content: ";
            _totalMass = 0;
            foreach(Item item in Items)
            {
                if (item != null)
                {
                    str = string.Format("{0} {1}(x{2})", str, item.Name, item.Count);
                    _totalMass += item.Mass;
                }
            }
            Debug.Log(str + "  Total mass: " + _totalMass);
            */
        }
    }


    public static bool AddItemToInventory(Item item)
    {
        //Looking for the same item
        for(int i = 0; i < Items.Length; i++)
        {
            if (Items[i] != null)
            {
                if (Items[i].Name == item.Name)
                {
                    Items[i].Count++;
                    GlobalEventText.AddMessage(string.Format("You picked up \"{0}\" (x{1})", item.Name, item.Count));
                    return true;
                }
            }
        }

        //Trying to add the item to the inventory
        for (int i = 0; i < Items.Length; i++)
        {
            if (Items[i] == null)
            {
                Items[i] = item;
                GlobalEventText.AddMessage(string.Format("You picked up \"{0}\" (x{1})", item.Name, item.Count));
                return true;
            }
        }

        //Inventory full
        Debug.Log("Inventory full!");
        GlobalEventText.AddMessage(string.Format("You can't pickup \"{0}\", because your inventory is full", item.Name));
        return false;
    }
}
