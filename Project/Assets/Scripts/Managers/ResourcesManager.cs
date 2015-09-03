using UnityEngine;
using System.Collections;

public class ResourcesManager : MonoBehaviour
{
    public static ResourcesManager instance = null;

    public GameObject Player;
    public GameObject MainCamera;
    public Sprite EmptySprite;

    public Sprite InterrogationDotSprite;
    public GameObject RecipeButton;

    #region Item sprites
    #region INGAME
    public Sprite IG_Bag01;
    public Sprite IG_WhiteGem;
    public Sprite IG_RedGem;
    public Sprite IG_YellowGem;
    public Sprite IG_MoneyBag;
    public Sprite IG_Rock;
    public Sprite IG_Bloc;
    public Sprite IG_RawGem;
    public Sprite IG_Boulders;
    #endregion
    #region INVENTORY
    public Sprite IN_Bag01;
    public Sprite IN_WhiteGem;
    public Sprite IN_RedGem;
    public Sprite IN_YellowGem;
    public Sprite IN_MoneyBag;
    public Sprite IN_Rock;
    public Sprite IN_Bloc;
    public Sprite IN_RawGem;
    public Sprite IN_Boulders;
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
