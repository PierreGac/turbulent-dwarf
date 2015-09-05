using UnityEngine;
using System.Collections;

public class CrystalResources : MonoBehaviour
{
    public Sprite INRawPentanite;
    public static Sprite IN_RawPentanite;
    public Sprite IGRawPentanite;
    public static Sprite IG_RawPentanite;

    void Awake()
    {
        IN_RawPentanite = INRawPentanite;
        IG_RawPentanite = IGRawPentanite;
    }
}
