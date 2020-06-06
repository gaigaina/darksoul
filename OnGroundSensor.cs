using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 落地侦测
/// </summary>

public class OnGroundSensor : MonoBehaviour
{
    public CapsuleCollider capcol;
    private Vector3 point1;//胶囊体下圆心
    private Vector3 point2;//胶囊体上圆心
    private float radius;//胶囊体圆半径
    public float offset=0.1f;//向下沉降

    private void Awake()
    {
        radius = capcol.radius-0.05f;
    }

    private void FixedUpdate()
    {
        point1 = transform.position + transform.up * (radius- offset);//圆心缩小并向下沉降，胶囊体碰撞体积缩小向下
        point2 = transform.position + transform.up * (capcol.height- offset) - transform.up * radius;
        Collider[] outputCols = Physics.OverlapCapsule(point1, point2, radius, LayerMask.GetMask("Ground"));
        if (outputCols.Length != 0)
        {
            SendMessageUpwards("IsGround");
        }
        else SendMessageUpwards("IsNotGround");
    }
}
