using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DungeonSpawner;
using UnityEngine.UI;

//Player inherits from MovingObject, our base class for objects that can move, Enemy also inherits from this.
public class Player : MonoBehaviour// : MovingObject
{
    private Rigidbody2D _rb2D;

    public static int CurrentIndexPosition = 0;
    public static int CurrentPosX = 0;
    public static int CurrentPosY = 0;

    private bool _movementReady = true;

    public int x = 0;
    public int y = 0;

    public List<Grid> SurroundingHexes;

    public Grid[] Map;

    public static PlayerStatistics Stats;

    private bool _start = false;

    private Animator animator;                  //Used to store a reference to the Player's animator component.
    //Start overrides the Start function of MovingObject
    /*protected override void Start()
    {
        SurroundingHexes = new List<Grid>();
        //Get a component reference to the Player's animator component
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();

        //Call the Start function of the MovingObject base class.
        //base.Start();
    }*/
    public void InitPlayer(int index)
    {
        _rb2D = GetComponent<Rigidbody2D>();
        Stats = new PlayerStatistics("Gafda", 100, 100, 100, 10, 0, 50, 50, 2, 1, 1);
        CurrentIndexPosition = index;
        CurrentPosY = index / Scene.Height;
        CurrentPosX = index - CurrentPosY * Scene.Height;
        _start = true;

    }

    protected IEnumerator SmoothMovement(int desiredIndex)
    {
        if (Scene._grid[desiredIndex].isWalkable && Scene._grid[desiredIndex].ItemValue != ItemValues.MiningBlock)
        {
            CurrentIndexPosition = desiredIndex;
            CurrentPosY = CurrentIndexPosition / Scene.Height;
            CurrentPosX = CurrentIndexPosition - CurrentPosY * Scene.Height;
            Vector3 end = Scene._grid[desiredIndex].position;
            //Calculate the remaining distance to move based on the square magnitude of the difference between current position and end parameter. 
            //Square magnitude is used instead of magnitude because it's computationally cheaper.
            float sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            //While that distance is greater than a very small amount (Epsilon, almost zero):
            while (sqrRemainingDistance > float.Epsilon)
            {
                //Find a new position proportionally closer to the end, based on the moveTime
                Vector3 newPostion = Vector3.MoveTowards(_rb2D.position, end, 10.0f * Time.deltaTime);
                //Call MovePosition on attached Rigidbody2D and move it to the calculated position.
                _rb2D.MovePosition(newPostion);

                //Recalculate the remaining distance after moving.
                sqrRemainingDistance = (transform.position - end).sqrMagnitude;

                //Return and loop until sqrRemainingDistance is close enough to zero to end the function
                yield return null;
            }
            Fog.UpdateFog(CurrentIndexPosition, TileFogState.Active);
        }
        else
        {
            if (Scene._grid[desiredIndex].ItemValue == ItemValues.MiningBlock)
            {
                Scene._grid[desiredIndex].TileItem.GetComponent<MiningWall>().OnDamage(50, desiredIndex);
                yield return new WaitForSeconds(0.5f); //Mining time
            }
        }

        _movementReady = true;
    }

    private void Update()
    {
        if (!_start || !_movementReady)
            return;
        //Get inputs:
        int horizontal = (int)Input.GetAxisRaw("Horizontal");
        int vertical = -(int)Input.GetAxisRaw("Vertical");
        if (horizontal != 0 && vertical == 0)
        {
            if (CurrentIndexPosition + horizontal >= 0 && CurrentIndexPosition + horizontal < Scene.Size1D)
            {
                _movementReady = false;
                StartCoroutine(SmoothMovement(CurrentIndexPosition + horizontal));
            }
        }
        if(horizontal != 0 && vertical != 0)
        {
            int desiredPosition = CurrentIndexPosition + (vertical * Scene.Width);
            if (desiredPosition >= 0 && desiredPosition < Scene.Size1D)
            {
                _movementReady = false;
                //Get the next position according to the inputs
                if(CurrentPosY % 2 == 0)
                {
                    if (horizontal == -1) StartCoroutine(SmoothMovement(desiredPosition - 1));
                    else StartCoroutine(SmoothMovement(desiredPosition));
                }
                else
                {
                    if (horizontal == 1) StartCoroutine(SmoothMovement(desiredPosition + 1));
                    else StartCoroutine(SmoothMovement(desiredPosition));

                }
                return;
            }
        }
        return;

/*
        //If it's not the player's turn, exit the function.
        if (!GameManager.instance.playersTurn) return;

        int horizontal = 0;     //Used to store the horizontal move direction.
        int vertical = 0;       //Used to store the vertical move direction.
        

        //Get input from the input manager, round it to an integer and store in horizontal to set x axis move direction
        horizontal = (int)(Input.GetAxisRaw("Horizontal"));

        //Get input from the input manager, round it to an integer and store in vertical to set y axis move direction
        vertical = (int)(Input.GetAxisRaw("Vertical"));

        //Check if moving horizontally, if so set vertical to zero.
        if (horizontal != 0)
        {
            vertical = 0;
        }

        //Check if we have a non-zero value for horizontal or vertical
        if (horizontal != 0 || vertical != 0)
        {
            //Call AttemptMove passing in the generic parameter Wall, since that is what Player may interact with if they encounter one (by attacking it)
            //Pass in horizontal and vertical as parameters to specify the direction to move Player in.
            AttemptMove<Wall>(horizontal, vertical);
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.L))
                PerformAction("light");
        }*/
    }

    private void PerformAction(string type)
    {
        switch(type)
        {
            case "light":
                FogController.SwitchLight();
                break;
        }
        GameManager.instance.playersTurn = false;
    }

    //AttemptMove overrides the AttemptMove function in the base class MovingObject
    //AttemptMove takes a generic parameter T which for Player will be of the type Wall, it also takes integers for x and y direction to move in.
    /*protected override void AttemptMove<T>(int xDir, int yDir)
    {
        //Call the AttemptMove method of the base class, passing in the component T (in this case Wall) and x and y direction to move.
        base.AttemptMove<T>(xDir, yDir);

        //Hit allows us to reference the result of the Linecast done in Move.
        RaycastHit2D hit;
        Move(xDir, yDir, out hit);

        //Set the playersTurn boolean of GameManager to false now that players turn is over.
        GameManager.instance.playersTurn = false;
    }*/


    //OnCantMove overrides the abstract function OnCantMove in MovingObject.
    //It takes a generic parameter T which in the case of Player is a Wall which the player can attack and destroy.
    /*protected override void OnCantMove<T>(T component)
    {
        //Set hitWall to equal the component passed in as a parameter.
        Wall hitWall = component as Wall;

        //Call the DamageWall function of the Wall we are hitting.
        hitWall.DamageWall(wallDamage);

        //Set the attack trigger of the player's animation controller in order to play the player's attack animation.
        animator.SetTrigger("playerChop");
    }*/


    //OnTriggerEnter2D is sent when another object enters a trigger collider attached to this object (2D physics only).
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.GetComponent<MonoItem>().isJustSpawned)
        {
            Scene._grid[Player.CurrentIndexPosition].TileItem = null;
            Scene._grid[Player.CurrentIndexPosition].ItemValue = ItemValues.NULL;
            other.GetComponent<MonoItem>().thisItem.PickupItem();
        }
        else
            other.GetComponent<MonoItem>().isJustSpawned = false;
    }


    //Restart reloads the scene when called.
    private void Restart()
    {
        //Load the last scene loaded, in this case Main, the only scene in the game.
        Application.LoadLevel(Application.loadedLevel);
    }
}