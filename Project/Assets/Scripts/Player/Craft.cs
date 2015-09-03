using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Craft : MonoBehaviour
{
    public static Recipe[] Recipes;

    void Awake()
    {
        CreateRecipes();
    }

    private void CreateRecipes()
    {
        Recipes = new Recipe[10];
        Item[] items = new Item[9];
        Item[] _rTMP = { null, null, null, null, new Gems(null, "White gem", "A white gem", ItemValues.WhiteGem), null, null, null, null };
        Recipes[0] = new Recipe(_rTMP, "NAAAME", 100, new Item[] { new Gems(null, "Red gem", "A blood red gem", ItemValues.RedGem), null, null });
        Recipes[0].isKnown = true;
        Recipes[0].InventorySprite = _rTMP[4].InventorySprite;

        Item[] _rTMP2 = { null, null, null, null, new Gems(null, "Red gem", "a blood red gem", ItemValues.RedGem), null, null, null, null };
        Recipes[1] = new Recipe(_rTMP2, "BWAA", 100, new Item[] { new Gems(null, "White gem", "A white gem", ItemValues.WhiteGem), null, null });
        Recipes[1].isKnown = true;
        Recipes[1].InventorySprite = _rTMP2[4].InventorySprite;

        Item[] _rTMP3 = { null, null, null, null, new Gems(null, "Yellow gem", "A yellow gem", ItemValues.YellowGem), null, null, null, null };
        Recipes[2] = new Recipe(_rTMP3, "TOTO", 100, new Item[] { new Gems(null, "White gem", "A white gem", ItemValues.WhiteGem), null, null });
        Recipes[2].InventorySprite = _rTMP3[4].InventorySprite;

        #region Rocks
        for (int i = 0; i < 9; i++)
        {
            items[i] = new Rocks(null, Rocks.BasaltName, ItemValues.RockBasalt);
        }
        Recipes[3] = new Recipe(items, RockBlocks.BasaltName, 100, new Item[] { null, new RockBlocks(null, RockBlocks.BasaltName, ItemValues.BlocBasalt), null });
        Recipes[3].InventorySprite = ResourcesManager.instance.IN_Bloc;

        //---------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------

        for (int i = 0; i < 9; i++)
        {
            items[i] = new Rocks(null, Rocks.DioriteName, ItemValues.RockDiorite);
        }
        Recipes[4] = new Recipe(items, RockBlocks.DioriteName, 100, new Item[] { null, new RockBlocks(null, RockBlocks.DioriteName, ItemValues.BlocDiorite), null });
        Recipes[4].InventorySprite = ResourcesManager.instance.IN_Bloc;

        //---------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------

        for (int i = 0; i < 9; i++)
        {
            items[i] = new Rocks(null, Rocks.GabbroName, ItemValues.RockGabbro);
        }
        Recipes[4] = new Recipe(items, RockBlocks.GabbroName, 100, new Item[] { null, new RockBlocks(null, RockBlocks.GabbroName, ItemValues.BlocGabbro), null });
        Recipes[4].InventorySprite = ResourcesManager.instance.IN_Bloc;

        //---------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------

        for (int i = 0; i < 9; i++)
        {
            items[i] = new Rocks(null, Rocks.GranitName, ItemValues.RockGranit);
        }
        Recipes[5] = new Recipe(items, RockBlocks.GranitName, 100, new Item[] { null, new RockBlocks(null, RockBlocks.GranitName, ItemValues.BlocGranit), null });
        Recipes[5].InventorySprite = ResourcesManager.instance.IN_Bloc;
        #endregion

        for (int i = 0; i < 9; i++)
        {
            if (i == 4)
                items[i] = new RawGems(null);
            else
                items[i] = null;
        }
        Recipes[6] = new Recipe(items, "Raw gem", 50, null, true, new Item[] { new Gems(null, "Red gem", "A blood red gem", ItemValues.RedGem), new Gems(null, "White gem", "A shiny white gem", ItemValues.WhiteGem), new Gems(null, "Yellow gem", "A yellow gem", ItemValues.YellowGem)}, 1);
        Recipes[6].InventorySprite = ResourcesManager.instance.IN_RawGem;
    }

    public static int CheckIfCorrectRecipe(Item[] recipe)
    {
        for(int i = 0; i < Recipes.Length; i++)
        {
            if (Recipes[i] != null)
            {
                if (Recipes[i].CompareRecipes(recipe)) //Check if the recipe is correct
                    return i;
            }
        }
        return -1;
    }

    /*public static RecipeItem[] GetCraftRest(RecipeItem[] recipe, int recipeID)
    {
        RecipeItem[] rest = new RecipeItem[9];
        int count;
        for (int i = 0; i < 9; i++)
        {
            if (recipe[i].Name == "EMPTY")
                rest[i] = RecipeItem.EMPTY;
            count = Recipes[recipeID].Items[i].Count - recipe[i].Count;
            if (count == 0)
                rest[i] = RecipeItem.DESTROY;
            else
                rest[i] = new RecipeItem(recipe[i].Name, count);
        }
        return rest;
    }*/
}



