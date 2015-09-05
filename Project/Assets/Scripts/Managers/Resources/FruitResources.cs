using UnityEngine;
using System.Collections;

public class FruitResources : MonoBehaviour
{
    public Sprite INOrange;
    public static Sprite IN_Orange;
    public Sprite IGOrange;
    public static Sprite IG_Orange;

    public Sprite INApple;
    public static Sprite IN_Apple;
    public Sprite IGApple;
    public static Sprite IG_Apple;

    public Sprite INCoconut;
    public static Sprite IN_Coconut;
    public Sprite IGCoconut;
    public static Sprite IG_Coconut;

    public Sprite INBanana;
    public static Sprite IN_Banana;
    public Sprite IGBanana;
    public static Sprite IG_Banana;

    public Sprite INPear;
    public static Sprite IN_Pear;
    public Sprite IGPear;
    public static Sprite IG_Pear;

    public Sprite INGreenMushroom;
    public static Sprite IN_GreenMushroom;
    public Sprite IGGreenMushroom;
    public static Sprite IG_GreenMushroom;

    void Awake()
    {
        IN_Orange = INOrange;
        IG_Orange = IGOrange;

        IN_Apple = INApple;
        IG_Apple = IGApple;

        IN_Coconut = INCoconut;
        IG_Coconut = IGCoconut;

        IN_Banana = INBanana;
        IG_Banana = IGBanana;

        IN_Pear = INPear;
        IG_Pear = IGPear;

        IN_GreenMushroom = INGreenMushroom;
        IG_GreenMushroom = IGGreenMushroom;
    }
}
