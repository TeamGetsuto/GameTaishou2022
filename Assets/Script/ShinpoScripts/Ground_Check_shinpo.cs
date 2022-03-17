using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground_Check_shinpo : MonoBehaviour
{
    //地面接触判定
    //[SerializeField] GameObject player;
    //[SerializeField] LayerMask ground;
    //[SerializeField] Vector2 offset; //プレイヤーからの相対距離
    //Vector2 position;
    //Vector2 size;
    //bool isGround = false;


    //void Start()
    //{
    //    position.y = player.transform.position.y - 0.2f;
    //    size = player.transform.localScale;
    //}

    //void Update()
    //{

    //}

    //public bool IsGround()
    //{
    //    if (Physics2D.OverlapBox(position, size, ground))
    //    {
    //        isGround = true;
    //    }
    //    else
    //    {
    //        isGround = false;
    //    }

    //    return isGround;
    //}

    bool isGround = false;
    bool isGroundEnter, isGroundStay, isGoundExit;

    public bool IsGround()
    {
        if (isGroundEnter || isGroundStay)
        {
            isGround = true;
        }
        else if (isGoundExit)
        {
            isGround = false;
        }

        isGroundEnter = false;
        isGroundStay = false;
        isGoundExit = false;

        return isGround;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ground")
        {
            isGroundEnter = true;
            //Debug.Log("地面に接触しました");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Ground")
        {
            isGroundStay = true;
            //Debug.Log("地面に接触し続けています");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Ground")
        {
            isGoundExit = true;
            //Debug.Log("地面から離れました");
        }
    }
}
