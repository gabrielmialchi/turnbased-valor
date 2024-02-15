using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    private int attackRoll;

    public int RollDice(int sides)
    {
        int singleRoll = Random.Range(1, sides);
        return singleRoll;
    }

    public int RollVariousDices(int sides, int numberOfDice)
    {
        int total = 0;
        for (int i = 0; i < numberOfDice; i++)
        {
            total += RollDice(sides);
        }
        return total;
    }

    public int VandalDamage()
    {
        attackRoll = RollDice(20);
        Debug.Log("Attack Roll: " + attackRoll);
        
        int vandalTotalDamage;

        if (attackRoll == 1)
        {
            //Critical Miss
            Debug.Log("Critical Miss");
            vandalTotalDamage = RollVariousDices(10, 3);
            Debug.Log("Vandal Damage = " + vandalTotalDamage);
        }
        else if (attackRoll < 10)
        {
            //Miss
            Debug.Log("Miss");
            vandalTotalDamage = 0;
            Debug.Log("Vandal Damage = " + vandalTotalDamage);

        }
        else if (attackRoll == 20)
        {
            //Critical Hit
            Debug.Log("CRITICAL!!!");
            vandalTotalDamage = 30;
            Debug.Log("Vandal Damage = " + vandalTotalDamage);

        }
        else
        {
            //Normal Hit
            Debug.Log("HIT");
            vandalTotalDamage = RollVariousDices(10, 3);
            Debug.Log("Vandal Damage = " + vandalTotalDamage);
        }

        return vandalTotalDamage;
    }
}
