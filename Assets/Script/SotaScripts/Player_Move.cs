using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Move : MonoBehaviour
{
    //���͔}��
    Gamepad gamepad;

    //�v���C���[
    Rigidbody2D rigid;

    //�W�����v
    [Header("jump")]
    [SerializeField] float jumpForce;
    [SerializeField] bool isJump = false;
    [SerializeField] bool isGound = false;
    [SerializeField] float waitTime;
    public Ground_Check ground;
    bool isGravity = true;

    [Header("�ڒn����")]
    [SerializeField] float offsetX;
    [SerializeField] float offsetY;
    [SerializeField] float rayDist;
    [SerializeField] LayerMask layer;

    //�ړ�
    [SerializeField] float walkSpeed;
    [SerializeField] Vector2 moveDirection;

    [Header("�T�u�v���C��")]
    [SerializeField] GameObject subPlayer;
    [SerializeField] Rigidbody2D subrb;
    [SerializeField] float pow;
    [SerializeField] float jointWaitTime;
    Vector2 defaultePos;

    DistanceJoint2D dis;
    public bool jointTrriger = false;
    
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        dis = GetComponent<DistanceJoint2D>();
        defaultePos = subPlayer.GetComponent<RotationPlayerMove>().defaultPos;
    }

    // Update is called once per frame
    void Update()
    {
        if (Gamepad.current != null)
        {
            gamepad = Gamepad.current;
        }

        //�n�ʂ̐ڐG����
        isGound = ground.IsGround();
        //�W�����v���Ă��Ȃ��Ƃ����n�ʐڐG���Ă���Ƃ�
        if (!isJump && isGound)
        {
            if (gamepad.buttonNorth.wasPressedThisFrame)
            {
                Debug.Log("�W�����v��");
                isJump = true;
            }
        }
        
        //�ړ�
        //---------------------------------------------------------
        if (gamepad.leftStick.ReadValue().x != 0)
        {
            moveDirection.x = gamepad.leftStick.ReadValue().x;
        }
        else
        { 
            moveDirection.x = 0;
        }
        //---------------------------------------------------------

        DistanceJoint();

        //��]�̃g���K�[
        //---------------------------------------------------------
        if (gamepad.rightShoulder.isPressed)
        {
            rigid.constraints = RigidbodyConstraints2D.FreezeAll;
            moveDirection.y = 0;
            jointTrriger = true;
        }
        else if(gamepad.rightShoulder.wasReleasedThisFrame)
        {
            jointTrriger = false;
            Vector2 fryPow = subPlayer.transform.position - transform.position;
            subrb.AddForce(fryPow * pow, ForceMode2D.Impulse);
            StartCoroutine("JointWait");
        }
        else
        {
            rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        //---------------------------------------------------------

    }

    void FixedUpdate()
    {
        Jump();
        Move();
        if (isGravity || !isGound) 
        {
            Gravity();
        }

        GroundChecker();
    }

    void Jump()
    {
        //�W�����v����
        if(isJump)
        {
            StartCoroutine("GravityWait");
        }
    }

    void Move()
    {
        rigid.MovePosition(new Vector2(rigid.position.x + moveDirection.x * walkSpeed * Time.deltaTime,rigid.position.y + moveDirection.y * Time.deltaTime));
    }

    private void Gravity()
    {
        if (!isGound)
        {
            moveDirection.y -= 0.2f;
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

    void DistanceJoint()
    {
        if (jointTrriger)
        {
            dis.enabled = true;  
        }
        else
        {
            dis.enabled = false;
        }
    }
    IEnumerator JointWait()
    {
        yield return new WaitForSeconds(jointWaitTime);
        jointTrriger = false;
    }

    //----------------------------------------------
   
    void GroundChecker()
    {
        bool isGround = GroundCheck.grCheck.IsGroundHit(rigid.position, offsetX, offsetY, rayDist, layer);

        Debug.Log(isGround);
        if (isGround) { Debug.Log("�n�ʂ��m�F"); }
        else { Debug.Log("�n�ʂɐG��Ă��܂���"); }
    }
}
