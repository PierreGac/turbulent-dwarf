using UnityEngine;
using System.Collections;


public enum BiomeType { Normal, Mushroom};

public interface IBiome
{
    /// <summary>
    /// Spawn the biome at the starting point specified
    /// </summary>
    /// <param name="centerIndex"></param>
    void SpawnBiome(int centerIndex);

    /// <summary>
    /// The type of biome
    /// </summary>
    BiomeType Type { get; set; }

    /// <summary>
    /// The index of the center of the biom [origin point]
    /// </summary>
    int BiomeCenterIndex { get; set; }

    /// <summary>
    /// Expand/Reduce the biome
    /// </summary>
    void ExpandBiome();

}
