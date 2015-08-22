using UnityEngine;
using System.Collections;
using System;
using Random = System.Random;
using DungeonSpawner;


public class Scene : MonoBehaviour
{
    private Random _rnd;
    private static Scene instance = null;
    public static Grid[] _grid;
    public static int Width;
    public static int Height;
    public static int Size1D
    {
        get
        {
            return Width * Height;
        }
    }
    private CellularAutomata _automata;
    private DungeonSpawner.DungeonSpawner _dugneon;
    public Player PlayerScript;

    private int _playerX;
    private int _playerY;
    private int _playerSpawn;
	// Use this for initialization

    void Awake()
    {
        Width = 100;
        Height = 100;
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    public static void SpawnScene()
    {
        instance._rnd = new Random();
        instance._automata = new CellularAutomata(100, 100, instance._rnd);
        //Init maxspawn value
        //ItemManager.
            instance._automata.Player = instance.PlayerScript.transform;
        instance._automata.RandomFillMap();
        instance._automata.ProcessCavern(4);
        instance._automata.PrintMap();
        instance.SpawnItems();
    }


    private void SpawnItems()
    {
        _playerSpawn = -1;
        GameObject item;
        for (int i = 0; i < Size1D; i++)
        {
            if (_rnd.Next(0, 50) == 1 && _automata.Map[i].BasicValue != 1)
                _playerSpawn = i;
            if (_rnd.Next(0, 100) == 1 && _automata.Map[i].BasicValue != 1)
            {
                item = GameObject.Instantiate(TileManager.instance.Gems[_rnd.Next(0, TileManager.instance.Gems.Length)], _grid[i].TileObject.transform.position, _grid[i].TileObject.transform.rotation) as GameObject;
                item.transform.SetParent(_grid[i].TileObject.transform);
            }
        }

        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        PlayerScript.transform.position = new Vector3((_playerSpawn - (_playerSpawn / Scene.Width) * Scene.Width), _playerSpawn / Scene.Height, 0f);
        PlayerScript.transform.gameObject.SetActive(true);
        Fog.UpdateFog(_playerSpawn, TileFogState.Active);
        GameObject.Find("Camera").gameObject.SetActive(false);
    }
}
