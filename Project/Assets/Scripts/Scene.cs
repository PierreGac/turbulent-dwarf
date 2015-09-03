using UnityEngine;
using System.Collections;
using System;
using Random = System.Random;
using DungeonSpawner;
using System.Collections.Generic;


public class Scene : MonoBehaviour
{
    private Random _rnd;
    public static Scene instance = null;
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
    public ICellularAutomata _automata;
    private Player _playerScript;

    private int _playerX;
    private int _playerY;
    private Vector3 _playerSpawn;
    private int _playerIndex;
	// Use this for initialization

    void Awake()
    {
        Width = 100;
        Height = 100;
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    /// <summary>
    /// Main function, used for playe spawning
    /// </summary>
    public static void SpawnScene()
    {
        instance._rnd = new Random();
        instance._automata = new HexCellularAutomata(100, 100, instance._rnd);
        instance._automata.RandomFillMap();
        instance._automata.ProcessCavern(3);
        instance._automata.PrintMap();

        /*instance._automata = new CellularAutomata(100, 100, instance._rnd);



        instance._automata.Player = instance.PlayerScript.transform;
        instance._automata.RandomFillMap();
        instance._automata.ProcessCavern(4);
        instance._automata.PrintMap();*/
        instance.SpawnItems();
    }


    private void SpawnItems()
    {
        _playerSpawn = Vector3.zero;
        GameObject item;
        for (int i = 0; i < Size1D; i++)
        {
            if (_rnd.Next(0, 50) == 1 && _automata.CompleteGrid[i].isWalkable == true && _playerSpawn == Vector3.zero)
            {
                _playerSpawn = _automata.CompleteGrid[i].position;
                _playerIndex = i;
            }


            if (_rnd.Next(0, 20) == 1 && _grid[i].isWalkable && _grid[i].ItemValue == ItemValues.NULL)
            {
                item = GameObject.Instantiate(HexTileManager.instance.Rocks[_rnd.Next(0, HexTileManager.instance.Rocks.Length)], _grid[i].TileObject.transform.position, _grid[i].TileObject.transform.rotation) as GameObject;
                item.transform.SetParent(_grid[i].TileObject.transform);
                _grid[i].TileItem = item;
                _grid[i].ItemValue = item.GetComponent<MonoItem>().thisItem.ItemValue;
            }
            if (_rnd.Next(0, 20) == 1 && _grid[i].isWalkable && _grid[i].ItemValue == ItemValues.NULL)
            {
                item = GameObject.Instantiate(HexTileManager.instance.RawGems[_rnd.Next(0, HexTileManager.instance.RawGems.Length)], _grid[i].TileObject.transform.position, _grid[i].TileObject.transform.rotation) as GameObject;
                item.transform.SetParent(_grid[i].TileObject.transform);
                _grid[i].TileItem = item;
                _grid[i].ItemValue = item.GetComponent<MonoItem>().thisItem.ItemValue;
            }
            if (_rnd.Next(0, 50) == 1 && _grid[i].isWalkable && _grid[i].ItemValue == ItemValues.NULL)
            {
                item = GameObject.Instantiate(HexTileManager.instance.MoneyTiles[_rnd.Next(0, HexTileManager.instance.MoneyTiles.Length)], _grid[i].TileObject.transform.position, _grid[i].TileObject.transform.rotation) as GameObject;
                item.transform.SetParent(_grid[i].TileObject.transform);
                _grid[i].TileItem = item;
                _grid[i].ItemValue = item.GetComponent<MonoItem>().thisItem.ItemValue;
            }
            if (_rnd.Next(0, 20) == 1 && _grid[i].isWalkable && _grid[i].ItemValue == ItemValues.NULL)
            {
                item = GameObject.Instantiate(HexTileManager.instance.Containers[_rnd.Next(0, HexTileManager.instance.Containers.Length)], _grid[i].TileObject.transform.position, _grid[i].TileObject.transform.rotation) as GameObject;
                item.transform.SetParent(_grid[i].TileObject.transform);
                _grid[i].TileItem = item;
                _grid[i].ItemValue = item.GetComponent<MonoItem>().thisItem.ItemValue;
            }
            #region Fruits
            if (_rnd.Next(0, SpawnRates.UpFruits) == 1 && _grid[i].isWalkable && _grid[i].ItemValue == ItemValues.NULL)
            {
                item = GameObject.Instantiate(HexTileManager.instance.Fruits[_rnd.Next(0, HexTileManager.instance.Fruits.Length)], _grid[i].TileObject.transform.position, _grid[i].TileObject.transform.rotation) as GameObject;
                item.transform.SetParent(_grid[i].TileObject.transform);
                _grid[i].TileItem = item;
                _grid[i].ItemValue = item.GetComponent<MonoItem>().thisItem.ItemValue;
            }
            #endregion
        }

        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        _playerScript = (Instantiate(ResourcesManager.instance.Player, Vector3.zero, Quaternion.identity) as GameObject).GetComponent<Player>();
        _playerScript.transform.position = _playerSpawn;
        Fog.UpdateFogPlayerSpawn(_playerIndex);
        Fog.UpdateFog(_playerIndex, TileFogState.Active);
        ResourcesManager.instance.MainCamera.SetActive(false);
        _playerScript.InitPlayer(_playerIndex);
    }

    public static List<Grid> GetSurroundingHexes(Grid hexagon)
    {
        //return instance._automata.GetSurroundingHexes(hex);
        if (hexagon == null)
            return new List<Grid>();
        int x = hexagon.posX;
        int y = hexagon.posY;
        List<Grid> surroundingHexs = new List<Grid>();
        #region LEFT SIDE
        //Left upper sider
        if (y % 2 != 0)
        {
            if (!IsOutOfBounds(x, y - 1))
            {
                if (_grid[x + (y - 1) * Width].TileObject != null)
                    surroundingHexs.Add(_grid[x + (y - 1) * Width]);
            }
        }
        else if (!IsOutOfBounds(x - 1, y - 1))
        {
            if (_grid[(x - 1) + (y - 1) * Width].TileObject != null)
                surroundingHexs.Add(_grid[(x - 1) + (y - 1) * Width]);
        }


        //Left tile
        if (!IsOutOfBounds(x - 1, y))
        {
            if (_grid[(x - 1) + y * Width].TileObject != null)
                surroundingHexs.Add(_grid[(x - 1) + y * Width]);
        }
        //Left down tile
        if (y % 2 != 0)
        {
            if (!IsOutOfBounds(x, y + 1))
            {
                if (_grid[x + (y + 1) * Width].TileObject != null)
                    surroundingHexs.Add(_grid[x + (y + 1) * Width]);
            }
        }
        else if (!IsOutOfBounds(x - 1, y + 1))
        {
            if (_grid[(x - 1) + (y + 1) * Width].TileObject != null)
                surroundingHexs.Add(_grid[(x - 1) + (y + 1) * Width]);
        }

        #endregion
        #region RIGHT SIDE
        //Right down tile
        if (y % 2 == 0)
        {
            if (!IsOutOfBounds(x, y + 1))
            {
                if (_grid[x + (y + 1) * Width].TileObject != null)
                    surroundingHexs.Add(_grid[x + (y + 1) * Width]);
            }
        }
        else if (!IsOutOfBounds(x + 1, y + 1))
        {
            if (_grid[(x + 1) + (y + 1) * Width].TileObject != null)
                surroundingHexs.Add(_grid[(x + 1) + (y + 1) * Width]);
        }
        //Right tile
        if (!IsOutOfBounds(x + 1, y))
        {
            if (_grid[(x + 1) + y * Width].TileObject != null)
                surroundingHexs.Add(_grid[(x + 1) + y * Width]);
        }

        if (y % 2 == 0)
        {
            if (!IsOutOfBounds(x, y - 1))
            {
                if (_grid[x + (y - 1) * Width].TileObject != null)
                    surroundingHexs.Add(_grid[x + (y - 1) * Width]);
            }
        }
        else if (!IsOutOfBounds(x + 1, y - 1))
        {
            if (_grid[(x + 1) + (y - 1) * Width].TileObject != null)
                surroundingHexs.Add(_grid[(x + 1) + (y - 1) * Width]);
        }
        #endregion

        return surroundingHexs;
    }

    public static List<Grid> GetHexagonsInRadius(Grid center, int radius)
    {
        List<Grid> hexagons = new List<Grid>();
        bool startIsModulo;
        if (center.posY % 2 == 0)
            startIsModulo = true;
        else
            startIsModulo = false;

        int end = center.posX + radius;
        int start = center.posX - radius;
        for (int y = center.posY - radius; y <= center.posY + radius; y++)
        {
            if (y % 2 == 0)
            {
                if (startIsModulo)
                {
                    start = center.posX - radius;
                    end = center.posX + radius;
                }
                else
                {
                    start = center.posX - (radius - 1);
                    end = center.posX + radius;
                }
            }
            else
            {
                if (startIsModulo)
                {
                    start = center.posX - radius;
                    end = center.posX + (radius - 1);
                }
                else
                {
                    start = center.posX - radius;
                    end = center.posX + radius;
                }
            }
            
            for (int x = start; x <= end; x++)
            {
                if (!IsOutOfBounds(x, y))
                {
                    if (_grid[x + y * Height].TileObject != null)
                        hexagons.Add(_grid[x + y * Height]);
                }
            }
        }
        return hexagons;
    }

    public static List<Grid> GetSurroundingHexes(Grid hexagon, int offset)
    {
        //Debug.Log("Center: " + hexagon.Coordinates);
        List<Grid> allOffsetGrid = GetHexagonsInRadius(hexagon, offset);
        List<Grid> inside = GetHexagonsInRadius(hexagon, offset - 1);
       
        foreach(Grid grd in inside)
        {
            if (allOffsetGrid.Contains(grd))
                allOffsetGrid.Remove(grd);
        }
        return allOffsetGrid;
    }

    private static bool IsOutOfBounds(int x, int y)
    {
        if (x < 0 || y < 0)
            return true;
        if (x >= Width || y >= Height)
            return true;
        return false;
    }
}
