using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 计时器
/// </summary>

public class MyTimer 
{
    public enum STATE
    {
        IDLE,
        RUN,
        FINISHED
    }
    public STATE state;
    public float duration = 1f; //延迟时间
    private float elapsedTime = 0;  //过去的时间

    public void Tick()
    {
        if (state == STATE.IDLE)
        {

        }
        else if (state==STATE.RUN)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime>=duration)
            {
                state = STATE.FINISHED;
            }
        }
        else if (state==STATE.FINISHED)
        {

        }
        else
        {
            Debug.Log("error");
        }
    }

    public void Go()
    {
        elapsedTime = 0;
        state = STATE.RUN;
    }

}
