using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CraftUI : MonoBehaviour
{
    public GameObject[] Buttons;
    private static Image[] _images;

    public GameObject[] ResultButtons;
    private static Image[] _resultImages;

    public static Item[] ItemsInCraftTable;
    public static Item[] ItemsInResult;

    public Sprite EmptySprite;
    private static Sprite _emptySprite;

    public GameObject panel;
    private static GameObject _panel;



    void Awake()
    {
        _panel = panel;
        _emptySprite = EmptySprite;
        _images = new Image[Buttons.Length];
        for(int i = 0; i < _images.Length; i++)
        {
            _images[i] = Buttons[i].transform.GetChild(0).GetComponent<Image>();
            _images[i].gameObject.SetActive(false);
        }

        _resultImages = new Image[ResultButtons.Length];
        for (int i = 0; i < 3; i++)
        {
            _resultImages[i] = ResultButtons[i].transform.GetChild(0).GetComponent<Image>();
            _resultImages[i].gameObject.SetActive(false);
        }


        ItemsInCraftTable = new Item[9];
        ItemsInResult = new Item[3];
    }

    public void Exit()
    {
        panel.SetActive(false);
    }

    public static void OpenCraftingUI()
    {
        _panel.SetActive(true);

        //Reset the sprites of the crafting table:
        ResetUI();
    }

    public void CraftTableBTN()
    {
        #region Creating the crafting string
        RecipeItem[] craftingTable = new RecipeItem[9];
        for(int i = 0; i < 9; i++)
        {
            if (ItemsInCraftTable[i] != null)
            {
                craftingTable[i] = new RecipeItem(ItemsInCraftTable[i].Name, ItemsInCraftTable[i].Count);
            }
            //Debug.Log(string.Format("Item {0} : {1} (x{2})", i, ItemsInCraftTable[i].Name, ItemsInCraftTable[i].Count));
            else
            {
                craftingTable[i] = RecipeItem.EMPTY;
                //Debug.Log(string.Format("Item {0} : {1}", i, ItemsInCraftTable[i]));
            }
        }
        #endregion

        #region Find the recipe
        int recipeIndex = Craft.CheckIfCorrectRecipe(craftingTable);
        if (recipeIndex == -1)
        {
            RemoveItemFromInventory();
            Debug.Log("Bad recipe !");
            return;
        }

        Debug.Log(Craft.Recipes[recipeIndex].ToString());
        

        //Get the results (put the results in the result panel)
        for(int i = 0; i < 3; i++)
        {
            if (Craft.Recipes[recipeIndex].Results[i] != null)
            {
                ItemsInResult[i] = (Item)Craft.Recipes[recipeIndex].Results[i].Clone();

                //Update result sprite
                _resultImages[i].gameObject.SetActive(true);
                _resultImages[i].sprite = ItemsInResult[i].InventorySprite;
            }
            else
            {
                ItemsInResult[i] = null;

                //Update result sprite
                _resultImages[i].gameObject.SetActive(false);
                _resultImages[i].sprite = ResourcesManager.instance.EmptySprite;
            }
        }

        //Handle craft items:
        for(int i = 0; i < 9; i++)
        {
            if (Craft.Recipes[recipeIndex].Items[i].Name != "EMPTY") //Check if not empty
                Inventory.DecreaseCount(craftingTable[i].Name, Craft.Recipes[recipeIndex].Items[i].Count); //Remove the recipe count of the item
        }
        #endregion

        //Update craftUI (keep the rest in the ui)
        RefreshUI();

        //Update the inventory
        Inventory.RefreshUI();
    }

    public void OnRemoveItem(int index)
    {
        if (ItemsInCraftTable[index] == null)
            return;
        if (Input.GetMouseButton(1))
        {
            Debug.Log("OnDelete");
            if (Input.GetKey(KeyCode.LeftShift))
                DecreaseCount(ItemsInCraftTable[index], 1);
            else
                RemoveItemFromInventory(ItemsInCraftTable[index]);
        }
    }

    private void RemoveItemFromInventory()
    {
        //Destroy the items
        for(int i = 0; i < 9; i++)
        {
            if (ItemsInCraftTable[i] != null)
            {
                Inventory.DecreaseCount(ItemsInCraftTable[i].Name, ItemsInCraftTable[i].Count);
                ItemsInCraftTable[i] = null;
            }
        }
        RefreshUI();
        Inventory.RefreshUI();
    }

    public static void RefreshUI()
    {
        for (int i = 0; i < _images.Length; i++)
        {
            if (ItemsInCraftTable[i] != null)
            {
                _images[i].gameObject.SetActive(true);
                _images[i].GetComponent<DragHandeler>().UpdateCountText(ItemsInCraftTable[i].Count);
                _images[i].sprite = ItemsInCraftTable[i].InventorySprite;
            }
            else
                _images[i].gameObject.SetActive(false);
        }
    }

    public static void ResetUI()
    {
        for (int i = 0; i < _images.Length; i++)
        {
            _images[i].gameObject.SetActive(false);
            _images[i].sprite = _emptySprite;
            ItemsInCraftTable[i] = null;
        }
    }

    public static bool CheckIfItemAlreadyPresent(Item item)
    {
        for(int i = 0; i < 9; i++)
        {
            if (ItemsInCraftTable[i] != null)
            {
                if (ItemsInCraftTable[i].Name == item.Name)
                    return true;
            }
        }
        return false;
    }

    public static void RemoveItemFromInventory(Item item)
    {
        for (int i = 0; i < 9; i++)
        {
            if (ItemsInCraftTable[i] == item)
            {
                ItemsInCraftTable[i] = null;
                RefreshUI();
                return;
            }
        }
    }

    public static void DecreaseCount(Item item, int value)
    {
        for (int i = 0; i < ItemsInCraftTable.Length; i++)
        {
            if (item == ItemsInCraftTable[i])
            {
                ItemsInCraftTable[i].Count -= value;
                //Make sure that the item is not with a negative count
                if (ItemsInCraftTable[i].Count <= 0)
                    RemoveItemFromInventory(item);
                else
                    RefreshUI();
                return;
            }
        }
    }

    /// <summary>
    /// Returns the total count of the desired item
    /// </summary>
    /// <param name="name">Name of the desired item</param>
    /// <returns>Total quantity of the item in the crafting table</returns>
    public static int GetCountOfItem(string name)
    {
        int count = 0;
        for (int i = 0; i < 9; i++)
        {
            if (ItemsInCraftTable[i] != null)
            {
                if (ItemsInCraftTable[i].Name == name)
                    count += ItemsInCraftTable[i].Count;
            }
        }
        return count;
    }
}
