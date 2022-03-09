using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall_Check : MonoBehaviour
{
    bool isWall = false;
    bool isWallEnter, isWallStay, isWallExit;

    public bool IsWall()
    {
        if (isWallEnter || isWallStay)
        {
            isWall = true;
        }
        else if (isWallExit)
        {
            isWall = false;
        }

        isWallEnter = false;
        isWallStay = false;
        isWallExit = false;

        return isWall;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Wall")
        {
            isWallEnter = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Wall")
        {
            isWallStay = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Wall")
        {
            isWallExit = true;
        }
    }

}
