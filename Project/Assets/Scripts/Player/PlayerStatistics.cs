using UnityEngine;
using System.Collections;

public class PlayerStatistics
{
    public string Name;
    public float Health;
    public float Stamina;
    public float Mana;
    public float Armor;
    public float Experience;
    public float Drink;
    public float Eat;

    public float Strength;
    public float Agility;
    public float Charisma;


    public PlayerStatistics(string name, float health, float stamina, float mana, float armor, float experience, float drink, float eat, float strength, float agility, float charisma)
    {
        Name = name;
        Health = health;
        Stamina = stamina;
        Mana = mana;
        Armor = armor;
        Experience = experience;

        Drink = drink;
        Eat = eat;

        Strength = strength;
        Agility = agility;
        Charisma = charisma;
    }
}


public interface StatisticValues
{
    void Init();
}
