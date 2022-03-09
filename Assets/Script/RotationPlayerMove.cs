using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RotationPlayerMove : MonoBehaviour
{
    Rigidbody2D rb;
    Gamepad gamepad;
    Collider2D col;
    Vector2 rightStick;

    [SerializeField] GameObject mainPlayer;
    Vector2 pos = Vector2.zero;

    public Vector2 defaultPos;
    public Player_Grab grab;
    bool isGrab;

    [Header("回転力")]
    [SerializeField] float upDownForce;
    [SerializeField] float leftRghitForce;
    [SerializeField] float motorPow;

    int? positionNow;
    int? positionBefore;
    [SerializeField] float jointWaitTime;
    public bool rotateTrigger = false;
    public bool jointTrriger = false;
    
    bool startUMovement = false;
    bool startRMovement = false;
    bool startLMovement = false;
    bool startDMovement = false;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();        
    }

    void Update()
    {
        // ゲームパッドが接続されていないとnullになる。
        if(gamepad == null)
            gamepad = Gamepad.current;

        rightStick = gamepad.rightStick.ReadValue();

        pos = mainPlayer.transform.position - transform.position;

        isGrab = grab.isGrab; 

        //R1ボタンで回転トリガーのON・OFF
        //------------------------------------------------
        if(gamepad.rightShoulder.isPressed == true)
        {
            col.isTrigger = true;
            rotateTrigger = true;
            jointTrriger = true;
            gamepad.SetMotorSpeeds(motorPow, motorPow);
        }
        else
        {
            col.isTrigger = false;
            rotateTrigger = false;
            StartCoroutine("JointWait");
            gamepad.SetMotorSpeeds(0.0f,0.0f);
        }
        //------------------------------------------------

        if(isGrab && gamepad.buttonEast.wasPressedThisFrame)
        {
            mainPlayer.transform.position = transform.position;
            col.isTrigger = false;
            jointTrriger = false;
        }

        if (!jointTrriger)
        {
            transform.position = new Vector2(mainPlayer.transform.position.x - 0.3f, mainPlayer.transform.position.y + 0.3f);
            col.isTrigger = true;
        }

        positionNow = StickPosition(rightStick);

        if (positionNow == positionBefore)
        {
            positionNow = null;
        }
        if (positionNow == null)
        {
            return;
        }

        PositionCheck();

        positionBefore = positionNow;
    }

    IEnumerator JointWait()
    {
        yield return new WaitForSeconds(jointWaitTime);
        if (isGrab)
        {
            yield return null;
        }
        else
        {
            jointTrriger = false;
        }
        
    }

    private void FixedUpdate()
    {
        if (rotateTrigger)
        {
            //コマンドに対する力の入力
            //-------------------------------------------------------------------
            if (startUMovement)
            {
                rb.AddForce(Vector2.up * upDownForce, ForceMode2D.Impulse);              
                startUMovement = false;
            }
            if (startLMovement)
            {
                rb.AddForce(Vector2.left * leftRghitForce, ForceMode2D.Impulse);                
                startLMovement = false;
            }
            if (startRMovement)
            {
                rb.AddForce(Vector2.right * leftRghitForce, ForceMode2D.Impulse);               
                startRMovement = false;
            }
            if (startDMovement)
            {
                rb.AddForce(Vector2.down * upDownForce, ForceMode2D.Impulse);
                startDMovement = false;
            }
            //-------------------------------------------------------------------
        }
        else
        {
            //離したときの処理
            //-----------------------------------
            rb.velocity *= 0.97f;
            if (rb.velocity.magnitude <= 0.01f)
            {
                rb.velocity = Vector2.zero;
                StartCoroutine("JointWait");
            }

            //-----------------------------------
        }
    }

    //スティックの位置からエリアを取得
    //-------------------------------------------------------------------------------
    int? StickPosition(Vector2 rightStick)
    {
        if (rightStick.x < -0.5f && -0.5f < rightStick.y && rightStick.y < 0.5f)
        {
            return 0;
        }
        else if (-0.5f < rightStick.x && rightStick.x < 0.5f && rightStick.y > 0.5)
        {
            return 1;
        }
        else if (rightStick.x > 0.5 && -0.5f < rightStick.y && rightStick.y < 0.5f)
        {
            return 2;
        }
        else if (-0.5f < rightStick.x && rightStick.x < 0.5f && rightStick.y < -0.5)
        {
            return 3;
        }
        return null;
    }
    //-------------------------------------------------------------------------------

    //コマンドの入力
    //-----------------------------------------------------------------
    private void PositionCheck()
    {
        if (positionNow == 1 && positionBefore == 0 && pos.x > 0)
        {
            startUMovement = true;
            positionNow = null;
        }
        if (positionNow == 0 && positionBefore == 3 && pos.y > 0)
        {
            startLMovement = true;
            positionNow = null;
        }
        if (positionNow == 2 && positionBefore == 1 && pos.y < 0)
        {
            startRMovement = true;
            positionNow = null;
        }
        if (positionNow == 3 && positionBefore == 2 && pos.x < 0)
        {
            startDMovement = true;
            positionNow = null;
        }
        if (positionNow == 1 && positionBefore == 2 && pos.x < 0)
        {
            startUMovement = true;
            positionNow = null;
        }
        if (positionNow == 0 && positionBefore == 1 && pos.y < 0)
        {
            startLMovement = true;
            positionNow = null;
        }
        if (positionNow == 2 && positionBefore == 3 && pos.y > 0)
        {
            startRMovement = true;
            positionNow = null;
        }
        if (positionNow == 3 && positionBefore == 0 && pos.x > 0)
        {
            startDMovement = true;
            positionNow = null;
        }
    }
    //-----------------------------------------------------------------
}

