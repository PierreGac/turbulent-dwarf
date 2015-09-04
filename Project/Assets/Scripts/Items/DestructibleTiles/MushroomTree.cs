using UnityEngine;
using System.Collections;

public class MushroomTree : MonoBehaviour, DestructibleTile
{
    public AudioClip[] MushroomTreeSounds;


    public float HealthPoints { get; set; }

    public float DamageCoef { get; set; }

    public AudioSource Audio { get; set; }
    void Awake()
    {
        Audio = gameObject.AddComponent<AudioSource>();
        Audio.playOnAwake = false;
        Audio.loop = false;
        HealthPoints = Scene.rnd.Next(50, 101);
        DamageCoef = 1.0f;
    }


    public bool OnDamage(float damagePoints, int index)
    {
        HealthPoints -= (damagePoints * DamageCoef);
        Audio.PlayOneShot(MushroomTreeSounds[Random.Range(0, MushroomTreeSounds.Length)]);
        if(HealthPoints <= 0)
        {
            //Debug.Log("Destroyed");
            Scene._grid[index].isWalkable = true;
            //Spawn boulder:
            GameObject toInstantiate = Instantiate(MushroomBiomeTileManager.LogTiles[Random.Range(0, MushroomBiomeTileManager.LogTiles.Length)], Scene._grid[index].position, Quaternion.identity) as GameObject;
            toInstantiate.transform.SetParent(Scene._grid[index].TileObject.transform); //Set the hex tile as parent
            toInstantiate.GetComponent<MonoItem>().spriteRenderer.enabled = true;
            toInstantiate.GetComponent<MonoItem>().thisItem.Count = Random.Range(2, 6);
            Scene._grid[index].ItemValue = ItemValues.MushroomLog;
            return true;
        }
        return false;
    }
}
