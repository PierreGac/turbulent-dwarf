using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections;

public interface DestructibleTile
{
    AudioSource Audio { get; set; }

    AudioClip[] Clips {get;set;}

    float HealthPoints { get; set; }

    float DamageCoef { get; set; }


    bool OnDamage(float damagePoints, int index);
}


public enum DestructibleType { MiningBlock, Tree, HugeCrystal, SimpleCrystal};

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(SpriteRenderer))]
public class MonoDestructibleTile : MonoBehaviour
{
    public DestructibleType Type;
    public DestructibleTile thisItem;

    public SpriteRenderer spriteRenderer { get; set; }

    public AudioSource Audio { get; set; }

    public AudioClip[] Clips;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Audio = GetComponent<AudioSource>();
        //spriteRenderer.enabled = false;
        GetItem();
    }

    public DestructibleTile GetItem()
    {
        switch (Type)
        {
            case DestructibleType.MiningBlock:
                thisItem = new MiningWall(Audio, Clips);
                break;
            case DestructibleType.Tree:
                thisItem = new MushroomTree(Audio, Clips);
                break;
            case DestructibleType.HugeCrystal:
                thisItem = new HugeCrystal(Audio, Clips);
                break;
            case DestructibleType.SimpleCrystal:
                thisItem = new SimpleCrystal(Audio, Clips);
                break;
        }
        return thisItem;
    }

    /*public static GameObject CreateGameObjectFromItem(DestructibleType item)
    {
        GameObject obj = new GameObject(item.Name);
        //Sprite renderer
        SpriteRenderer sprite = obj.AddComponent<SpriteRenderer>();
        sprite.sprite = item.InGameSprite;
        sprite.sortingLayerID = item.SortingLayer;

        //BoxCollider
        BoxCollider2D collider = obj.AddComponent<BoxCollider2D>();
        collider.size = sprite.sprite.bounds.size;

        MonoItem monoItem = obj.AddComponent<MonoItem>();
        monoItem.thisItem = (Item)item.Clone();
        monoItem.thisItem.gameObject = obj;
        monoItem.Type = item.Type;
        monoItem.isJustSpawned = true;
        monoItem.spriteRenderer.enabled = true;

        return obj;
    }*/
}