public class Recipe
{
    public float SuccessChance { get; set; }
    public string Name { get; set; }
    public Sprite InventorySprite { get; set; }

    public Item[] Items;

    public Item[] Results;

    public bool isRandomResult { get; set; }

    public Item[] RandomResults { get; set; }

    public int AmountOfRandomItemInResult { get; set; }
    public bool isKnown { get; set; }

    public Recipe(Item[] items, string name, float success, Item[] results, bool random = false, Item[] randomResults = null, int amountOfRandomItems = 1)
    {
        Items = new Item[9];
        items.CopyTo(Items, 0);

        Results = new Item[3];
        if (results != null)
        {
            results.CopyTo(Results, 0);
        }
        SuccessChance = success;
        Name = name;
        isKnown = false;
        isRandomResult = random;
        if (amountOfRandomItems > 3)
            AmountOfRandomItemInResult = 3;
        else
            AmountOfRandomItemInResult = amountOfRandomItems;
        if(randomResults != null)
        {
            RandomResults = new Item[randomResults.Length];
            randomResults.CopyTo(RandomResults, 0);
        }
    }

    public bool CompareRecipes(Item[] recipe)
    {
        for(int i = 0; i < 9; i++)
        {
            if (recipe[i] != null)
            {
                if (Items[i].Name == recipe[i].Name) //Check for the name
                {
                    if (recipe[i].Count < Items[i].Count) //Check for the correct count
                        return false;
                }
                else
                    return false;
            }
        }
        return true;
    }

    public static GameObject CreateRecipeInventoryButton(int index, int recipeID)
    {
        GameObject obj = new GameObject();

        if (index < 10)
            obj.name = string.Format("0{0}", index);
        else
            obj.name = index.ToString();
        //Set the image component:
        Image img = obj.AddComponent<Image>();
        img.type = Image.Type.Sliced;
        img.sprite = ResourcesManager.instance.EmptySprite;
        img.color = new Color32(0, 0, 0, 255);
        //Add events:
        EventTrigger eventTrigger = obj.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((eventData) => { CraftUI.OnRecipePointerEnter(recipeID); });
        eventTrigger.triggers.Add(entry);

        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerExit;
        entry.callback.AddListener((eventData) => { CraftUI.OnRecipePointerExit(); });
        eventTrigger.triggers.Add(entry);

        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((eventData) => { CraftUI.OnRecipeClick(recipeID); });
        eventTrigger.triggers.Add(entry);

        //Create first child:
        GameObject child1 = new GameObject("inventorySlot");
        child1.transform.SetParent(obj.transform);
        img = child1.AddComponent<Image>();
        child1.GetComponent<RectTransform>().SetSize(new Vector2(35, 35));
        img.type = Image.Type.Sliced;
        img.sprite = Craft.Recipes[recipeID].InventorySprite;
        img.color = new Color32(255, 255, 255, 255);

        return obj;
    }
}

public class RecipeItem
{
    public static RecipeItem EMPTY = new RecipeItem("EMPTY", null,  0);
    public string Name { get; set; }
    public int Count { get; set; }
    public Sprite InventorySprite { get; set; }

    public RecipeItem(string name, Sprite inventorySprite, int count = 1)
    {
        this.Name = name;
        this.Count = count;
        this.InventorySprite = inventorySprite;
    }

    public override string ToString()
    {
        if (Name == "EMPTY")
            return "EMPTY";
        else
            return string.Format("{0}({1})", Name, Count);
    }
}