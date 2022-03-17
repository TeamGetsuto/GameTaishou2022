using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class Player_Move_shinpo : MonoBehaviour
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
    [SerializeField] bool isJump = false;
    [SerializeField] bool isGround = false;
    bool isGravity = true;

    //�ړ�
    [Header("�ړ�")]
    [SerializeField] Vector2 moveDirection;
    [SerializeField] float walkSpeed;
    Vector2 lstick;

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

        //�W�����v���Ă��Ȃ��Ƃ����n�ʐڐG���Ă���Ƃ�
        if (!isJump && isGround)
        {
            //if (gamepad.buttonSouth.wasPressedThisFrame)
            //{
            //    isJump = true;
            //}

            if (keyboard.wKey.wasPressedThisFrame)
            {
                isJump = true;
            }
        }

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
        Jump();
        Move();
        //�͂߂�A�ǂ�����
        if (isGrab && isWall)
        {
            Grab();
        }
        else if (rigid.gravityScale == 0)
        {
            rigid.gravityScale = 1;
        }

        ////Grab();

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

    void Jump()
    {
        //�W�����v����
        if (isJump)
        {
            StartCoroutine("GravityWait");
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
        isJump = false;
        moveDirection.y = jumpForce;
        Debug.Log("�W�����v��");
        yield return new WaitForSeconds(waitTime);
        isGravity = true;
    }

    void Grab()
    {
            isGravity = false;
            isJump = false;
            rigid.gravityScale = 0;
            rigid.MovePosition(new Vector2(rigid.position.x, rigid.position.y));
    }
}

