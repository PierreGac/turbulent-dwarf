using UnityEngine;
using System.Collections;

public class Loader : MonoBehaviour
{
    public GameObject gameManager;
    public GameObject tileManager;
    public GameObject userInterface;
    void Awake()
    {
        if (GameManager.instance == null)
            Instantiate(gameManager);
        if (TileManager.instance == null)
            Instantiate(tileManager);
        if (UserInterface.instance == null)
            Instantiate(userInterface);
    }
}
