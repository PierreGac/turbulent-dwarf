using UnityEngine;
using System.Collections;

public class Craft : MonoBehaviour
{
    private RecipeItem[] _rTMP = { RecipeItem.EMPTY, RecipeItem.EMPTY, RecipeItem.EMPTY, RecipeItem.EMPTY, new RecipeItem("White gem"), RecipeItem.EMPTY, RecipeItem.EMPTY, RecipeItem.EMPTY, RecipeItem.EMPTY };





    public static Recipe[] Recipes;

    void Awake()
    {
        Recipes = new Recipe[10];
        Recipes[0] = new Recipe(_rTMP, "Temp", 100, new Item[]{new Gems(null, "Red gem", "A blood red gem", ItemValues.RedGem), null, null});
    }

    public static int CheckIfCorrectRecipe(RecipeItem[] recipe)
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
    public RecipeItem[] Items;

    public Item[] Results;

    public bool isKnown { get; set; }

    public Recipe(RecipeItem[] items, string name, float success, Item[] results)
    {
        Items = new RecipeItem[9];
        items.CopyTo(Items, 0);
        Results = new Item[3];
        results.CopyTo(Results, 0);
        SuccessChance = success;
        isKnown = false;
    }

    public override string ToString()
    {
        string str = Items[0].ToString();
        for(int i = 1; i < 9; i++)
        {
            str = string.Format("{0},{1}", str, Items[i].ToString());
        }
        return str;
    }

    public bool CompareRecipes(RecipeItem[] recipe)
    {
        for(int i = 0; i < 9; i++)
        {
            if (Items[i].Name == recipe[i].Name) //Check for the name
            {
                if (recipe[i].Count < Items[i].Count) //Check for the correct count
                    return false;
            }
            else
                return false;
        }
        return true;
    }
}

public class RecipeItem
{
    public static RecipeItem EMPTY = new RecipeItem("EMPTY", 0);
    public string Name { get; set; }
    public int Count { get; set; }

    public RecipeItem(string name, int count = 1)
    {
        this.Name = name;
        this.Count = count;
    }

    public override string ToString()
    {
        if (Name == "EMPTY")
            return "EMPTY";
        else
            return string.Format("{0}({1})", Name, Count);
    }
}