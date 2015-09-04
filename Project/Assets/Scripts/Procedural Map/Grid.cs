using UnityEngine;
using System.Collections;

namespace DungeonSpawner
{
    public enum TileFogState { Revealed, Active, Hiden };
    public enum Status { Revealed, Undisclosed };
    public enum Tile { Ground, Floor, Wall, Door, Full, Water, Grass };
    public enum RoomType { SquareRoom, RoundRoom, Corridor };
    public class Grid
    {
        public bool isWalkable { get; set; }
        public Status status { get; set; }
        public Tile tile { get; set; }
        public RoomType roomType { get; set; }
        public BiomeType Biome { get; set; }
        public Vector3 position { get; set; }
        public byte BasicValue { get; set; }
        public byte ItemValue { get; set; }
        public MovingObject Entity { get; set; }
        public GameObject TileObject { get; set; }
        public GameObject TileItem { get; set; }
        public int posX { get; set; }
        public int posY { get; set; }
        public float Strength { get; set; }
        public byte FloodValue { get; set; }
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
            this.FloodValue = 0;
            this.Biome = BiomeType.Normal;
            this.Strength = Scene.rnd.Next(60, 101);
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
    }
}