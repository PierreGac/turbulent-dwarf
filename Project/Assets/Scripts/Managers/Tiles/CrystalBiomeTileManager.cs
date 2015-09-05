using UnityEngine;
using System.Collections;

public class CrystalBiomeTileManager : MonoBehaviour
{
    public GameObject[] _GroundTiles;
    public static GameObject[] GroundTiles;

    public GameObject[] _HugeCrystalTiles;
    public static GameObject[] HugeCrystalTiles;

    public GameObject[] _SimpleCrystalTiles;
    public static GameObject[] SimpleCrystalTiles;

    void Awake()
    {
        GroundTiles = _GroundTiles;
        HugeCrystalTiles = _HugeCrystalTiles;
        SimpleCrystalTiles = _SimpleCrystalTiles;
    }
}
