using UnityEngine;
using System.Collections;

public class MushroomTree : DestructibleTile
{
    public AudioClip[] Clips { get; set; }


    public float HealthPoints { get; set; }

    public float DamageCoef { get; set; }

    public AudioSource Audio { get; set; }
    public MushroomTree(AudioSource source, AudioClip[] clips)
    {
        Audio = source;
        Clips = new AudioClip[clips.Length];
        clips.CopyTo(Clips, 0);

        HealthPoints = Scene.rnd.Next(50, 101);
        DamageCoef = 1.0f;
    }


    public bool OnDamage(float damagePoints, int index)
    {
        HealthPoints -= (damagePoints * DamageCoef);
        Audio.PlayOneShot(Clips[Random.Range(0, Clips.Length)]);
        if(HealthPoints <= 0)
        {
            Scene._grid[index].isWalkable = true;
            //Spawn boulder:
            GameObject toInstantiate = MonoBehaviour.Instantiate(MushroomBiomeTileManager.LogTiles[Random.Range(0, MushroomBiomeTileManager.LogTiles.Length)], Scene._grid[index].position, Quaternion.identity) as GameObject;
            toInstantiate.transform.SetParent(Scene._grid[index].TileObject.transform); //Set the hex tile as parent
            toInstantiate.GetComponent<MonoItem>().spriteRenderer.enabled = true;
            toInstantiate.GetComponent<MonoItem>().thisItem.Count = Random.Range(2, 6);
            Scene._grid[index].ItemValue = ItemValues.MushroomLog;
            return true;
        }
        return false;
    }
}
