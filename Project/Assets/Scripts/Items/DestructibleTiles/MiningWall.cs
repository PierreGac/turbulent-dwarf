using UnityEngine;
using System.Collections;

public class MiningWall : MonoBehaviour, DestructibleTile
{
    public AudioClip[] MiningSounds;


    public float HealthPoints { get; set; }

    public float DamageCoef { get; set; }

    public AudioSource Audio { get; set; }
    void Awake()
    {
        Audio = GetComponent<AudioSource>();
        HealthPoints = Scene.rnd.Next(100, 151);
        DamageCoef = 1.0f;
    }


    public bool OnDamage(float damagePoints, int index)
    {
        HealthPoints -= (damagePoints * DamageCoef);
        Audio.PlayOneShot(MiningSounds[Random.Range(0, MiningSounds.Length)]);
        if(HealthPoints <= 0)
        {
            //Debug.Log("Destroyed");
            Scene._grid[index].isWalkable = true;
            Scene._grid[index].ItemValue = 0;
            //Spawn boulder:
            GameObject toInstantiate = Instantiate(HexTileManager.instance.Boulders[Random.Range(0, HexTileManager.instance.Boulders.Length)], Scene._grid[index].position, Quaternion.identity) as GameObject;
            toInstantiate.transform.SetParent(Scene._grid[index].TileObject.transform); //Set the hex tile as parent
            toInstantiate.GetComponent<MonoItem>().spriteRenderer.enabled = true;
            return true;
        }
        return false;
    }
}
