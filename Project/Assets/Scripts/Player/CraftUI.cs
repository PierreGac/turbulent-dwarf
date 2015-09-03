using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

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

    public GameObject RecipeButtonsObject;
    private static GameObject _recipeButtonsObject;
    public static bool isActive
    {
        get
        {
            return _panel.activeInHierarchy;
        }
    }

    private static Recipe _previousRecipe;


    void Awake()
    {
        _panel = panel;
        _emptySprite = EmptySprite;
        _recipeButtonsObject = RecipeButtonsObject;
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
        //Add result items to the inventory
        for (int i = 0; i < 3; i++)
        {
            if (ItemsInResult[i] != null)
            {
                Inventory.AddItemToInventory(ItemsInResult[i]);
            }
        }
        Inventory.RefreshUI();
        _panel.SetActive(false);
    }
    public static void StaticExit()
    {
        //Add result items to the inventory
        for (int i = 0; i < 3; i++)
        {
            if (ItemsInResult[i] != null)
            {
                Inventory.AddItemToInventory(ItemsInResult[i]);
            }
        }
        Inventory.RefreshUI();
        _panel.SetActive(false);
    }

    public static void OpenCraftingUI()
    {
        _panel.SetActive(true);

        //Reset the sprites of the crafting table:
        ResetUI();
        RefreshRecipesList();
    }

    public void CraftTableBTN()
    {
        /*#region Creating the crafting string
        RecipeItem[] craftingTable = new RecipeItem[9];
        for(int i = 0; i < 9; i++)
        {
            if (ItemsInCraftTable[i] != null)
            {
                craftingTable[i] = new RecipeItem(ItemsInCraftTable[i].Name, ItemsInCraftTable[i].InventorySprite, ItemsInCraftTable[i].Count);
            }
            //Debug.Log(string.Format("Item {0} : {1} (x{2})", i, ItemsInCraftTable[i].Name, ItemsInCraftTable[i].Count));
            else
            {
                craftingTable[i] = RecipeItem.EMPTY;
                //Debug.Log(string.Format("Item {0} : {1}", i, ItemsInCraftTable[i]));
            }
        }
        #endregion
        */

        #region Find the recipe
        int recipeIndex = Craft.CheckIfCorrectRecipe(ItemsInCraftTable);
        if (recipeIndex == -1)
        {
            RemoveItemFromInventory();
            GlobalEventText.AddMessage("Bad recipe!");
            return;
        }
        if (!Craft.Recipes[recipeIndex].isKnown)
        {
            Craft.Recipes[recipeIndex].isKnown = true; //Set the selected recipe as "known"
            GlobalEventText.AddMessage(string.Format("Yay! You learned how to make {0}", Craft.Recipes[recipeIndex].Name));
        }
        //Get the results (put the results in the result panel)
        for(int i = 0; i < 3; i++)
        {
            if (Craft.Recipes[recipeIndex].Results[i] != null)
            {
                //Compare the actual recipe to the old one:
                if (_previousRecipe != null && Craft.Recipes[recipeIndex].Name == _previousRecipe.Name)
                {
                    if(ItemsInResult[i] != null)
                    {
                        ItemsInResult[i].Count += Craft.Recipes[recipeIndex].Results[i].Count;
                    }
                }
                else
                {
                    //Add the previous item to the inventory
                    if(ItemsInResult[i] != null)
                    {
                        Inventory.AddItemToInventory(ItemsInResult[i]);
                    }

                    ItemsInResult[i] = (Item)Craft.Recipes[recipeIndex].Results[i].Clone();

                    //Update result sprite
                    _resultImages[i].gameObject.SetActive(true);
                    _resultImages[i].sprite = ItemsInResult[i].InventorySprite;
                }
            }
            else
            {
                ItemsInResult[i] = null;

                //Update result sprite
                _resultImages[i].gameObject.SetActive(false);
                _resultImages[i].sprite = ResourcesManager.instance.EmptySprite;
            }
        }

        _previousRecipe = Craft.Recipes[recipeIndex];

        //Handle craft items:
        for(int i = 0; i < 9; i++)
        {
            if (Craft.Recipes[recipeIndex].Items[i] != null) //Check if not empty
            {
                Inventory.DecreaseCount(ItemsInCraftTable[i].Name, Craft.Recipes[recipeIndex].Items[i].Count); //Remove the recipe count of the item
                DecreaseCount(ItemsInCraftTable[i], Craft.Recipes[recipeIndex].Items[i].Count);
            }
        }
        #endregion

        //Update craftUI (keep the rest in the ui)
        RefreshUI();
        RefreshResults();
        RefreshRecipesList();
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

    public void GetResults(int index)
    {
        if (ItemsInResult[index] == null)
            return;
        Inventory.AddItemToInventory(ItemsInResult[index]);
        ItemsInResult[index] = null;
        _previousRecipe = null;
        RefreshResults();
        Inventory.RefreshUI();
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

    public static void RefreshResults()
    {
        for (int i = 0; i < _resultImages.Length; i++)
        {
            if (ItemsInResult[i] != null)
            {
                _resultImages[i].gameObject.SetActive(true);
                _resultImages[i].transform.GetChild(0).GetComponent<Text>().text = ItemsInResult[i].Count.ToString();
                _resultImages[i].sprite = ItemsInResult[i].InventorySprite;
            }
            else
                _resultImages[i].gameObject.SetActive(false);
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
        for (int i = 0; i < _resultImages.Length; i++)
        {
            _resultImages[i].gameObject.SetActive(false);
            _resultImages[i].sprite = _emptySprite;
            ItemsInResult[i] = null;
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


    #region Recipes list
    private static List<GameObject> _recipeButtons;
    private static Image[] _recipeImages;
    private static Recipe[] _knownRecipes;
    public static void RefreshRecipesList()
    {
        if (_recipeButtons != null)
        {
            //1- Delete existing buttons
            for (int i = 0; i < _recipeButtons.Count; i++)
            {
                Destroy(_recipeButtons[i]);
            }
        }
        _recipeButtons = new List<GameObject>();


        int ID = 0;
        List<Recipe> tmpList = new List<Recipe>();
        //Looping through the recipes list
        for(int i = 0; i < Craft.Recipes.Length; i++)
        {
            if(Craft.Recipes[i] != null)
            {
                //If the recipe is known => display it ! YAY !
                if(Craft.Recipes[i].isKnown)
                {
                    tmpList.Add(Craft.Recipes[i]);
                    //Create a new button:
                    GameObject btn = Recipe.CreateRecipeInventoryButton(ID, i);
                    btn.transform.SetParent(_recipeButtonsObject.transform);
                    _recipeButtons.Add(btn);
                    ID++;
                }
            }
        }
        _knownRecipes = new Recipe[tmpList.Count];
        tmpList.CopyTo(_knownRecipes);
    }

    public static void OnRecipePointerEnter(int index)
    {
        if(_knownRecipes[index] != null)
        {
            InventoryUI.OnHooveringRecipe = true;
            RecipePopupInfos.Show(index);
            //Display the recipe info panel
        }
    }

    public static void OnRecipePointerExit()
    {
        InventoryUI.OnHooveringRecipe = false;
        RecipePopupInfos.Hide();
    }

    public static void OnRecipeClick(int index)
    {
        ResetUI();
        bool isOK = true;
        bool[] tmp = { false, false, false, false, false, false, false, false, false };

        int count = 0;
        //1- Check if the inventory have enough items:
        for (int i = 0; i < 9; i++ )
        {
            if (!isOK)
            {
                GlobalEventText.AddMessage("You can't craft this recipe!");
                break;
            }


            if (Craft.Recipes[index].Items[i] != null)
            {
                count = 0;
                for (int j = 0; j < Inventory.Items.Length; j++)
                {
                    if (Inventory.Items[j] != null)
                    {
                        if (Inventory.Items[j].Name == Craft.Recipes[index].Items[i].Name)
                        {
                            count += Inventory.Items[j].Count;
                        }
                    }
                }
                //Check the count:
                if (count < Craft.Recipes[index].Items[i].Count)
                {
                    isOK = false;
                }
                else
                    tmp[i] = true;
            }
            else
                tmp[i] = true;
        }
        if (!isOK)
            return;
        foreach(bool b in tmp)
        {
            if(!b)
            {
                GlobalEventText.AddMessage("You can't craft this item!");
                return;
            }
        }
        //Put the ingredients in the list
        for (int i = 0; i < 9; i++)
        {
            if (Craft.Recipes[index].Items[i] != null)
            {
                _images[i].gameObject.SetActive(true);
                _images[i].sprite = Craft.Recipes[index].Items[i].InventorySprite;
                _images[i].transform.GetChild(0).GetComponent<Text>().text = Craft.Recipes[index].Items[i].Count.ToString();
                ItemsInCraftTable[i] = (Item)Craft.Recipes[index].Items[i].Clone();
            }
        }
    }

    #endregion
}
