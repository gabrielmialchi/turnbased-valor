using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    public int RollDice(int sides)
    {
        return Random.Range(1, sides);
    }

    public int VandalDamage(int sides, int numberOfDice)
    {
        int total = 0;
        for (int i = 0; i < numberOfDice; i++)
        {
            total += RollDice(sides);
        }
        return total;
    }

    public int ClassicDamage(int sides, int numberOfDice)
    {
        int total = 0;
        for (int i = 0; i < numberOfDice; i++)
        {
            total += RollDice(sides);
        }
        return total;
    }
}
