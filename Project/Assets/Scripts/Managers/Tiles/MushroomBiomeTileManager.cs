using UnityEngine;
using System.Collections;

public class MushroomBiomeTileManager : MonoBehaviour
{
    public GameObject[] _GroundTiles;
    public static GameObject[] GroundTiles;

    public GameObject[] _TreeTiles;
    public static GameObject[] TreeTiles;

    public GameObject[] _MushroomTiles;
    public static GameObject[] MushroomTiles;

    public GameObject[] _WaterTiles;
    public static GameObject[] WaterTiles;

    public GameObject[] _LogTiles;
    public static GameObject[] LogTiles;


    void Awake()
    {
        GroundTiles = _GroundTiles;
        TreeTiles = _TreeTiles;
        MushroomTiles = _MushroomTiles;
        WaterTiles = _WaterTiles;
        LogTiles = _LogTiles;
    }
}
