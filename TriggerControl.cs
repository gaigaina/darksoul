using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>

public class TriggerControl : MonoBehaviour
{
    private Animator anim;
    public IUserInput pi;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    public void ResetTrigger(string triggerName)
    {
        anim.ResetTrigger(triggerName);
    }
    public void StopTrigger(string triggerName)
    {
        anim.SetBool(triggerName,true);
    }
    public void StartTrigger(string triggerName)
    {
        anim.SetBool(triggerName, false);
    }
}
