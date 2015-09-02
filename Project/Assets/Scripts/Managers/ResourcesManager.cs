using UnityEngine;
using System.Collections;

public class ResourcesManager : MonoBehaviour
{
    public static ResourcesManager instance = null;

    public GameObject Player;
    public GameObject MainCamera;
    public Sprite EmptySprite;


    #region Item sprites
    #region INGAME
    public Sprite IG_Bag01;
    #endregion
    #region INVENTORY
    public Sprite IN_Bag01;
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
