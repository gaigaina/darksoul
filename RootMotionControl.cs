using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>

public class RootMotionControl : MonoBehaviour
{
    private Animator anim;

    public void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void OnAnimatorMove()
    {
        SendMessageUpwards("OnUpdateRM", (object)anim.deltaPosition);
    }
}
