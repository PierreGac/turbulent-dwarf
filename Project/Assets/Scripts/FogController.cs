
using UnityEngine;
using System.Collections;

public class FogController : MonoBehaviour
{
    public float dist01 = 5f;
    public float dist02 = 3f;


    private bool _lightMultiplier = true;
    private float _lightConst = 4f;

    public static FogController instance = null;

    void Awake()
    {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }


    public static void SwitchLight()
    {
        if (instance._lightMultiplier)
        {
            instance.dist01 += instance._lightConst;
            instance.dist02 += instance._lightConst;
            instance._lightMultiplier = false;
        }
        else
        {
            instance.dist01 -= instance._lightConst;
            instance.dist02 -= instance._lightConst;
            instance._lightMultiplier = true;
        }
    }
}
