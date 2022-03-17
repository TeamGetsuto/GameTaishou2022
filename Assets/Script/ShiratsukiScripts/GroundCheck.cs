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

    //�ڒn����
    const int judge = 4;                 //���C�̖{��
    Vector2[] pos = new Vector2[judge];
    bool[] hit = new bool[judge];


    //�ڒn����
    public bool IsGroundHit(Vector2 p, float xOffset, float yOffset, float rayDistance, LayerMask mask)
    {
        //��Ƀv���C���[�̈ʒu����ڒn����
        //�����͖{���𑝂₷�Ȃ�蓮�Œǉ�
        pos[0] = p + new Vector2(xOffset, 0);
        pos[1] = p + new Vector2(-xOffset, 0);
        pos[2] = p + new Vector2(0, yOffset);
        pos[3] = p + new Vector2(0, -yOffset);
        
        //���C�̕\��
        Debug.DrawRay(pos[0], Vector2.right * rayDistance, Color.cyan);
        Debug.DrawRay(pos[1], Vector2.left * rayDistance, Color.cyan);
        Debug.DrawRay(pos[2], Vector2.up * rayDistance, Color.cyan);
        Debug.DrawRay(pos[3], Vector2.down * rayDistance, Color.cyan);

        //����
        hit[0] = Physics.Raycast(pos[0], Vector2.right, out RaycastHit info0, rayDistance, mask);
        hit[1] = Physics.Raycast(pos[1], Vector2.left, out RaycastHit info1, rayDistance, mask);
        hit[2] = Physics.Raycast(pos[2], Vector2.up, out RaycastHit info2, rayDistance, mask);
        hit[3] = Physics.Raycast(pos[3], Vector2.down, out RaycastHit info3, rayDistance, mask);

        Debug.Log(hit[3] + "hit3");

        return hit[0] || hit[1] || hit[2] || hit[3];
    }
}
