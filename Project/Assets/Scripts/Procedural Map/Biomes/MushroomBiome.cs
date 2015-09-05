using UnityEngine;
using System;
using Random = System.Random;
using System.Collections.Generic;
using DungeonSpawner;

public class MushroomBiome : IBiome
{

    private int TreePercentage = 10;
    private int MushroomPercentage = 10;

    private float TreeStrength = 7f;
    private float BaseStrength = 7f;

    public Random rnd { get; set; }

    public MushroomBiome(Random rnd)
    {
        this.rnd = rnd;
    }

    public void SpawnBiome(int centerIndex)
    {
        BiomeCenterIndex = centerIndex;
        Scene._grid[centerIndex].Strength = rnd.Next(80, 101);
        Scene._grid[centerIndex].isWalkable = true;
        Scene._grid[centerIndex].BasicValue = BasicValues.MushroomBiome;
        Scene._grid[centerIndex].Biome = BiomeType.Mushroom;
        //Check if we have to spawn  tree or not:
        int result = rnd.Next(101);
        if (result <= TreePercentage && Scene._grid[centerIndex].Strength >= TreeStrength)
        {
            //Spawn a tree
            Scene._grid[centerIndex].isWalkable = false;
            Scene._grid[centerIndex].ItemValue = ItemValues.MushroomTree;
        }

        for (int pass = 0; pass < 6; pass++)
        {
            RecSpawnBiome(Scene._grid[centerIndex]);
        }

    }

    private float RecalculateStrength(Grid hex, List<Grid> surrHex)
    {
        //List<Grid> surrHex = Scene.GetSurroundingHexesBasicValue(hex);
        float originStrength = hex.Strength; //Store the strength of the previous hex
        for (int i = 0; i < surrHex.Count; i++)
        {
            if (surrHex[i].BasicValue == BasicValues.MushroomBiome || surrHex[i].BasicValue == BasicValues.MushroomWater)
            {
                if(surrHex[i].Strength < originStrength)
                {
                    surrHex[i].Strength = (surrHex[i].Strength + originStrength) / 2f;
                }
                else if(surrHex[i].Strength > originStrength)
                {
                    originStrength = (surrHex[i].Strength + originStrength) / 2f;
                }
            }
        }
        return originStrength;
    }

    private bool RecSpawnBiome(Grid hex)
    {
        if (hex.Strength <= 0)
            return false;


        float originStrength = hex.Strength; //Store the strength of the previous hex
        int result = 0;
        List<Grid> surrHex = Scene.GetSurroundingHexesBasicValue(hex);

        for (int i = 0; i < surrHex.Count; i++)
        {
            if (surrHex[i].BasicValue == 0 || surrHex[i].BasicValue == BasicValues.Water) //If raw tile
            {
                if (surrHex[i].BasicValue == BasicValues.Water)
                    surrHex[i].BasicValue = BasicValues.MushroomWater;
                //Debug.Log("New mushroom tile: " + _pass);
                surrHex[i].Strength = originStrength - BaseStrength;
                if (surrHex[i].Strength >= BaseStrength)
                {
                    //Spawn the ground tile:
                    surrHex[i].isWalkable = true;
                    surrHex[i].BasicValue = BasicValues.MushroomBiome;
                    surrHex[i].Biome = BiomeType.Mushroom;
                    //Check if we have to spawn  tree or not:
                    result = rnd.Next(101);
                    if (result <= TreePercentage && surrHex[i].Strength >= TreeStrength && surrHex[i].ItemValue == ItemValues.NULL)
                    {
                        //Spawn a tree
                        surrHex[i].isWalkable = false;
                        surrHex[i].ItemValue = ItemValues.MushroomTree;
                        surrHex[i].Strength -= TreeStrength;
                    }
                    else
                    {
                        //Random for spawning other items:

                        result = rnd.Next(101);
                        if (result <= MushroomPercentage)
                            surrHex[i].ItemValue = ItemValues.GreenMushroom;
                    }
                    if (surrHex[i].Strength < 0)
                        surrHex[i].Strength = 0;

                    RecSpawnBiome(surrHex[i]);
                }
            }
            else
            {
                if (surrHex[i].BasicValue == BasicValues.MushroomBiome || surrHex[i].BasicValue == BasicValues.MushroomWater)
                {
                    /*if(surrHex[i].Strength < originStrength)
                    {
                        float moy = (surrHex[i].Strength + originStrength) / 2f;
                        surrHex[i].Strength = moy;
                        hex.Strength = moy;
                        originStrength = moy;
                    }*/
                    originStrength = RecalculateStrength(hex, surrHex);
                }
            }
        }
        return true;
    }

    public BiomeType Type { get; set; }

    public int BiomeCenterIndex { get; set; }

    public void ExpandBiome()
    {
        throw new System.NotImplementedException();
    }
}
