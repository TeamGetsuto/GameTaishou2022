using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class Player_Move : MonoBehaviour
{
    //���͔}��
    Gamepad gamepad;
    Keyboard keyboard;

    //�v���C���[
    Rigidbody2D rigid;

    //�W�����v
    [Header("�W�����v")]
    public Ground_Check ground;
    [SerializeField] float jumpForce;
    [SerializeField] float waitTime;
    [SerializeField] bool isGround = false;
    bool isGravity = true;

    //�ړ�
    [Header("�ړ�")]
    [SerializeField] Vector2 moveDirection;
    [SerializeField] float walkSpeed;
    Vector2 lstick;
    //�ړ���on/off
    [SerializeField] bool moveSwitchOn = false;

    //�͂�
    [Header("�͂�")]
    public Wall_Check wall;
    [SerializeField] bool isGrab = false;
    [SerializeField] bool isWall = false;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Gamepad.current != null)
        //{
        //    gamepad = Gamepad.current;
        //}

        if (Keyboard.current != null)
        {
            keyboard = Keyboard.current;
        }

        //lstick = gamepad.leftStick.ReadValue();

        //�n�ʂ̐ڐG����
        isGround = ground.IsGround();

        //�ǔ���
        isWall = wall.IsWall();


        //���ړ�����
        //if (lstick.x < 0)
        //{
        //    moveDirection.x = -1;
        //}
        //else if (lstick.x > 0)
        //{
        //    moveDirection.x = 1;
        //}
        //else
        //{
        //    moveDirection.x = 0;
        //}

        if (moveSwitchOn)
        {
            if (keyboard.aKey.isPressed)
            {
                moveDirection.x = -1;
            }
            else if (keyboard.dKey.isPressed)
            {
                moveDirection.x = 1;
            }
            else
            {
                moveDirection.x = 0;
            }
        }

        //�ړ�on/off
        if (keyboard.oKey.isPressed)
        {
            moveSwitchOn = true;
        }
        if (keyboard.pKey.isPressed)
        {
            moveSwitchOn = false;
        }

        //if (gamepad.leftShoulder.isPressed)
        //{
        //    moveSwitchOn = true;
        //}
        //if (gamepad.leftTrigger.isPressed)
        //{
        //    moveSwitchOn = false;
        //}

        //�͂ޓ���
        if (!isGrab)
        {
            //�͂ރ{�^��
            if (keyboard.eKey.isPressed)
            {
                isGrab = true;
            }
        }
        else
        {
            if (keyboard.eKey.wasReleasedThisFrame)
            {
                isGrab = false;
            }
        }
    }

    void FixedUpdate()
    {
        if (moveSwitchOn)
        {
            Move();
        }
        //�͂߂�A�ǂ�����
        if (isGrab && isWall)
        {
            Grab();
        }
        else if (rigid.gravityScale == 0)
        {
            rigid.gravityScale = 1;
        }

        if (isGravity)
        {
            Gravity();
        }

        //�W�����v���Ă��Ȃ��Ƃ��n�ʂ𗣂ꂽ�痎����
        if (!isGround)
        {
            isGravity = true;
        }
    }

    void Move()
    {
            rigid.MovePosition(new Vector2
                              (rigid.position.x + moveDirection.x * walkSpeed * Time.deltaTime,
                               rigid.position.y + moveDirection.y * Time.deltaTime));
    }

    private void Gravity()
    {
        if (!isGround)
        {
            moveDirection.y -= 0.3f;
        }
        else
        {
            moveDirection.y = 0;
            isGravity = false;
        }
    }

    IEnumerator GravityWait()
    {
        moveDirection.y = jumpForce;
        Debug.Log("�W�����v��");
        yield return new WaitForSeconds(waitTime);
        isGravity = true;
    }

    void Grab()
    {
            isGravity = false;
            rigid.gravityScale = 0;
            rigid.MovePosition(new Vector2(rigid.position.x, rigid.position.y));
    }
}

