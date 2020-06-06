using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>

public class WeaponController : MonoBehaviour
{
    public WeaponManager wm;
    public WeaponData wdata;

    private void Awake()
    {
        wdata = GetComponentInChildren<WeaponData>();
    }

    public float GetATK()
    {
        return wdata.ATK + wm.am.sm.ATK;
    }
}
