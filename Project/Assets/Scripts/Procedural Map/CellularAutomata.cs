namespace DungeonSpawner
{
    using UnityEngine;
    using System.Collections;
    using System;
    using System.IO;
    using System.Collections.Generic;
    using Random = System.Random;

    public class CellularAutomata : ICellularAutomata
    {
        private Random _rnd;
        private string _seed = null;
        public GameObject BlackTile;
        public Transform Player;

        private int _playerSpawn;

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
        private int _percentage = 45;
        private int _size1D;
        public int Size1D
        {
            get
            {
                return _size1D;
            }
        }
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

        private Transform _boardHolder;
        private RawGrid[] _map;
        public RawGrid[] Map
        {
            get
            {
                return _map;
            }
        }
        private Grid[] _grid;
        public Grid[] CompleteGrid { get { return _grid; } }
        public CellularAutomata(int width, int height, Random rnd)
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

        public List<Grid> GetSurroundingHexes(Grid hex)
        {
            return new List<Grid>();
        }

        public void ProcessCavern(int pass)
        {
            for (int i = 0; i < pass; i++)
            {
                PlaceWalls_1D5678_2D12(3);
                PlaceWalls_1B345678(2);
                PlaceWalls_1D5678(2);
            }

            //reduc space in the cavern
            //PlaceWalls_1D5678_2D1(1);


            //Spawn other cave elements:
            SpawnWaterMultiple();
            //SpawnGrassMultiple();
            #region FloodCavern/Check for isolated caves
            if (!CheckFloodCavern())
            {
                for (int i = 0; i < 10; i++)
                {
                    FloodCavern();
                    JoinCaves();

                    if (CheckFloodCavern())
                    {
                        Debug.Log("Flood iterations:" + (i+1));
                        break;
                    }
                    else
                        ResetFloodValue();
                }
            }


            Debug.Log(CheckFloodCavern());
            #endregion

            //SaveToFile();
            ProcessBorders();
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
                        /*if (!(iX == x && iY == y))
                        {*/
                        if (IsWall(iX, iY))
                        {
                            walls += 1;
                        }
                        //}
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
                                /*if (!(iX == x && iY == y))
                                {*/
                                if (IsWall(iX, iY))
                                {
                                    walls += 1;
                                    //}
                                }
                            }
                        }
                }
            }
            return walls;
        }

        public void ProcessBorders()
        {
            int index;
            for (int x = 0; x < _mapWidth; x++)
            {
                for (int y = 0; y < _mapHeight; y++)
                {
                    index = x + y * _mapHeight;
                    if (_map[index].BasicValue != 1)
                    {
                        _grid[index].status = Status.Revealed;
                        _grid[index].BasicValue = _map[index].BasicValue;
                        switch (_map[index].BasicValue)
                        {
                            case 0: _grid[index].tile = Tile.Floor; break;
                            case 2: _grid[index].tile = Tile.Water; break;
                            case 3: _grid[index].tile = Tile.Grass; break;
                        }
                        FillAdjacentWalls(x, y);
                    }
                }
            }
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
                        {
                            _grid[iX + iY * _mapHeight].tile = Tile.Wall;
                        }
                    }
                }
            }
        }


        public void Destroy()
        {
            if (_boardHolder != null)
                GameObject.Destroy(_boardHolder.gameObject);
            if(_seed != null)
                _rnd = new Random(_seed.GetHashCode());
        }

        

        public void RandomFillMap()
        {
            _map = new RawGrid[_mapWidth * _mapHeight];
            _grid = new Grid[_mapWidth * _mapHeight];
            int middle = 0;
            int index;
            for (int x = 0; x < _mapWidth; x++)
            {
                for (int y = 0; y < _mapHeight; y++)
                {
                    index = x + y * _mapHeight;
                    _map[index] = new RawGrid();
                    _grid[index] = new Grid(new Vector3(x, y, 0f), Tile.Full);
                    //Border
                    if (x == 0 || y == 0 || (x == _mapWidth - 1) || (y == _mapHeight - 1))
                    {
                        _map[index].BasicValue = 1;
                    }
                    // Inside
                    else
                    {
                        /*middle = (_mapHeight / 2);

                        if (y == middle)
                            _map[x + y * _mapHeight].BasicValue = 0;
                        else*/
                            _map[index].BasicValue = GetRandomTileStatus(_percentage);
                    }
                }
            }
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
        private int[] _caves; //Storedcaverns
        private int[] _cavesRandomPoint;
        private void FloodCavern()
        {
            _floodValue = 0;
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
            Debug.Log("Number of caverns: " + _floodValue + "  " + CheckFloodCavern());
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
            for (int x = 0; x < _mapWidth; x++)
            {
                for (int y = 0; y < _mapHeight; y++)
                {
                    if (_map[x + y * _mapHeight].BasicValue != 1 && _map[x + y * _mapHeight].FloodValue == 0)
                        return false;    
                }
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

            if(found >= 2)
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
            for(int i = 0; i < _caves.Length; i++)
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
            for(int i = 0; i < _caves.Length; i++)
            {
                if(i != largeID)
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
                        for(int j = 0; j < linePoints.Length; j ++)
                        {
                            if (_map[linePoints[j]].BasicValue == 1 && _map[linePoints[j]].FloodValue == 0 || _map[linePoints[j]].BasicValue != 1 && _map[linePoints[j]].FloodValue == _caves[i])
                            {
                                _map[linePoints[j]].BasicValue = 0;
                                _map[linePoints[j]].FloodValue = (byte)(i+1);
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
        #endregion

        #region SpawnGrass
        private void SpawnGrassMultiple()
        {
            //Get a random number of water spots:
            int waterSpots = _rnd.Next(4, 15);
            Debug.Log("OnSpawnGrass: " + waterSpots);
            int sizeX;
            int sizeY;
            int posX;
            int posY;


            //Setup a random cellular automata for each spot:
            CellularAutomata[] water = new CellularAutomata[waterSpots];
            for(int i = 0; i < waterSpots; i++)
            {
                //Get a random size for the cellular automata:
                //sizeX = _rnd.Next(4, 11);
                sizeY = _rnd.Next(20, 30);
                sizeX = sizeY + 2;
                water[i] = new CellularAutomata(sizeX, sizeY, _rnd);
                water[i].RandomFillMap();
                water[i].PlaceWalls_1D5678_2D1(4);

                //Select a random position for the water in the actual grid:
                posX = _rnd.Next(0, _mapWidth - sizeX);
                posY = _rnd.Next(0, _mapHeight - sizeY);
                //Merge the water map and the current map:
                for(int x = 0; x < water[i]._mapWidth; x++)
                {
                    for (int y = 0; y < water[i]._mapHeight; y++)
                    {
                        if (water[i].Map[x + y * water[i]._mapHeight].BasicValue == 0)
                        {
                            if (_map[(posX + x) + (posY + y) * _mapHeight].BasicValue != 1 && _map[(posX + x) + (posY + y) * _mapHeight].BasicValue != 2)
                                _map[(posX + x) + (posY + y) * _mapHeight].BasicValue = 3;
                        }
                    }
                }
            }
        }
        #endregion

        #region SpawnWater
        private void SpawnWaterMultiple()
        {
            //Get a random number of water spots:
            int waterSpots = _rnd.Next(4, 15);
            Debug.Log("OnSpawnWater: " + waterSpots);
            int sizeX;
            int sizeY;
            int posX;
            int posY;


            //Setup a random cellular automata for each spot:
            CellularAutomata[] water = new CellularAutomata[waterSpots];
            for (int i = 0; i < waterSpots; i++)
            {
                //Get a random size for the cellular automata:
                //sizeX = _rnd.Next(4, 11);
                sizeY = _rnd.Next(8, 15);
                sizeX = sizeY +_rnd.Next(0, 5);
                water[i] = new CellularAutomata(sizeX, sizeY, _rnd);
                water[i].RandomFillMap();
                water[i].PlaceWalls_1D5678_2D1(3);

                //Select a random position for the water in the actual grid:
                posX = _rnd.Next(0, _mapWidth - sizeX);
                posY = _rnd.Next(0, _mapHeight - sizeY);
                //Merge the water map and the current map:
                for (int x = 0; x < water[i]._mapWidth; x++)
                {
                    for (int y = 0; y < water[i]._mapHeight; y++)
                    {
                        if (water[i].Map[x + y * water[i]._mapHeight].BasicValue == 0)
                        {
                            _map[(posX + x) + (posY + y) * _mapHeight].BasicValue = 2;
                        }
                    }
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
            _boardHolder = new GameObject("Board").transform;
            Scene._grid = _grid;
            for (int x = 0; x < _mapWidth; x++)
            {
                for (int y = 0; y < _mapHeight; y++)
                {
                    GameObject toInstantiate = null;
                    switch (Scene._grid[x + y * _mapHeight].tile)
                    {
                        case Tile.Floor:
                            toInstantiate = TileManager.instance.FloorTiles[_rnd.Next(0, TileManager.instance.FloorTiles.Length)];
                            break;
                        case Tile.Water:
                            toInstantiate = TileManager.instance.WaterTiles[_rnd.Next(0, TileManager.instance.WaterTiles.Length)];
                            break;
                        case Tile.Grass:
                            toInstantiate = TileManager.instance.GrassTiles[_rnd.Next(0, TileManager.instance.GrassTiles.Length)];
                            break;
                        case Tile.Wall:
                            toInstantiate = TileManager.instance.WallTiles[_rnd.Next(0, TileManager.instance.WallTiles.Length)];
                            break;
                        case Tile.Ground:
                            toInstantiate = TileManager.instance.GroundTiles[_rnd.Next(0, TileManager.instance.GroundTiles.Length)];
                            break;
                    }
                    if (toInstantiate != null)
                    {
                        GameObject instance = GameObject.Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
                        Scene._grid[x + y * _mapHeight].TileObject = instance;
                        GameObject fog = GameObject.Instantiate(TileManager.instance.Fog, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
                        fog.transform.SetParent(instance.transform);
                        instance.transform.SetParent(_boardHolder);
                    }
                }
            }
        }
        #endregion
    }
}
