using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 武器开关
/// </summary>

public class WeaponManager : IActorManagerInterface
{
    private Collider weaponColL;
    private Collider weaponColR;
    public GameObject whL;
    public GameObject whR;
    public WeaponController wcL;
    public WeaponController wcR;

    private void Start()
    {
        try
        {
            whL = transform.DeepFind("WeaponHandleL").gameObject;
            wcL = BindWeaponController(whL);
            weaponColL = whL.GetComponentInChildren<Collider>();
        }
        catch (System.Exception)
        {

        }
        try
        {
            whR = transform.DeepFind("WeaponHandleR").gameObject;
            wcR = BindWeaponController(whR);
            weaponColR = whR.GetComponentInChildren<Collider>();
            weaponColR.enabled = false;
        }
        catch (System.Exception)
        {

        }
    }

    public WeaponController BindWeaponController(GameObject targetObj)//获取WC，若无则自动添加
    {
        WeaponController tempWc;
        tempWc = targetObj.GetComponent<WeaponController>();
        if (tempWc == null)
        {
            tempWc = targetObj.AddComponent<WeaponController>();
        }
        tempWc.wm = this;
        return tempWc;
    }
    public void WeaponEnable()
    {
        weaponColR.enabled = true;
    }
    public void WeaponDisable()
    {
        weaponColR.enabled = false;
    }
    public void CounterBackEnable()
    {
        am.SetIsCounterBack(true);
    }
    public void CounterBackDisable()
    {
        am.SetIsCounterBack(false);
    }
}
