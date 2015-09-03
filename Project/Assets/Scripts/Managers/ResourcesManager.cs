using UnityEngine;
using System.Collections;

public class ResourcesManager : MonoBehaviour
{
    public static ResourcesManager instance = null;

    public GameObject Player;
    public GameObject MainCamera;
    public Sprite EmptySprite;

    public GameObject RecipeButton;

    #region Item sprites
    #region INGAME
    public Sprite IG_Bag01;
    public Sprite IG_WhiteGem;
    public Sprite IG_RedGem;
    public Sprite IG_YellowGem;
    public Sprite IG_MoneyBag;
    #endregion
    #region INVENTORY
    public Sprite IN_Bag01;
    public Sprite IN_WhiteGem;
    public Sprite IN_RedGem;
    public Sprite IN_YellowGem;
    public Sprite IN_MoneyBag;
    #endregion

    #endregion

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
}
