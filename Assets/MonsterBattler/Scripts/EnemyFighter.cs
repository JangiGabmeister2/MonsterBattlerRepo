using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFighter : FighterInfo
{
    public bool isDead()
    {
        if (health == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool isHealthLow()
    {
        if (health >= (maxHealth / 2))
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
