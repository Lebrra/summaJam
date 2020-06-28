﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class knightBehavior : unitBehavior
{
    int peacefulSpaces = 1;

    protected override void act()
    {
        //move or attack
        spaceComponent nextSpace = mySpace.chooseSpace(playerUnit);
        if (!nextSpace)
        {
            //Debug.Log("Unable to move, wating for next turn.");
            return;
        }
        else if (nextSpace == mySpace)
        {
            // player takes damage
            if (playerUnit)
            {
                GameObject.Find("EnemyObject").GetComponent<playerManager>().health--;
                if (evolved) GameObject.Find("EnemyObject").GetComponent<playerManager>().health--;
            }
            else
            {
                GameObject.Find("PlayerObject").GetComponent<playerManager>().health--;
                if (evolved) GameObject.Find("PlayerObject").GetComponent<playerManager>().health--;
            }

            mySpace.removeUnit();
            Destroy(myHealthBar);
            Destroy(gameObject);
            return;
        }

        bool killed = false;
        if (nextSpace.unit)
        {
            //Debug.Log("Attacking!");
            peacefulSpaces = 1;

            if (nextSpace.unit.takeDamage(damage))
            {
                killed = true;
                nextSpace.removeUnit();
            }
        }
        if (!nextSpace.unit || killed)
        {
            //Debug.Log("Moving to a new space");
            if (!killed) peacefulSpaces++;

            mySpace.removeUnit();
            mySpace = nextSpace;
            nextSpace.addUnit(this);

            // move the dude
            GetComponent<Animator>().SetTrigger("Jump");
            location = nextSpace.transform.position;
            Invoke("setMove", 0.2F);
        }
    }

    protected override bool evolveCondition()
    {
        if (peacefulSpaces >= 5) return true;
        else return false;
    }
}
