using UnityEngine;
using System;
using Random = System.Random;
using System.Collections.Generic;
using DungeonSpawner;

public class CrystalBiome : IBiome
{

    private int HugeCrystalsPercentage = 10;
    private int GemPercentage = 10;

    private float HugeCrystalStrength = 7f;
    private float BaseStrength = 7f;

    private byte[] GemValues = { ItemValues.RedGem, ItemValues.WhiteGem, ItemValues.YellowGem };

    public Random rnd { get; set; }

    public CrystalBiome(Random rnd)
    {
        this.rnd = rnd;
    }

    public void SpawnBiome(int centerIndex)
    {
        BiomeCenterIndex = centerIndex;
        Scene._grid[centerIndex].Strength = rnd.Next(50, 80);
        Scene._grid[centerIndex].isWalkable = true;
        Scene._grid[centerIndex].BasicValue = BasicValues.CrystalBiome;
        Scene._grid[centerIndex].Biome = BiomeType.Crystal;
        //Check if we have to spawn  tree or not:
        int result = rnd.Next(101);
        if (result <= HugeCrystalsPercentage && Scene._grid[centerIndex].Strength >= HugeCrystalStrength)
        {
            //Spawn a tree
            Scene._grid[centerIndex].isWalkable = false;
            Scene._grid[centerIndex].ItemValue = ItemValues.HugeCrystal;
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
            if (surrHex[i].BasicValue == BasicValues.CrystalBiome)
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
            if (surrHex[i].BasicValue == BasicValues.Ground) //If raw tile
            {
                //Debug.Log("New mushroom tile: " + _pass);
                surrHex[i].Strength = originStrength - BaseStrength;
                if (surrHex[i].Strength >= BaseStrength)
                {
                    //Spawn the ground tile:
                    surrHex[i].isWalkable = true;
                    surrHex[i].BasicValue = BasicValues.CrystalBiome;
                    surrHex[i].Biome = BiomeType.Crystal;
                    //Check if we have to spawn  tree or not:
                    result = rnd.Next(101);
                    if (result <= HugeCrystalsPercentage && surrHex[i].Strength >= HugeCrystalStrength && surrHex[i].ItemValue == ItemValues.NULL)
                    {
                        //Spawn a tree
                        surrHex[i].isWalkable = false;
                        surrHex[i].ItemValue = ItemValues.HugeCrystal;
                        surrHex[i].Strength -= HugeCrystalStrength;
                    }
                    else
                    {
                        //Random for spawning other items:

                        result = rnd.Next(101);
                        if (result <= GemPercentage)
                            surrHex[i].ItemValue = GemValues[rnd.Next(GemValues.Length)];
                    }
                    if (surrHex[i].Strength < 0)
                        surrHex[i].Strength = 0;

                    RecSpawnBiome(surrHex[i]);
                }
            }
            else
            {
                if (surrHex[i].BasicValue == BasicValues.CrystalBiome)
                    originStrength = RecalculateStrength(hex, surrHex);
            }
        }
        return true;
    }

    public BiomeType Type{ get; set; }

    public int BiomeCenterIndex { get; set; }

    public void ExpandBiome()
    {
        throw new System.NotImplementedException();
    }
}
