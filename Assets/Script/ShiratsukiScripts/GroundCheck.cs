using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    #region Singleton
    public static GroundCheck grCheck;
    private void Awake()
    {
        if(grCheck == null)
        {
            grCheck = this;
        }
    }
    #endregion

    //接地判定
    const int judge = 4;                 //レイの本数
    Vector2[] pos = new Vector2[judge];
    bool[] hit = new bool[judge];


    //接地判定
    public bool IsGroundHit(Vector2 p, float xOffset, float yOffset, float rayDistance, LayerMask mask)
    {
        //常にプレイヤーの位置から接地判定
        //ここは本数を増やすなら手動で追加
        pos[0] = p + new Vector2(xOffset, 0);
        pos[1] = p + new Vector2(-xOffset, 0);
        pos[2] = p + new Vector2(0, yOffset);
        pos[3] = p + new Vector2(0, -yOffset);
        
        //レイの表示
        Debug.DrawRay(pos[0], Vector2.right * rayDistance, Color.cyan);
        Debug.DrawRay(pos[1], Vector2.left * rayDistance, Color.cyan);
        Debug.DrawRay(pos[2], Vector2.up * rayDistance, Color.cyan);
        Debug.DrawRay(pos[3], Vector2.down * rayDistance, Color.cyan);

        //判定
        hit[0] = Physics.Raycast(pos[0], Vector2.right, out RaycastHit info0, rayDistance, mask);
        hit[1] = Physics.Raycast(pos[1], Vector2.left, out RaycastHit info1, rayDistance, mask);
        hit[2] = Physics.Raycast(pos[2], Vector2.up, out RaycastHit info2, rayDistance, mask);
        hit[3] = Physics.Raycast(pos[3], Vector2.down, out RaycastHit info3, rayDistance, mask);

        Debug.Log(hit[3] + "hit3");

        return hit[0] || hit[1] || hit[2] || hit[3];
    }
}
