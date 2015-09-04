using UnityEngine;
using System.Collections;
using DungeonSpawner;
using System;
using Random = System.Random;
using System.IO;
using System.Collections.Generic;

public class HexCellularAutomataIndependent : ICellularAutomata
{
    #region private variables
    private Random _rnd;
    private Grid[] _grid;
    private string _seed = null;

    //private Transform _boardHolder;
    #endregion
    public static float _hexWidth;
    public static float _hexHeight;
    #region Properties
    private int _mapWidth;
    public int MapWidth
    {
        get
        {
            return _mapWidth;
        }
        set
        {
            _mapWidth = value;
        }
    }
    private int _mapHeight;
    public int MapHeight
    {
        get
        {
            return _mapHeight;
        }
        set
        {
            _mapHeight = value;
        }
    }
    private int _size1D;
    public int Size1D
    {
        get
        {
            return _size1D;
        }
    }
    private int _percentage = 45;
    public int Percentage
    {
        get
        {
            return _percentage;
        }
        set
        {
            _percentage = value;
        }
    }
    private RawGrid[] _map;
    public RawGrid[] Map
    {
        get
        {
            return _map;
        }
    }
    #endregion


    public HexCellularAutomataIndependent(int width, int height, Random rnd)
    {
        _mapWidth = width;
        _mapHeight = height;
        _size1D = _mapHeight * _mapWidth;
        _rnd = rnd;
    }

    public void SetSize(int width, int height)
    {
        _mapWidth = width;
        _mapHeight = height;
    }

    //Method to initialise Hexagon width and height
    private void setSizes()
    {
        //renderer component attached to the Hex prefab is used to get the current width and height
        _hexWidth = HexTileManager.instance.Hex.GetComponent<Renderer>().bounds.size.x;
        _hexHeight = HexTileManager.instance.Hex.GetComponent<Renderer>().bounds.size.y;
    }

