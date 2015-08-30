using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DungeonSpawner
{

    public interface ICellularAutomata
    {
        int MapWidth { get; set; }
        int MapHeight { get; set; }
        int Size1D { get; }
        int Percentage { get; set; }
        RawGrid[] Map { get; }
        Grid[] CompleteGrid { get; }

        void RandomFillMap();
        void SetSize(int width, int height);
        void ProcessCavern(int iterations);

        void PrintMap();

        List<Grid> GetSurroundingHexes(Grid hex);

    }
}