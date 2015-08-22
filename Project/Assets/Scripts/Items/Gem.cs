using UnityEngine;
using System.Collections;

public class Gem : MonoBehaviour
{
    public float Value = 50f;

    public AudioClip Sound;

    public void GetGem()
    {
        GameManager.Money += Value;
        Destroy(gameObject);
    }
}
