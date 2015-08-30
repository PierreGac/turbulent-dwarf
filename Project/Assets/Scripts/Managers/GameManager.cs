using UnityEngine;
using System.Collections;
using System.Collections.Generic;       //Allows us to use Lists. 
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public float levelStartDelay = 2f;
    public float turnDelay = .1f;
    public static GameManager instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.
    private BoardManager boardScript;                       //Store a reference to our BoardManager which will set up the level.
    private int level = 1;                                  //Current level number, expressed in game as "Day 1".
    public int playerFoodPoints = 100;
    [HideInInspector] public bool playersTurn = true;
    private List<Enemy> enemies;
    private bool enemiesMoving;

    private static float _money;
    public static float Money
    { 
        get
        {
            return _money;
        }
        set
        {
            _money = value;
            UserInterface.MoneyText.text = string.Format("Money: {0}", value);
        }
    }

    private Text levelText;
    private GameObject levelImage;
    private bool doingSetup;

    //Awake is always called before any Start functions
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

        enemies = new List<Enemy>();
        _money = 0;
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
        if (playersTurn || enemiesMoving || doingSetup)
            return;
        StartCoroutine(MoveEnemies());
    }

    public void AddEnemyToList(Enemy script)
    {
        enemies.Add(script);
    }

    IEnumerator MoveEnemies()
    {
        enemiesMoving = true;
        yield return new WaitForSeconds(turnDelay);
        if(enemies.Count == 0)
        {
            yield return new WaitForSeconds(turnDelay);
        }

        for(int i = 0; i < enemies.Count; i++)
        {
            enemies[i].MoveEnemy();
            yield return new WaitForSeconds(enemies[i].moveTime);
        }

        playersTurn = true;
        enemiesMoving = false;
    }
}