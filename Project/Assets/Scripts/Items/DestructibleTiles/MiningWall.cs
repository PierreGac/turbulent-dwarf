using UnityEngine;
using System.Collections;

public class MiningWall : DestructibleTile
{
    public AudioClip[] Clips { get; set; }


    public float HealthPoints { get; set; }

    public float DamageCoef { get; set; }

    public AudioSource Audio { get; set; }
    public MiningWall(AudioSource source, AudioClip[] clips)
    {
        Audio = source;
        Clips = new AudioClip[clips.Length];
        clips.CopyTo(Clips,0);
        HealthPoints = Scene.rnd.Next(100, 151);
        DamageCoef = 1.0f;
    }


    public bool OnDamage(float damagePoints, int index)
    {
        HealthPoints -= (damagePoints * DamageCoef);
        Audio.PlayOneShot(Clips[Random.Range(0, Clips.Length)]);
        if(HealthPoints <= 0)
        {
            Scene._grid[index].isWalkable = true;
            Scene._grid[index].ItemValue = 0;
            //Spawn boulder:
            GameObject toInstantiate = MonoBehaviour.Instantiate(HexTileManager.instance.Boulders[Random.Range(0, HexTileManager.instance.Boulders.Length)], Scene._grid[index].position, Quaternion.identity) as GameObject;
            toInstantiate.transform.SetParent(Scene._grid[index].TileObject.transform); //Set the hex tile as parent
            toInstantiate.GetComponent<MonoItem>().spriteRenderer.enabled = true;
            return true;
        }
        return false;
    }
}
