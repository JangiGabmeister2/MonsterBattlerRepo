using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFighter : FighterInfo
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
}
