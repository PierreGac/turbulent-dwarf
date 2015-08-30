using UnityEngine;
using System.Collections;

public class MiningWall : MonoBehaviour
{
    public AudioClip[] MiningSounds;


    public float HealthPoints = 150;

    public float DamageCoef = 1.0f;

    private AudioSource _audioSource;
    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }


    public void OnDamage(float damagePoints, int index)
    {
        HealthPoints -= (damagePoints * DamageCoef);
        _audioSource.PlayOneShot(MiningSounds[Random.Range(0, MiningSounds.Length)]);
        if(HealthPoints <= 0)
        {
            Debug.Log("Destroyed");
            Scene._grid[index].TileItem = null;
            Scene._grid[index].isWalkable = true;
            Scene._grid[index].ItemValue = 0;
            Destroy(gameObject);
        }
    }
}
