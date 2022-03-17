using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Move : MonoBehaviour
{
    //入力媒体
    Gamepad gamepad;

    //プレイヤー
    Rigidbody2D rigid;

    //ジャンプ
    [Header("jump")]
    [SerializeField] float jumpForce;
    [SerializeField] bool isJump = false;
    [SerializeField] bool isGound = false;
    [SerializeField] float waitTime;
    public Ground_Check ground;
    bool isGravity = true;

    [Header("接地判定")]
    [SerializeField] float offsetX;
    [SerializeField] float offsetY;
    [SerializeField] float rayDist;
    [SerializeField] LayerMask layer;

    //移動
    [SerializeField] float walkSpeed;
    [SerializeField] Vector2 moveDirection;

    [Header("サブプレイヤ")]
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

        //地面の接触判定
        isGound = ground.IsGround();
        //ジャンプしていないときかつ地面接触しているとき
        if (!isJump && isGound)
        {
            if (gamepad.buttonNorth.wasPressedThisFrame)
            {
                Debug.Log("ジャンプ中");
                isJump = true;
            }
        }
        
        //移動
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

        //回転のトリガー
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
        //ジャンプする
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
        Debug.Log("ジャンプ中");
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
        if (isGround) { Debug.Log("地面を確認"); }
        else { Debug.Log("地面に触れていません"); }
    }
}