    /// <summary>
    /// Get the surrounding tiles of the hexagon parameter [Only visible hex]
    /// </summary>
    /// <param name="hexagon"></param>
    /// <returns></returns>
    public List<Grid> GetSurroundingHexes(Grid hexagon)
    {
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
                if (_grid[x + (y - 1) * _mapWidth].TileObject != null)
                    surroundingHexs.Add(_grid[x + (y - 1) * _mapWidth]);
            }
        }
        else if (!IsOutOfBounds(x - 1, y - 1))
        {
            if (_grid[(x - 1) + (y - 1) * _mapWidth].TileObject != null)
                surroundingHexs.Add(_grid[(x - 1) + (y - 1) * _mapWidth]);
        }


        //Left tile
        if (!IsOutOfBounds(x - 1, y))
        {
            if (_grid[(x - 1) + y * _mapWidth].TileObject != null)
                surroundingHexs.Add(_grid[(x - 1) + y * _mapWidth]);
        }
        //Left down tile
        if (y % 2 != 0)
        {
            if (!IsOutOfBounds(x, y + 1))
            {
                if (_grid[x + (y + 1) * _mapWidth].TileObject != null)
                    surroundingHexs.Add(_grid[x + (y + 1) * _mapWidth]);
            }
        }
        else if (!IsOutOfBounds(x - 1, y + 1))
        {
            if (_grid[(x - 1) + (y + 1) * _mapWidth].TileObject != null)
                surroundingHexs.Add(_grid[(x - 1) + (y + 1) * _mapWidth]);
        }

        #endregion
        #region RIGHT SIDE
        //Right down tile
        if (y % 2 == 0)
        {
            if (!IsOutOfBounds(x, y + 1))
            {
                if (_grid[x + (y + 1) * _mapWidth].TileObject != null)
                    surroundingHexs.Add(_grid[x + (y + 1) * _mapWidth]);
            }
        }
        else if (!IsOutOfBounds(x + 1, y + 1))
        {
            if (_grid[(x + 1) + (y + 1) * _mapWidth].TileObject != null)
                surroundingHexs.Add(_grid[(x + 1) + (y + 1) * _mapWidth]);
        }
        //Right tile
        if (!IsOutOfBounds(x + 1, y))
        {
            if (_grid[(x + 1) + y * _mapWidth].TileObject != null)
                surroundingHexs.Add(_grid[(x + 1) + y * _mapWidth]);
        }

        if (y % 2 == 0)
        {
            if (!IsOutOfBounds(x, y - 1))
            {
                if (_grid[x + (y - 1) * _mapWidth].TileObject != null)
                    surroundingHexs.Add(_grid[x + (y - 1) * _mapWidth]);
            }
        }
        else if (!IsOutOfBounds(x + 1, y - 1))
        {
            if (_grid[(x + 1) + (y - 1) * _mapWidth].TileObject != null)
                surroundingHexs.Add(_grid[(x + 1) + (y - 1) * _mapWidth]);
        }
        #endregion

        return surroundingHexs;
    }

    private List<RawGrid> GetSurroundingHexes(int posX, int posY, int radius)
    {
        List<RawGrid> hexagons = new List<RawGrid>();
        bool startIsModulo;
        if (posY % 2 == 0)
            startIsModulo = true;
        else
            startIsModulo = false;

        int end = posX + radius;
        int start = posX - radius;
        for (int y = posY - radius; y <= posY + radius; y++)
        {
            if (y % 2 == 0)
            {
                if (startIsModulo)
                {
                    start = posX - radius;
                    end = posX + radius;
                }
                else
                {
                    start = posX - (radius - 1);
                    end = posX + radius;
                }
            }
            else
            {
                if (startIsModulo)
                {
                    start = posX - radius;
                    end = posX + (radius - 1);
                }
                else
                {
                    start = posX - radius;
                    end = posX + radius;
                }
            }

            for (int x = start; x <= end; x++)
            {
                if (!IsOutOfBounds(x, y))
                {
                    if (_map[x + y * _mapHeight].BasicValue != 1)
                        hexagons.Add(_map[x + y * _mapHeight]);
                }
            }
        }
        return hexagons;
    }

    public void ProcessCavern(int pass)
    {
        DateTime time = DateTime.Now;
        for (int i = 0; i < pass; i++)
        {
            PlaceWalls_1D5678_2D12(3);
            PlaceWalls_1B345678(2);
            PlaceWalls_1D5678(2);
        }

        //SpawnGrassMultiple();
        #region FloodCavern/Check for isolated caves
        if (!CheckFloodCavern())
        {
            for (int i = 0; i < 10; i++)
            {
                FloodCavern();
                JoinCaves();

                if (CheckFloodCavern())
                    break;
                else
                    ResetFloodValue();
            }
        }
        //Debug.Log(CheckFloodCavern());
        #endregion
        GetAccessibleIndexes();
    }
    #region PlaceWalls Born/Death

    public void PlaceWalls_1D5678_2D12(int pass)
    {
        for (int i = 0; i < pass; i++)
        {
            for (int x = 0; x < _mapWidth; x++)
            {
                for (int y = 0; y < _mapHeight; y++)
                {
                    if (GetAdjacentWalls(x, y, 1, 1) >= 5 || GetAdjacentWalls(x, y, 2, 2) <= 2) //DIE!
                        _map[x + y * _mapHeight].BasicValue = 1;
                    else
                        _map[x + y * _mapHeight].BasicValue = 0;
                }
            }
        }
    }
    public void PlaceWalls_1D5678_2D1(int pass)
    {
        for (int i = 0; i < pass; i++)
        {
            for (int x = 0; x < _mapWidth; x++)
            {
                for (int y = 0; y < _mapHeight; y++)
                {
                    if (GetAdjacentWalls(x, y, 1, 1) >= 5 || GetAdjacentWalls(x, y, 2, 2) <= 1) //DIE!
                        _map[x + y * _mapHeight].BasicValue = 1;
                    else
                        _map[x + y * _mapHeight].BasicValue = 0;
                }
            }
        }
    }
    public void PlaceWalls_1B345678(int pass)
    {
        for (int i = 0; i < pass; i++)
        {
            for (int x = 0; x < _mapWidth; x++)
            {
                for (int y = 0; y < _mapHeight; y++)
                {
                    if (GetAdjacentWalls(x, y, 1, 1) < 3 && _map[x + y * _mapHeight].BasicValue == 1)
                        _map[x + y * _mapHeight].BasicValue = 0;
                    if (GetAdjacentWalls(x, y, 1, 1) > 4 && _map[x + y * _mapHeight].BasicValue != 1)
                        _map[x + y * _mapHeight].BasicValue = 1;
                }
            }
        }
    }
    public void PlaceWalls_1D5678(int pass)
    {
        for (int i = 0; i < pass; i++)
        {
            for (int x = 0; x < _mapWidth; x++)
            {
                for (int y = 0; y < _mapHeight; y++)
                {
                    if (GetAdjacentWalls(x, y, 1, 1) >= 5) //DIE!
                        _map[x + y * _mapHeight].BasicValue = 1;
                    else
                        _map[x + y * _mapHeight].BasicValue = 0;
                }
            }
        }
    }
    #endregion

    public int GetAdjacentWalls(int x, int y, int offX, int offY)
    {
        int walls = 0;
        int startX = x - offX;
        int startY = y - offY;
        int endX = x + offX;
        int endY = y + offY;

        int iX = startX;
        int iY = startY;
        if (offX == 1)
        {
            for (iX = startX; iX <= endX; iX++)
            {
                for (iY = startY; iY <= endY; iY++)
                {
                    if (IsWall(iX, iY))
                        walls += 1;
                }
            }
        }
        else
        {
            for (iX = startX; iX <= endX; iX++)
            {
                for (iY = startY; iY <= endY; iY++)
                {
                    if (iX != (x - 1) && iY != (y - 1))
                    {
                        if (IsWall(iX, iY))
                            walls += 1;
                    }
                }
            }
        }
        return walls;
    }

    public void FillAdjacentWalls(int x, int y)
    {
        int startX = x - 1;
        int startY = y - 1;
        int endX = x + 1;
        int endY = y + 1;

        int iX = startX;
        int iY = startY;

        for (iX = startX; iX <= endX; iX++)
        {
            for (iY = startY; iY <= endY; iY++)
            {
                if (!(iX == x && iY == y))
                {
                    if (IsWall(iX, iY) && !IsOutOfBounds(iX, iY))
                        _grid[iX + iY * _mapHeight].tile = Tile.Wall;
                }
            }
        }
    }



    public void RandomFillMap()
    {
        setSizes();
        _map = new RawGrid[_mapWidth * _mapHeight];
        _grid = new Grid[_mapWidth * _mapHeight];

        int index;
        for (int x = 0; x < _mapWidth; x++)
        {
            for (int y = 0; y < _mapHeight; y++)
            {
                index = x + y * _mapHeight;
                _map[index] = new RawGrid();
                _grid[index] = new Grid(calcWorldCoord(new Vector2(x, y)), Tile.Full);
                _grid[index].posX = x;
                _grid[index].posY = y;
                //Border
                if (x == 0 || y == 0 || (x == _mapWidth - 1) || (y == _mapHeight - 1))
                    _map[index].BasicValue = BasicValues.Wall;
                // Inside
                else
                    _map[index].BasicValue = GetRandomTileStatus(_percentage);
            }
        }
    }

    //Method to calculate the position of the first hexagon tile
    //The center of the hex grid is (0,0,0)
    Vector3 calcInitPos()
    {
        Vector3 initPos;
        var xCenter = (_mapWidth) / 2f; //25
        var zCenter = (_mapHeight) / 2f;// -0.75f;
        //Debug.Log("centers: " + xCenter + " " + zCenter);
        //Debug.Log("widths: " + hexWidth + " " + hexHeight);
        //the initial position will be in the left upper corner
        initPos = new Vector3(xCenter * -_hexWidth + (_hexWidth / 2f), zCenter * _hexHeight - _hexHeight / 2f, 0); //Switched y and z
        //Debug.Log(initPos);

        return initPos;
    }


    //method used to convert hex grid coordinates to game world coordinates
    public Vector3 calcWorldCoord(Vector2 gridPos)
    {
        //Position of the first hex tile
        Vector3 initPos = calcInitPos();
        //Every second row is offset by half of the tile width
        float offset = 0;
        if (gridPos.y % 2 != 0)
            offset = _hexWidth / 2;

        float x = initPos.x + offset + gridPos.x * _hexWidth;
        //Every new line is offset in z direction by 3/4 of the hexagon height
        float z = initPos.y - gridPos.y * _hexHeight * 0.75f; //init.z => y
        return new Vector3(x, z, 0);
    }

    private bool IsWall(int x, int y)
    {
        if (IsOutOfBounds(x, y))
            return true; //true
        if (_map[x + y * _mapHeight].BasicValue == 1)
            return true;
        else
            return false;
    }

    private bool IsOutOfBounds(int x, int y)
    {
        if (x < 0 || y < 0)
            return true;
        if (x >= _mapWidth || y >= _mapHeight)
            return true;
        return false;
    }

    private byte GetRandomTileStatus(int percent)
    {
        if (percent >= _rnd.Next(0, 101))
        {
            return 1;
        }
        return 0;
    }

    #region Flood fill algorithm
    private byte _floodValue = 0;
    private int[] _validIndexes;
    private int[] _caves; //Storedcaverns
    private int[] _cavesRandomPoint;
    private void FloodCavern()
    {
        _floodValue = 0; //Initial flood value
        //Start the algorithm
        FloodStart();
        //Fill the rest of the image

        int index;
        for (int x = 0; x < _mapWidth; x++)
        {
            for (int y = 0; y < _mapHeight; y++)
            {
                index = x + y * _mapHeight;
                if (_map[index].BasicValue != 1)
                {
                    if (_map[index].FloodValue != 0)
                        GetAdjacentFloodValue(x, y, _floodValue);
                    else //New cavern?
                    {
                        if (CheckAdjacentFloodValue(x, y))
                            _map[index].FloodValue = GetAdjacentFloodValue(x, y, _floodValue);
                        else
                        {
                            _floodValue++;
                            _map[index].FloodValue = GetAdjacentFloodValue(x, y, _floodValue);
                        }
                    }

                }
            }
        }
        _caves = new int[_floodValue];
        _cavesRandomPoint = new int[_floodValue];
        int[] cavePoints = new int[_floodValue];
        for (int i = 0; i < _floodValue; i++)
        {
            _cavesRandomPoint[i] = -1;
            cavePoints[i] = 9999999;
        }
        int middle = _size1D / 2;
        for (int i = 0; i < _size1D; i++)
        {
            if (_map[i].FloodValue > 0)
            {
                _caves[_map[i].FloodValue - 1]++;
                if (Mathf.Abs(i - middle) < Mathf.Abs(cavePoints[_map[i].FloodValue - 1] - middle))
                {
                    cavePoints[_map[i].FloodValue - 1] = i;
                    _cavesRandomPoint[_map[i].FloodValue - 1] = i;
                }
            }
        }
        /*for (int cnt = 0; cnt < _floodValue; cnt++)
            Debug.Log(string.Format("Cavern #{0}: {1} tiles", cnt, _caves[cnt]));*/
    }

    /// <summary>
    /// Initialize the flood fill algorithm
    /// </summary>
    private void FloodStart()
    {
        //Get the first empty block:
        for (int x = 0; x < _mapWidth; x++)
        {
            for (int y = 0; y < _mapHeight; y++)
            {
                if (_map[x + y * _mapHeight].BasicValue != 1)
                {
                    _floodValue++;
                    _map[x + y * _mapHeight].FloodValue = GetAdjacentFloodValue(x, y, _floodValue);
                    return;
                }
            }
        }
    }

    private bool CheckFloodCavern()
    {
        for (int i = 0; i < _size1D; i++)
        {
            /*for (int x = 0; x < _mapWidth; x++)
            {
                for (int y = 0; y < _mapHeight; y++)
                {*/
            if (_map[i].BasicValue != 1 && _map[i].FloodValue == 0)
                return false;
            /*}
        }*/
        }
        return true;
    }

    private byte GetAdjacentFloodValue(int x, int y, byte floodValue)
    {
        int startX = x - 1;
        int startY = y - 1;
        int endX = x + 1;
        int endY = y + 1;
        //Debug.Log(_map[startX + y * _mapHeight].BasicValue + "  " + _map[startX + y * _mapHeight].BasicValue + "  " + _map[startX + y * _mapHeight].BasicValue + "  " + _map[startX + y * _mapHeight].BasicValue);
        if (!IsOutOfBounds(startX, y) && _map[startX + y * _mapHeight].BasicValue != 1 && _map[startX + y * _mapHeight].FloodValue == 0)
        {
            _map[startX + y * _mapHeight].FloodValue = floodValue;
            GetAdjacentFloodValue(startX, y, floodValue);
        }
        if (!IsOutOfBounds(endX, y) && _map[endX + y * _mapHeight].BasicValue != 1 && _map[endX + y * _mapHeight].FloodValue == 0)
        {
            _map[endX + y * _mapHeight].FloodValue = floodValue;
            GetAdjacentFloodValue(endX, y, floodValue);
        }
        if (!IsOutOfBounds(x, startY) && _map[x + startY * _mapHeight].BasicValue != 1 && _map[x + startY * _mapHeight].FloodValue == 0)
        {
            _map[x + startY * _mapHeight].FloodValue = floodValue;
            GetAdjacentFloodValue(x, startY, floodValue);
        }
        if (!IsOutOfBounds(x, endY) && _map[x + endY * _mapHeight].BasicValue != 1 && _map[x + endY * _mapHeight].FloodValue == 0)
        {
            _map[x + endY * _mapHeight].FloodValue = floodValue;
            GetAdjacentFloodValue(x, endY, floodValue);
        }
        return floodValue;
    }

    private bool CheckAdjacentFloodValue(int x, int y)
    {
        int startX = x - 1;
        int startY = y - 1;
        int endX = x + 1;
        int endY = y + 1;

        if (!IsOutOfBounds(startX, y) && _map[startX + y * _mapHeight].FloodValue != 0)
            return true;
        if (!IsOutOfBounds(endX, y) && _map[endX + y * _mapHeight].FloodValue != 0)
            return true;

        if (!IsOutOfBounds(x, startY) && _map[x + startY * _mapHeight].FloodValue != 0)
            return true;
        if (!IsOutOfBounds(x, endY) && _map[x + endY * _mapHeight].FloodValue != 0)
            return true;

        return false;
    }
    private bool CheckAdjacentFloodValue(int x, int y, int defaultFloodValue)
    {
        int startX = x - 1;
        int startY = y - 1;
        int endX = x + 1;
        int endY = y + 1;

        int found = 0;

        if (!IsOutOfBounds(startX, y) && _map[startX + y * _mapHeight].FloodValue == defaultFloodValue)
            found++;
        if (!IsOutOfBounds(endX, y) && _map[endX + y * _mapHeight].FloodValue == defaultFloodValue)
            found++;

        if (!IsOutOfBounds(x, startY) && _map[x + startY * _mapHeight].FloodValue == defaultFloodValue)
            found++;
        if (!IsOutOfBounds(x, endY) && _map[x + endY * _mapHeight].FloodValue == defaultFloodValue)
            found++;

        if (found >= 2)
            return true;
        else
            return false;
    }

    private void JoinCaves()
    {
        int[] middleArray = new int[9];
        int startX = (_mapWidth / 2) - 1;
        int startY = (_mapHeight / 2) - 1;
        int endX = (_mapWidth / 2) + 1;
        int endY = (_mapHeight / 2) + 1;

        int iX = startX;
        int iY = startY;

        int cnt = 0;
        for (iX = startX; iX <= endX; iX++)
        {
            for (iY = startY; iY <= endY; iY++)
            {
                middleArray[cnt] = iX + iY * _mapHeight;
                cnt++;
            }
        }
        int largeID = 0;
        int tmp = 0;
        //Get the largest cave id
        for (int i = 0; i < _caves.Length; i++)
        {
            if (_caves[i] > tmp)
            {
                largeID = i;
                tmp = _caves[i];
            }
        }

        //Dig each cave until find another floodValue
        float a;
        float b;
        int x1;
        float x2 = _mapWidth / 2f;
        int y1;
        int[] linePoints;
        for (int i = 0; i < _caves.Length; i++)
        {
            if (i != largeID)
            {
                x1 = _cavesRandomPoint[i] - (int)(_cavesRandomPoint[i] / _mapWidth) * _mapWidth;
                y1 = (int)(_cavesRandomPoint[i] / _mapHeight);
                a = ((float)y1 - (_mapHeight / 2f)) / ((float)x1 - (_mapWidth / 2f));
                b = -a * (_mapWidth / 2f) + (_mapHeight / 2f);


                //Get all points on the current equation:
                if (x1 < x2)
                {
                    linePoints = new int[(int)x2 - x1];
                    cnt = 0;
                    for (int x = x1; x < x2; x++)
                    {
                        linePoints[cnt] = x + (int)(a * x + b) * _mapHeight;
                        cnt++;
                    }
                    for (int j = 0; j < linePoints.Length; j++)
                    {
                        if (_map[linePoints[j]].BasicValue == 1 && _map[linePoints[j]].FloodValue == 0 || _map[linePoints[j]].BasicValue != 1 && _map[linePoints[j]].FloodValue == _caves[i])
                        {
                            _map[linePoints[j]].BasicValue = 0;
                            _map[linePoints[j]].FloodValue = (byte)(i + 1);
                            AddAdjacentNoise(linePoints[j] - (int)(linePoints[j] / _mapWidth) * _mapWidth, linePoints[j] / _mapHeight, _map[linePoints[j]].FloodValue);
                        }
                        else if (_map[linePoints[j]].BasicValue != 1 && _map[linePoints[j]].FloodValue != (byte)(i + 1))
                            break;
                    }
                }
                else
                {
                    linePoints = new int[x1 - (int)x2];
                    cnt = 0;
                    for (int x = x1; x > x2; x--)
                    {
                        linePoints[cnt] = x + (int)(a * x + b) * _mapHeight;
                        cnt++;
                    }
                    for (int j = 0; j < linePoints.Length; j++)
                    {
                        if (_map[linePoints[j]].BasicValue == 1 && _map[linePoints[j]].FloodValue == 0 || _map[linePoints[j]].BasicValue != 1 && _map[linePoints[j]].FloodValue == _caves[i])
                        {
                            _map[linePoints[j]].BasicValue = 0;
                            _map[linePoints[j]].FloodValue = (byte)(i + 1);
                            AddAdjacentNoise(linePoints[j] - (int)(linePoints[j] / _mapWidth) * _mapWidth, linePoints[j] / _mapHeight, _map[linePoints[j]].FloodValue);
                        }
                        else if (_map[linePoints[j]].BasicValue != 1 && _map[linePoints[j]].FloodValue != (byte)(i + 1))
                            break;
                    }
                }
            }
        }
    }

    private void AddAdjacentNoise(int x, int y, byte floodValue)
    {
        int startX = x - 1;
        int startY = y - 1;
        int endX = x + 1;
        int endY = y + 1;

        if (!IsOutOfBounds(startX, y))
        {
            if (_map[startX + y * _mapHeight].BasicValue == 1)
                _map[startX + y * _mapHeight].BasicValue = 0;
            _map[startX + y * _mapHeight].FloodValue = floodValue;
        }

        if (!IsOutOfBounds(endX, y))
        {
            if (_map[endX + y * _mapHeight].BasicValue == 1)
                _map[endX + y * _mapHeight].BasicValue = 0;
            _map[endX + y * _mapHeight].FloodValue = floodValue;
        }

        if (!IsOutOfBounds(x, startY))
        {
            if (_map[x + startY * _mapHeight].BasicValue == 1)
                _map[x + startY * _mapHeight].BasicValue = 0;
            _map[x + startY * _mapHeight].FloodValue = floodValue;
        }

        if (!IsOutOfBounds(x, endY))
        {
            if (_map[x + endY * _mapHeight].BasicValue == 1)
                _map[x + endY * _mapHeight].BasicValue = 0;
            _map[x + endY * _mapHeight].FloodValue = floodValue;
        }
    }

    private void ResetFloodValue()
    {
        for (int i = 0; i < _size1D; i++)
            _map[i].FloodValue = 0;
    }

    /// <summary>
    /// Get only the valid indexes => Better for placing things on the map [not browsing 1 tiles]
    /// </summary>
    private void GetAccessibleIndexes()
    {
        int count = 0;
        for (int i = 0; i < _size1D; i++)
        {
            if (_map[i].BasicValue != 1)
                count++;
        }

        _validIndexes = new int[count];
        count = 0;
        for (int i = 0; i < _size1D; i++)
        {
            if (_map[i].BasicValue != 1)
            {
                _validIndexes[count] = i;
                count++;
            }
        }
    }
    #endregion



    #region Output
    public void SaveToFile()
    {
        using (StreamWriter writer = new StreamWriter(File.Create(string.Format("{0}.txt", _rnd.GetHashCode()))))
        {
            string str = "";
            for (int x = 0; x < _mapWidth; x++)
            {
                str = "";
                for (int y = _mapHeight - 1; y != 0; y--)
                {
                    if (_map[x + y * _mapHeight].BasicValue != 1)
                        str += _map[x + y * _mapHeight].FloodValue;
                    else
                    {
                        if (_floodValue >= 10)
                            str += "##";
                        else
                            str += "#";
                    }
                }
                writer.WriteLine(str);
            }
        }
    }

    public void PrintMap()
    {

    }

    #endregion
}
