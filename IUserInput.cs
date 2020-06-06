using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>

public abstract class IUserInput : MonoBehaviour
{
    [Header("=====  output signal =====")]
    public float Dup;
    public float Dright;
    public float lockDup;
    public float lockDright;
    public float Dmag;
    public float lockDmag;
    public Vector3 Dvec;
    public Vector3 lockDvec;
    protected float Dup2;
    protected float Dright2;
    protected float lockDup2;
    protected float lockDright2;
    protected Vector2 tempDAxis;
    protected Vector2 lockTempDAxis;
    public float Jup;
    public float Jright;
    //按住信号
    public bool run;
    public bool defense;
    public bool heavyAttack;
    public bool counterBack;
    //按一次
    public bool action;
    public bool jump;
    public bool attack = false;
    public bool roll;
    public bool lockon;
    public bool locking;
    //按两次
    [Header("=====  others =====")]
    public bool inputEnable = true;
    public bool lockEnable = false;
    protected float targetDup;
    protected float targetDright;
    protected float lockTargetDup;
    protected float lockTargetDright;
    protected float velocityDup;
    protected float velocityDright;

    protected Vector2 SquareToCircle(Vector2 input)
    {
        Vector2 output = Vector2.zero;
        output.x = input.x * Mathf.Sqrt(1 - (input.y * input.y) / 2.0f);
        output.y = input.y * Mathf.Sqrt(1 - (input.x * input.x) / 2.0f);
        return output;
    }

    protected void UpdateDmagDvec(float Dup2,float Dright2)
    {
        Dmag = Mathf.Sqrt(Dup2 * Dup2 + Dright2 * Dright2);//速度
        Dvec = Dright2 * transform.right + Dup2 * transform.forward; //方向
        
    }
}
