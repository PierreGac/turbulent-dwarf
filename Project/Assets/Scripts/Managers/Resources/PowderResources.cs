using UnityEngine;
using System.Collections;

public class PowderResources : MonoBehaviour
{
    public Sprite INPentanitePowder;
    public static Sprite IN_PentanitePowder;
    public Sprite IGPentanitePowder;
    public static Sprite IG_PentanitePowder;

    void Awake()
    {
        IN_PentanitePowder = INPentanitePowder;
        IG_PentanitePowder = IGPentanitePowder;
    }
}
