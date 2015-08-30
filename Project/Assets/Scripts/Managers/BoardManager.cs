using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{
    [Serializable]
    public class Count
    {
        public int Minimum;
        public int Maximum;
        public Count(int min, int max)
        {
            Minimum = min;
            Maximum = max;
        }
    }


    public int Columns = 8;
    public int Rows = 8;
    public Count WallCount = new Count(5, 9);
    public Count FoodCount = new Count(1, 5);
    public GameObject Exit;
    public GameObject[] FloorTiles;
    public GameObject[] WallTiles;
    public GameObject[] FoodTiles;
    public GameObject[] EnemyTiles;
    public GameObject[] OuterWallTiles;

    private Transform _boardHolder;
    private List<Vector3> _gridPositions = new List<Vector3>();

    //Clears our list gridPositions and prepares it to generate a new board.
    private void InitialiseList()
    {
        //Clear our list gridPositions.
        _gridPositions.Clear();

        //Loop through x axis (columns).
        for (int x = 1; x < Columns - 1; x++)
        {
            //Within each column, loop through y axis (rows).
            for (int y = 1; y < Rows - 1; y++)
            {
                //At each index add a new Vector3 to our list with the x and y coordinates of that position.
                _gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    private void BoardSetup()
    {
        //Instantiate Board and set boardHolder to its transform.
        _boardHolder = new GameObject("Board").transform;

        //Loop along x axis, starting from -1 (to fill corner) with floor or outerwall edge tiles.
        for (int x = -1; x < Columns + 1; x++)
        {
            //Loop along y axis, starting from -1 to place floor or outerwall tiles.
            for (int y = -1; y < Rows + 1; y++)
            {
                //Choose a random tile from our array of floor tile prefabs and prepare to instantiate it.
                GameObject toInstantiate = FloorTiles[Random.Range(0, FloorTiles.Length)];

                //Check if we current position is at board edge, if so choose a random outer wall prefab from our array of outer wall tiles.
                if (x == -1 || x == Columns || y == -1 || y == Rows)
                    toInstantiate = OuterWallTiles[Random.Range(0, OuterWallTiles.Length)];

                //Instantiate the GameObject instance using the prefab chosen for toInstantiate at the Vector3 corresponding to current grid position in loop, cast it to GameObject.
                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                //Set the parent of our newly instantiated object instance to boardHolder, this is just organizational to avoid cluttering hierarchy.
                instance.transform.SetParent(_boardHolder);
            }
        }
    }


    //RandomPosition returns a random position from our list gridPositions.
    private Vector3 RandomPosition()
    {
        //Declare an integer randomIndex, set it's value to a random number between 0 and the count of items in our List gridPositions.
        int randomIndex = Random.Range(0, _gridPositions.Count);

        //Declare a variable of type Vector3 called randomPosition, set it's value to the entry at randomIndex from our List gridPositions.
        Vector3 randomPosition = _gridPositions[randomIndex];

        //Remove the entry at randomIndex from the list so that it can't be re-used.
        _gridPositions.RemoveAt(randomIndex);

        //Return the randomly selected Vector3 position.
        return randomPosition;
    }

    //LayoutObjectAtRandom accepts an array of game objects to choose from along with a minimum and maximum range for the number of objects to create.
    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
    {
        //Choose a random number of objects to instantiate within the minimum and maximum limits
        int objectCount = Random.Range(minimum, maximum + 1);

        //Instantiate objects until the randomly chosen limit objectCount is reached
        for (int i = 0; i < objectCount; i++)
        {
            //Choose a position for randomPosition by getting a random position from our list of available Vector3s stored in gridPosition
            Vector3 randomPosition = RandomPosition();

            //Choose a random tile from tileArray and assign it to tileChoice
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];

            //Instantiate tileChoice at the position returned by RandomPosition with no change in rotation
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }


    //SetupScene initializes our level and calls the previous functions to lay out the game board
    public void SetupScene(int level)
    {
        //Creates the outer walls and floor.
        BoardSetup();

        //Reset our list of gridpositions.
        InitialiseList();

        //Instantiate a random number of wall tiles based on minimum and maximum, at randomized positions.
        LayoutObjectAtRandom(WallTiles, WallCount.Minimum, WallCount.Maximum);

        //Instantiate a random number of food tiles based on minimum and maximum, at randomized positions.
        LayoutObjectAtRandom(FoodTiles, FoodCount.Minimum, FoodCount.Maximum);

        //Determine number of enemies based on current level number, based on a logarithmic progression
        int enemyCount = (int)Mathf.Log(level, 2f);

        //Instantiate a random number of enemies based on minimum and maximum, at randomized positions.
        LayoutObjectAtRandom(EnemyTiles, enemyCount, enemyCount);

        //Instantiate the exit tile in the upper right hand corner of our game board
        Instantiate(Exit, new Vector3(Columns - 1, Rows - 1, 0f), Quaternion.identity);
    }
}