using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceManager : MonoBehaviour
{

    public int RollDice(int sides)
    {
        return Random.Range(0, sides);
    }

}
