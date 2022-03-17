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
    bool IsGroundHit(Vector2 p, float xOffset, float yOffset, float rayDistance, LayerMask mask)
    {
        //��Ƀv���C���[�̈ʒu����ڒn����
        //�����͖{���𑝂₷�Ȃ�蓮�Œǉ�
        pos[0] = p + new Vector2(xOffset, 0);
        pos[1] = p + new Vector2(-xOffset, 0);
        pos[2] = p + new Vector2(0, yOffset);
        pos[3] = p + new Vector2(0, -yOffset);
        //-----------------------------------------------

        int i;
        for (i = 0; pos[i].x != 0; ++i)
        {
            //���C�̕\��
            Debug.DrawRay(pos[i], Vector2.right * rayDistance, Color.cyan);

            //����
            hit[i] = Physics.Raycast(pos[i], Vector2.right, out RaycastHit info, rayDistance, mask);
        }
        for (; i < judge; ++i)
        {
            //���C�̕\��
            Debug.DrawRay(pos[i], Vector2.up * rayDistance, Color.cyan);

            //����
            hit[i] = Physics.Raycast(pos[i], Vector2.up, out RaycastHit info, rayDistance, mask);
        }

        return hit[0] || hit[1] || hit[2] || hit[3];
    }
}
