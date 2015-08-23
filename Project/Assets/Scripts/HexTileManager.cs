using UnityEngine;
using System.Collections;

public class HexTileManager : MonoBehaviour
{
    public GameObject Hex;
    public GameObject[] WaterTiles;
    public GameObject[] WallTiles;
    public GameObject[] GroundTiles;
    public GameObject[] FloorTiles;
    public GameObject[] GrassTiles;
    public GameObject Fog;

    #region Items
    public GameObject[] Gems;
    #endregion

    public static HexTileManager instance = null;

    void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
}
