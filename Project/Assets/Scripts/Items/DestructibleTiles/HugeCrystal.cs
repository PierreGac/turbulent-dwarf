using UnityEngine;
using System.Collections;

public class HugeCrystal : DestructibleTile
{
    public AudioClip[] Clips { get; set; }


    public float HealthPoints { get; set; }

    public float DamageCoef { get; set; }

    public AudioSource Audio { get; set; }
    public HugeCrystal(AudioSource source, AudioClip[] clips)
    {
        Audio = source;
        Clips = new AudioClip[clips.Length];
        clips.CopyTo(Clips, 0);

        HealthPoints = Scene.rnd.Next(120, 200);
        DamageCoef = 1.1f;
    }


    public bool OnDamage(float damagePoints, int index)
    {
        HealthPoints -= (damagePoints * DamageCoef);
        Audio.PlayOneShot(Clips[Random.Range(0, Clips.Length)]);
        if(HealthPoints <= 0)
        {
            Scene._grid[index].isWalkable = true;
            //Spawn bag:
            GameObject toInstantiate = MonoBehaviour.Instantiate(HexTileManager.instance.Containers[Random.Range(0, HexTileManager.instance.Containers.Length)], Scene._grid[index].position, Quaternion.identity) as GameObject;
            toInstantiate.transform.SetParent(Scene._grid[index].TileObject.transform); //Set the hex tile as parent
            toInstantiate.GetComponent<MonoItem>().spriteRenderer.enabled = true;

            //Create items:
            int maxItem = Random.Range(3, 6);
            Item[] content = new Item[maxItem];
            for (int i = 0; i < maxItem; i++)
            {
                //Select a random item
                // [GEMS][POWDER][RAWGEM][RAWCRYSTAL]
                switch (Random.Range(0, 4))
                {
                    //[GEMS]
                    case 0:
                        //Add a gem
                        //Get a random gem
                        switch (CrystalBiome.GemValues[Random.Range(0, CrystalBiome.GemValues.Length)])
                        {
                            case ItemValues.WhiteGem:
                                content[i] = new Gems(null, Gems.RedGemName, Gems.RedGemDescription, ItemValues.WhiteGem);
                                break;
                            case ItemValues.YellowGem:
                                content[i] = new Gems(null, Gems.YellowGemName, Gems.YellowGemDescription, ItemValues.YellowGem);
                                break;
                            case ItemValues.RedGem:
                                content[i] = new Gems(null, Gems.WhiteGemName, Gems.WhiteGemDescription, ItemValues.RedGem);
                                break;
                        }
                        content[i].Count = Random.Range(2, 6);
                        break;
                    case 1:
                        switch(CrystalPowder.PowderValues[Random.Range(0, CrystalPowder.PowderValues.Length)])
                        {
                            case ItemValues.PentanitePowder:
                                content[i] = new CrystalPowder(null, ItemValues.PentanitePowder);
                                content[i].Count = Random.Range(4, 11);
                                break;
                        }
                        break;
                    case 2:
                        content[i] = new RawGems(null);
                        content[i].Count = Random.Range(1, 4);
                        break;
                    case 3:
                        switch (CrystalRaw.RawValues[Random.Range(0, CrystalRaw.RawValues.Length)])
                        {
                            case ItemValues.PentaniteRaw:
                                content[i] = new CrystalRaw(null, ItemValues.PentaniteRaw);
                                content[i].Count = Random.Range(3, 8);
                                break;
                        }
                        break;
                }
            }

            (toInstantiate.GetComponent<MonoItem>().thisItem as ItemContainer).SetContent(content);

            Scene._grid[index].ItemValue = toInstantiate.GetComponent<MonoItem>().thisItem.ItemValue;
            return true;
        }
        return false;
    }
}
