using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>

public class EventCasterManager : IActorManagerInterface
{
    public string eventName;
    public bool active;
    public Vector3 offset = new Vector3(0, 0, 1f);

    private void Start()
    {
        if (am == null)
        {
            am = GetComponentInParent<ActorManager>();
        }
    }
}
