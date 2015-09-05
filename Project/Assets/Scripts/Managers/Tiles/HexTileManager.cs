using UnityEngine;
using System.Collections;

public class HexTileManager : MonoBehaviour
{
    public GameObject Hex;
    public GameObject[] WaterTiles;
    public GameObject[] WallTiles;
    public GameObject[] LavaTiles;
    public GameObject[] GroundTiles;
    public GameObject[] FloorTiles;
    public GameObject[] GrassTiles;
    public GameObject Fog;


    public GameObject ExitDoor;
    public GameObject MiningBlock;


    #region Items
    public GameObject[] Gems;
    public GameObject[] RawGems;
    public GameObject[] MoneyTiles;
    public GameObject[] Boulders;
    public GameObject[] Containers;
    public GameObject[] Fruits;
    public GameObject[] Vegetables;
    public GameObject[] Rocks;
    public GameObject[] Blocs;
    #endregion

    public static HexTileManager instance = null;

    void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);
    }
}
