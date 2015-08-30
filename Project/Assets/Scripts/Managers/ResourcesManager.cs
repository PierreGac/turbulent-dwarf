using UnityEngine;
using System.Collections;

public class ResourcesManager : MonoBehaviour
{
    public static ResourcesManager instance = null;

    public GameObject Player;
    public GameObject MainCamera;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
}
