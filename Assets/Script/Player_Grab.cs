using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Grab : MonoBehaviour
{
    Gamepad gamepad;

    private bool canGrab;
    private bool isCalcEnd;
    public bool isGrab;
    private Vector2 grabpos;
    public Ground_Check ground;
    [SerializeField] bool isGround;

    // Start is called before the first frame update
    void Start()
    {
        canGrab = false;
        isCalcEnd = false;
    }

    // Update is called once per frame
    void Update()
    {
        gamepad = Gamepad.current;

        if (!canGrab)
            return;
        
        if (gamepad.leftShoulder.isPressed)
        {
            CalcGrabPos();
            isGrab = true;
            gameObject.transform.position = grabpos;
        }
        else
        {
            isGrab = false;
            isCalcEnd = false;
        }
        Debug.Log(canGrab);
    }

    void CalcGrabPos()
    {
        if(isCalcEnd)
        {
            return;
        }

        grabpos = gameObject.transform.position;

        isCalcEnd = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Ground")
        canGrab = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Debug.LogWarning("call");
        canGrab = false;
    }
}
