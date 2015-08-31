using UnityEngine;
using System.Collections;
using DungeonSpawner;
public class Fog : MonoBehaviour
{
    public Sprite BlackFog;
    public Sprite Fog1;

    public static int VisionDistance = 8; //Maximum legth of the straight line stating at the player and facing to the current position
    public static int VisionAngle = 45; //Degres

    public TileFogState State;

    private Transform _thisTransform;
    private Transform _player;
    private SpriteRenderer _spriteRenderer;


    void Awake()
    {
        _thisTransform = transform;
        _spriteRenderer = _thisTransform.GetComponent<SpriteRenderer>();
        //_spriteRenderer.enabled = false; //Enable to disable the fog
    }


    public void UpdateFogTile(TileFogState state)
    {
        switch (state)
        {
            case TileFogState.Active:
                _spriteRenderer.sprite = null;
                break;
            case TileFogState.Revealed:
                _spriteRenderer.sprite = Fog1;
                break;
            case TileFogState.Hiden:
                _spriteRenderer.sprite = BlackFog;
                break;
        }
    }

    public static void SetPlayerVision(int playerIndex, Direction dir)
    {
        int x = playerIndex - (int)(playerIndex / Scene.Width) * Scene.Height;
        int y = playerIndex / Scene.Height;

        int cnt = 0;
        int YOffset = 0;
        //Set the straight line to Active:
        switch(dir)
        {
            #region EAST
            case Direction.East:
                //playerIndex + VisionLength
                if (x + VisionDistance >= Scene.Width)
                    cnt = Scene.Width - x;
                else
                    cnt = x + VisionDistance;
                for (int i = 0; i < cnt; i++ )
                    Scene._grid[i].FogScript.UpdateFogTile(TileFogState.Active);
                    break;
            #endregion
            #region NORTH
            case Direction.North:
                    //playerIndex + VisionLength
                    if (y - VisionDistance < 0)
                        cnt = 0;
                    else
                        cnt = x + VisionDistance;
                    for (int i = 0; i < cnt; i++)
                        Scene._grid[i].FogScript.UpdateFogTile(TileFogState.Active);
                break;
            #endregion
            #region SOUTH
            case Direction.South:
                break;
            #endregion
            #region WEST
            case Direction.West:
                //playerIndex - VisionLength
                if (x - VisionDistance < 0)
                    cnt = 0;
                else
                    cnt = x - VisionDistance;
                for (int i = 0; i < cnt; i++)
                    Scene._grid[i].FogScript.UpdateFogTile(TileFogState.Active);
                break;
            #endregion
        }


    }

    public static void UpdateFogPlayerSpawn(int index)
    {
        Scene._grid[index].FogScript.UpdateFogTile(TileFogState.Active);
        Scene._grid[index].FogScript.State = TileFogState.Active;
    }

    // Update is called once per frame
    public static void UpdateFog(int index, TileFogState state)
    {
        int x = index - (int)(index / Scene.Width) * Scene.Width;
        int y = index / Scene.Height;
        int startX;
        int startY;
        int endX;
        int endY;

        int iX;
        int iY;
        startX = x - 1;
        startY = y - 1;
        endX = x + 1;
        endY = y + 1;

        iX = startX;
        iY = startY;

        /*for (iX = startX; iX <= endX; iX++)
        {
            for (iY = startY; iY <= endY; iY++)
            {
                if ((iX + iY * Scene.Height) < Scene._grid.Length && Scene._grid[iX + iY * Scene.Height].tile != Tile.Full)
                {
                    if (Scene._grid[iX + iY * Scene.Height].FogScript.State != state)
                    {
                        Scene._grid[iX + iY * Scene.Height].FogScript.UpdateFogTile(state);
                        Scene._grid[iX + iY * Scene.Height].FogScript.State = state;
                    }
                }
            }
        }*/
        foreach(Grid grid in Scene.GetSurroundingHexes(Scene._grid[index]))
        {
            if (grid.FogScript != null)
            {
                grid.FogScript.UpdateFogTile(state);
                grid.FogScript.State = state;
            }
        }

        foreach (Grid grid in Scene.GetSurroundingHexes(Scene._grid[index], 2))
        {
            if (grid.FogScript != null)
            {
                grid.FogScript.UpdateFogTile(TileFogState.Revealed);
                grid.FogScript.State = TileFogState.Revealed;
            }
        }

        return;

        startX = x - 2;
        startY = y - 2;
        endX = x + 2;
        endY = y + 2;

        iX = startX;
        iY = startY;

        for (iX = startX; iX <= endX; iX++)
        {
            for (iY = startY; iY <= endY; iY++)
            {
                if (iX == startX || iX == endX || iY == startY || iY == endY)
                {
                    if ((iX + iY * Scene.Height) < Scene._grid.Length && Scene._grid[iX + iY * Scene.Height].tile != Tile.Full)
                    {
                        //if (Scene._grid[iX + iY * Scene.Height].FogScript.State != TileFogState.Revealed)
                        //{
                            Scene._grid[iX + iY * Scene.Height].FogScript.UpdateFogTile(TileFogState.Revealed);
                            Scene._grid[iX + iY * Scene.Height].FogScript.State = TileFogState.Revealed;
                        //}
                    }
                }
            }
        }
    }
}
