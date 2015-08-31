using UnityEngine;
using System.Collections;

public class Loader : MonoBehaviour
{
    public GameObject gameManager;
    public GameObject tileManager;
    public GameObject hexTileManager;
    public GameObject sceneObject;
    public GameObject resourcesManager;
    void Awake()
    {
        if (ResourcesManager.instance == null)
            Instantiate(resourcesManager);
        if (HexTileManager.instance == null)
            Instantiate(hexTileManager);
        if (GameManager.instance == null)
            Instantiate(gameManager);
        if (TileManager.instance == null)
            Instantiate(tileManager);
        if (Scene.instance == null)
            Instantiate(sceneObject);

        //Load item statistics
        StatisticValues stats = new FruitsStatistics();
        stats.Init();

        Scene.SpawnScene();
    }
}
