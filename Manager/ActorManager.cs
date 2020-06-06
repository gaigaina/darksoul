using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>

public class ActorManager : MonoBehaviour
{
    public ActorController ac;
    [Header("=== Auto Generate if Null ===")]
    public BattleManager bm;
    public WeaponManager wm;
    public StateManager sm;
    public DirectorManager dm;
    public InterActionManager im;

    private void Awake()
    {
        ac = GetComponent<ActorController>();
        GameObject model = ac.model;
        GameObject sensor = null;
        try
        {//尝试获取，没有则跳过
            sensor = transform.Find("sensor").gameObject;
        }
        catch (System.Exception)
        {

        }

        bm = Bind<BattleManager>(sensor);
        wm = Bind<WeaponManager>(model);
        sm = Bind<StateManager>(gameObject);
        dm = Bind<DirectorManager>(gameObject);
        im = Bind<InterActionManager>(sensor);

        ac.OnAction += DoAction;
    }
    public void DoAction()
    {
        if (im.overlapEcastms.Count != 0)
        {
            if (im.overlapEcastms[0].active == true && !dm.IsPlaying())
            {
                if (im.overlapEcastms[0].eventName == "frontStab")
                {
                    dm.PlayFrontStab("frontStab", this, im.overlapEcastms[0].am);
                }
                else if (im.overlapEcastms[0].eventName == "openBox")
                {
                    if (BattleManager.CheckAnglePlayer(ac.model, im.overlapEcastms[0].am.gameObject,180))
                    {
                        im.overlapEcastms[0].active = false;
                        transform.position = im.overlapEcastms[0].am.transform.position 
                            + im.overlapEcastms[0].am.transform.TransformVector(im.overlapEcastms[0].offset);
                        ac.model.transform.LookAt(im.overlapEcastms[0].am.transform, Vector3.up);
                        dm.PlayFrontStab("openBox", this, im.overlapEcastms[0].am);
                    }
                }
                else if (im.overlapEcastms[0].eventName == "leverUp")
                {
                    if (BattleManager.CheckAnglePlayer(ac.model, im.overlapEcastms[0].am.gameObject, 180))
                    {
                        transform.position = im.overlapEcastms[0].am.transform.position
                            + im.overlapEcastms[0].am.transform.TransformVector(im.overlapEcastms[0].offset);
                        ac.model.transform.LookAt(im.overlapEcastms[0].am.transform, Vector3.up);
                        dm.PlayFrontStab("leverUp", this, im.overlapEcastms[0].am);
                    }
                }

            }
        }
    }

    private T Bind<T>(GameObject go) where T:IActorManagerInterface  //泛型
    {
        T tempInst;
        if (go == null)
        {
            return null;
        }
        tempInst = go.GetComponent<T>();
        if (tempInst == null)
        {
            tempInst = go.AddComponent<T>();
        }
        tempInst.am = this;

        return tempInst;
    }

    public void SetIsCounterBack(bool value)
    {
        sm.isCounterBackEnable = value;
    }
    public void TryDoDamage(WeaponController targetWc,bool attackerValid,bool counterValid)
    {
        if (sm.isCounterBackSuccess)
        {
            if (counterValid)
            {
                targetWc.wm.am.Stunned();
            }
        }
        else if (sm.isCounterBackFailure)
        {
            
            HitOrDie(targetWc);
        }
        else if (sm.isImmortal)
        {

        }
        else if (sm.isDefense)
        {
            Blocked();
        }
        else
        {
            HitOrDie(targetWc);
        }
    }

    public void Stunned()
    {
        ac.IssueTrigger("stunned");
    }
    public void Blocked()
    {
        ac.IssueTrigger("blocked");
    }
    public void HitOrDie(WeaponController targetWc)
    {
        if (sm.HP <= 0)
        {

        }
        else
        {
            sm.AddHP(-1 * targetWc.GetATK());
            if (sm.HP > 0)
            {
                Hit();
            }
            else
            {
                Die();
            }
        }
    }
    public void Hit()
    {
        ac.IssueTrigger("hit");
    }
    public void Die()
    {
        ac.IssueTrigger("die");

        //ac.camcon.enabled = false;
    }

    public void LockUnlockActorController(bool value)
    {
        ac.SetBool("lock",value);
    }

}
