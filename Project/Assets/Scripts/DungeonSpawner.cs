namespace DungeonSpawner
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using Random = UnityEngine.Random;

    public class DungeonSpawner : MonoBehaviour
    {
        public GameObject[] FloorSprites;
        public GameObject[] WallSprites;
        public GameObject BlackTile;
        public GameObject[] Water;
        public GameObject[] Grass;
        public Transform Player;
        CellularAutomata _automata;

        public int MapWidth = 100;
        public int MapHeight = 100;

        private int _minimumCorridorLength = 5;
        private Grid[] _grid;
        
        private Transform _boardHolder;


        void Start()
        {
            //BuildDungeon(Random.Range(7, 20));
            _automata = new CellularAutomata(100, 100, new System.Random());
        }

        /*void OnGUI()
        {
            if(GUI.Button(new Rect(10,10, 100, 100), "Refresh"))
            {
                BuildDungeon(Random.Range(7, 20));
            }
            if (GUI.Button(new Rect(10, 110, 100, 100), "Automata"))
            {
                _automata.Destroy();
                _automata.FloorSprites = FloorSprites;
                _automata.WallSprites = WallSprites;
                _automata.BlackTile = BlackTile;
                _automata.RandomFillMap();
                _automata.ProcessCavern(4);
                _automata.PrintMap();
            }
            if(GUI.Button(new Rect(110, 110, 100, 100), "Add iteration"))
            {
                _automata.ProcessCavern(1);
                _automata.Destroy();
                _automata.PrintMap();
            }
            if (GUI.Button(new Rect(220, 110, 50, 100), "R1"))
            {
                _automata.Destroy();
                _automata.PrintMap();
            }
            if (GUI.Button(new Rect(280, 110, 50, 100), "R2"))
            {
                _automata.Destroy();
                _automata.PrintMap();
            }
        }*/

        public void BuildDungeon(int NumberOfRooms)
        {
            FillGround();
            int rooms = (int)Mathf.Log(NumberOfRooms, 2f);
            Debug.Log(rooms);
            for (int i = 0; i < rooms; i++)
            {
                SpawnRoom(Random.Range(3, 10), Random.Range(3, 10), new Vector3(Random.Range(10, MapWidth), Random.Range(10, MapHeight)));
            }
            /*SpawnRoom(8, 3, new Vector3(60, 60));
            SpawnRoom(4, 4, new Vector3(40, 12));
            SpawnRoom(6, 2, new Vector3(30, 70));*/

            DrawSprites();
        }

        public void FillGround()
        {
            _grid = new Grid[MapHeight * MapWidth];

            //Set the grid with undisclosed tiles 
            for (int x = 0; x < MapWidth; x++)
            {
                for (int y = 0; y < MapHeight; y++)
                {
                    _grid[x + y * MapHeight] = new Grid(new Vector3(x, y, 0f));
                }
            }
        }
        private void DrawSprites()
        {
            _boardHolder = new GameObject("Board").transform;
            for (int x = -1; x < MapWidth + 1; x++)
            {
                for (int y = -1; y < MapHeight + 1; y++)
                {
                    GameObject toInstantiate = null;
                    if (x == -1 || x == MapWidth || y == -1 || y == MapHeight)
                        toInstantiate = WallSprites[Random.Range(0, WallSprites.Length)];
                    else if (_grid[x + y * MapHeight].status == Status.Revealed)
                    {
                        switch (_grid[x + y * MapHeight].tile)
                        {
                            case Tile.Floor:
                                toInstantiate = FloorSprites[Random.Range(0, FloorSprites.Length)];
                                break;
                            case Tile.Wall:
                                toInstantiate = WallSprites[Random.Range(0, WallSprites.Length)];
                                break;
                            case Tile.Door:
                                toInstantiate = FloorSprites[Random.Range(0, FloorSprites.Length)];
                                break;
                            case Tile.Ground:
                                toInstantiate = FloorSprites[Random.Range(0, FloorSprites.Length)];
                                break;
                        }
                    }
                    if (toInstantiate != null)
                    {
                        GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                        instance.transform.SetParent(_boardHolder);
                    }
                }
            }


            //Draw the rest of the walls
            for (int x = 0; x < MapWidth; x++)
            {
                for (int y = 0; y < MapHeight; y++)
                {
                    GameObject toInstantiate = null;
                    if (_grid[x + y * MapHeight].tile == Tile.Wall && _grid[x + y * MapHeight].status == Status.Undisclosed)
                    {
                        toInstantiate = WallSprites[Random.Range(0, WallSprites.Length)];
                    }
                    else if (_grid[x + y * MapHeight].status == Status.Undisclosed)
                    {
                        toInstantiate = BlackTile;
                    }
                    if (toInstantiate != null)
                    {
                        GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                        instance.transform.SetParent(_boardHolder);
                    }
                }
            }
        }

        public bool SpawnRoom(int width, int height, Vector2 pos, bool initialRoom = false)
        {
            int[] walls = new int[2 * (width) + 2 * (height)];
            int[] currentRoom = new int[width * height];

            int counter = 0;
            //Position => upper left pos
            if (!initialRoom)
            {
                for (int x = (int)pos.x; x <= (pos.x + width); x++)
                {
                    for (int y = (int)pos.y; y <= (pos.y + height); y++)
                    {
                        if (_grid[x + y * MapHeight].status != Status.Undisclosed)
                            return false;
                    }
                }
            }

            #region SetWallArray
            //Set the walls array
            for (int x = (int)(pos.x - 1); x <= (int)(pos.x + width); x++)
            {
                for (int y = (int)(pos.y - 1); y <= (int)(pos.y + height); y++)
                {
                    if (!(
                        ((x == pos.x - 1) && (y == pos.y - 1)) ||
                        ((x == pos.x - 1) && (y == pos.y + height)) ||
                        ((x == pos.x + width) && (y == pos.y - 1)) ||
                        ((x == pos.x + width) && (y == pos.y + height))
                        ))
                    {
                        if (x == (pos.x - 1) || x == (pos.x + width))
                        {
                            //Debug.Log(x + "  " + y);
                            _grid[x + y * MapHeight].tile = Tile.Wall;
                            walls[counter] = x + y * MapHeight;
                            counter++;
                        }
                        else if (y == (pos.y - 1) || y == (pos.y + height))
                        {
                            //Debug.Log(x + "  " + y);
                            _grid[x + y * MapHeight].tile = Tile.Wall;
                            walls[counter] = x + y * MapHeight;
                            counter++;
                        }
                        //Debug.Log(counter);
                    }
                }
            }
            #endregion

            counter = 0;
            for (int x = (int)pos.x; x < (pos.x + width); x++)
            {
                for (int y = (int)pos.y; y < (pos.y + height); y++)
                {
                    _grid[x + y * MapHeight].status = Status.Revealed;
                    _grid[x + y * MapHeight].tile = Tile.Floor;
                    currentRoom[counter] = x + y * MapHeight;
                    counter++;
                }
            }

            CreateDoorOnWall(walls, currentRoom);

            return true;
        }

        private void CreateDoorOnWall(int[] walls, int[] currentRoom)
        {
            bool isValidWall = true;
            //Select a random wall:
            int pos = walls[Random.Range(0, walls.Length)];
            ///Debug.Log(pos + " ( " + walls.Length + " )");

            //Scan for corridor
            //Get the direction of the corridor
            Direction dir = Direction.NULL;

            //Check for EAST
            if (_grid[pos - 1].status == Status.Revealed)
            {
                //Debug.Log("1");
                for (int i = 0; i < currentRoom.Length; i++)
                {
                    if (currentRoom[i] == (pos - 1))
                        dir = Direction.East;
                }
                if (dir == Direction.NULL)
                    isValidWall = false;
            }
            //WEST
            else if (_grid[pos + 1].status == Status.Revealed)
            {
                //Debug.Log("2");
                for (int i = 0; i < currentRoom.Length; i++)
                {
                    if (currentRoom[i] == (pos + 1))
                        dir = Direction.West;
                }
                if (dir == Direction.NULL)
                    isValidWall = false;
            }
            //SOUTH
            else if (_grid[pos + MapWidth].status == Status.Revealed)
            {
                //Debug.Log("3");
                for (int i = 0; i < currentRoom.Length; i++)
                {
                    if (currentRoom[i] == (pos + MapWidth))
                        dir = Direction.South;
                }
                if (dir == Direction.NULL)
                    isValidWall = false;
            }
            //NORTH
            else if (_grid[pos - MapWidth].status == Status.Revealed)
            {
                //Debug.Log("4");
                for (int i = 0; i < currentRoom.Length; i++)
                {
                    if (currentRoom[i] == (pos - MapWidth))
                        dir = Direction.North;
                }
                if (dir == Direction.NULL)
                    isValidWall = false;
            }

            //Debug.Log(dir);

            //Check if the corridor is correct

            bool isValidCorridor = true;
            switch (dir)
            {
                case Direction.East:
                    #region EAST
                    isValidWall = CheckPointInSameLine(pos, _minimumCorridorLength);
                    //Check if the corridor is empty
                    isValidCorridor = CheckCorridorEmpty(1, pos);

                    for (int i = 0; i < 2; i++)
                    {
                        for (int j = 0; j < _minimumCorridorLength; j++)
                        {
                            //
                            if (i == 0)
                            {
                                if (_grid[(pos - MapWidth) + j].status != Status.Undisclosed)
                                {
                                    isValidCorridor = false;
                                    break;
                                }
                            }
                            else
                            {
                                if (_grid[(pos + MapWidth) + j].status != Status.Undisclosed)
                                {
                                    isValidCorridor = false;
                                    break;
                                }
                            }
                        }
                    }

                    if (isValidCorridor)
                    {
                        _grid[pos].status = Status.Revealed;
                        _grid[pos].tile = Tile.Door;
                        for (int i = 0; i < 3; i++)
                        {
                            for (int j = 0; j < _minimumCorridorLength; j++)
                            {
                                switch (i)
                                {
                                    case 0:
                                        if (j != 0)
                                        {
                                            _grid[pos + j].status = Status.Revealed;
                                            _grid[pos + j].tile = Tile.Floor;
                                            
                                        }
                                        break;
                                    case 1:
                                        _grid[(pos - MapWidth) + j].status = Status.Revealed;
                                        _grid[(pos - MapWidth) + j].tile = Tile.Wall;
                                        break;
                                    case 2:
                                        _grid[(pos + MapWidth) + j].status = Status.Revealed;
                                        _grid[(pos + MapWidth) + j].tile = Tile.Wall;
                                        break;
                                }
                            }
                        }
                        return;
                    }
                    #endregion
                    break;
                case Direction.West:
                    #region WEST
                    isValidWall = CheckPointInSameLine(pos, -_minimumCorridorLength);
                    //Check if the corridor is empty
                    isValidCorridor = CheckCorridorEmpty(-1, pos);

                    for (int i = 0; i < 2; i++)
                    {
                        for (int j = 0; j < _minimumCorridorLength; j++)
                        {
                            //
                            if (i == 0)
                            {
                                if (_grid[(pos - MapWidth) - j].status != Status.Undisclosed)
                                {
                                    isValidCorridor = false;
                                    break;
                                }
                            }
                            else
                            {
                                if (_grid[(pos + MapWidth) - j].status != Status.Undisclosed)
                                {
                                    isValidCorridor = false;
                                    break;
                                }
                            }
                        }
                    }

                    if (isValidCorridor)
                    {
                        _grid[pos].status = Status.Revealed;
                        _grid[pos].tile = Tile.Door;
                        for (int i = 0; i < 3; i++)
                        {
                            for (int j = 0; j < _minimumCorridorLength; j++)
                            {
                                switch (i)
                                {
                                    case 0:
                                        if (j != 0)
                                        {
                                            _grid[pos - j].status = Status.Revealed;
                                            _grid[pos - j].tile = Tile.Floor;
                                        }
                                        break;
                                    case 1:
                                        _grid[(pos - MapWidth) - j].status = Status.Revealed;
                                        _grid[(pos - MapWidth) - j].tile = Tile.Wall;
                                        break;
                                    case 2:
                                        _grid[(pos + MapWidth) - j].status = Status.Revealed;
                                        _grid[(pos + MapWidth) - j].tile = Tile.Wall;
                                        break;
                                }
                            }
                        }
                        return;
                    }
                    #endregion
                    break;
                case Direction.North:
                    #region NORTH
                    //Debug.Log("=>north");
                    isValidWall = CheckPointInSameColumn(pos, _minimumCorridorLength);
                    //Check if the corridor is empty
                    isValidCorridor = CheckCorridorEmpty(MapWidth, pos);

                    #region Check corridor walls
                    for (int i = 0; i < 2; i++)
                    {
                        for (int j = 0; j < _minimumCorridorLength; j++)
                        {
                            //
                            if (i == 0)
                            {
                                if (_grid[(pos + 1) + (j * MapWidth)].status != Status.Undisclosed)
                                {
                                    isValidCorridor = false;
                                    break;
                                }
                            }
                            else
                            {
                                if (_grid[(pos - 1) + (j * MapWidth)].status != Status.Undisclosed)
                                {
                                    isValidCorridor = false;
                                    break;
                                }
                            }
                        }
                    }
                    #endregion

                    if (isValidCorridor)
                    {
                        _grid[pos].status = Status.Revealed;
                        _grid[pos].tile = Tile.Door;
                        for (int i = 0; i < 3; i++)
                        {
                            for (int j = 0; j < _minimumCorridorLength; j++)
                            {
                                switch (i)
                                {
                                    case 0:
                                        if (j != 0)
                                        {
                                            _grid[pos + MapWidth * j].status = Status.Revealed;
                                            _grid[pos + MapWidth * j].tile = Tile.Floor;
                                        }
                                        break;
                                    case 1:
                                        _grid[(pos - 1) + (j * MapWidth)].status = Status.Revealed;
                                        _grid[(pos - 1) + (j * MapWidth)].tile = Tile.Wall;
                                        break;
                                    case 2:
                                        _grid[(pos + 1) + (j * MapWidth)].status = Status.Revealed;
                                        _grid[(pos + 1) + (j * MapWidth)].tile = Tile.Wall;
                                        break;
                                }
                            }
                        }
                        return;
                    }
                    #endregion
                    break;
                case Direction.South:
                    #region SOUTH
                    isValidWall = CheckPointInSameColumn(pos, -_minimumCorridorLength);
                    //Debug.Log(isValidWall);
                    //Check if the corridor is empty
                    isValidCorridor = CheckCorridorEmpty(-MapWidth, pos);

                    #region Check corridor walls
                    for (int i = 0; i < 2; i++)
                    {
                        for (int j = 0; j < _minimumCorridorLength; j++)
                        {
                            //
                            if (i == 0)
                            {
                                if (_grid[(pos + 1) - (j * MapWidth)].status != Status.Undisclosed)
                                {
                                    isValidCorridor = false;
                                    break;
                                }
                            }
                            else
                            {
                                if (_grid[(pos - 1) - (j * MapWidth)].status != Status.Undisclosed)
                                {
                                    isValidCorridor = false;
                                    break;
                                }
                            }
                        }
                    }
                    #endregion

                    if (isValidCorridor)
                    {
                        Debug.Log("ok");
                        _grid[pos].status = Status.Revealed;
                        _grid[pos].tile = Tile.Door;
                        for (int i = 0; i < 3; i++)
                        {
                            for (int j = 0; j < _minimumCorridorLength; j++)
                            {
                                switch (i)
                                {
                                    case 0:
                                        if (j != 0)
                                        {
                                            _grid[pos - MapWidth * j].status = Status.Revealed;
                                            _grid[pos - MapWidth * j].tile = Tile.Floor;
                                        }
                                        break;
                                    case 1:
                                        _grid[(pos - 1) - (j * MapWidth)].status = Status.Revealed;
                                        _grid[(pos - 1) - (j * MapWidth)].tile = Tile.Wall;
                                        break;
                                    case 2:
                                        _grid[(pos + 1) - (j * MapWidth)].status = Status.Revealed;
                                        _grid[(pos + 1) - (j * MapWidth)].tile = Tile.Wall;
                                        break;
                                }
                            }
                        }
                        return;
                    }
                    #endregion
                    break;
            }
        }

        /// <summary>
        /// Scan the desired corridor location
        /// </summary>
        /// <param name="offset">Offset for the North/South</param>
        /// <param name="pos">Position of the door/entry</param>
        /// <returns>True: OK, corridor clear
        ///          False: Not clear 
        /// </returns>
        private bool CheckCorridorEmpty(int offset, int pos)
        {
            for (int i = 1; i <= _minimumCorridorLength; i++)
            {
                if ((pos + offset * i) >= _grid.Length)
                    return false;
                if (_grid[pos + offset * i].status != Status.Undisclosed)
                {
                    return false;
                }
            }
            return true;
        }

        private bool CheckPointInSameLine(int point, int offset)
        {
            if ((int)(point / MapWidth) == (int)((point + offset) / MapWidth))
                return true;
            else
                return false;
        }

        private bool CheckPointInSameColumn(int point, int offset)
        {
            if (point + (offset * MapHeight) < _grid.Length)
                return true;
            else
                return false;
        }

        private void SetRoomType(RoomType type, int pos)
        {
            _grid[pos].roomType = type;
        }
    }

    /*public enum TileFogState { Revealed, Active, Hiden };
    public enum Status { Revealed, Undisclosed };
    public enum Tile { Ground, Floor, Wall, Door, Full, Water, Grass };
    public enum RoomType { SquareRoom, RoundRoom, Corridor };
    public class Grid
    {
        public bool isWalkable { get; set; }
        public Status status { get; set; }
        public Tile tile { get; set; }
        public RoomType roomType { get; set; }
        public Vector3 position { get; set; }
        public byte BasicValue { get; set; }
        public byte ItemValue { get; set; }
        public MovingObject Entity { get; set; }
        public GameObject TileObject { get; set; }
        public GameObject TileItem { get; set; }
        public int posX { get; set; }
        public int posY { get; set; }
        public float Strength { get; set; }
        public string Coordinates
        {
            get
            {
                return string.Format("({0},{1})", posX, posY);
            }
        }
        public Fog FogScript
        { 
            get
            {
                return TileObject.transform.GetChild(0).GetComponent<Fog>();
            }
        }
        public Grid(Vector3 position, Tile tile = Tile.Ground)
        {
            this.position = position;
            this.status = Status.Undisclosed;
            this.tile = tile;
            this.isWalkable = false;
            this.BasicValue = 1;
        }

        public static Grid[] BasicValueToStatus(RawGrid[] rawGrid, int width, int height)
        {
            int length = rawGrid.Length;
            Grid[] changedGrid = new Grid[length];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    changedGrid[x + y * height] = new Grid(new Vector3(x, y, 0f));
                    changedGrid[x + y * height].BasicValue = rawGrid[x + y * height].BasicValue;
                    if (rawGrid[x + y * height].BasicValue == 0)
                    {
                        changedGrid[x + y * height].status = Status.Revealed;
                        changedGrid[x + y * height].tile = Tile.Floor;
                    }
                    else
                    {
                        changedGrid[x + y * height].status = Status.Undisclosed;
                        changedGrid[x + y * height].tile = Tile.Full;
                    }
                }
            }
            return changedGrid;
        }
    }
    public class RawGrid
    {
        public byte BasicValue { get; set; } //0=empty--1=filled
        public byte ItemValue { get; set; }
        public byte FloodValue { get; set; }
        public RawGrid()
        {
            BasicValue = 1;
            ItemValue = 0;
            FloodValue = 0;
        }
    }

    public enum Direction { North, South, West, East, NULL };

    public class DirectionClass
    {

        public static Direction GetRandomDirection()
        {
            switch (Random.Range(0, 4))
            {
                case 0:
                    return Direction.North;
                case 1:
                    return Direction.South;
                case 2:
                    return Direction.West;
                case 3:
                    return Direction.East;
                default:
                    return Direction.NULL;
            }
        }
    }*/
}