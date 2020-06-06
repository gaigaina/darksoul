using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 判断攻击
/// </summary>
[RequireComponent(typeof(CapsuleCollider))]
public class BattleManager : IActorManagerInterface
{
    private CapsuleCollider defCol;
    private void Start()
    {
        defCol = GetComponent<CapsuleCollider>();
        defCol.center = Vector3.up*1f;
        defCol.height = 2f;
        defCol.radius = 0.35f;
        defCol.isTrigger = true;

    }

    private void OnTriggerEnter(Collider col)
    {
        WeaponController targetWc = col.GetComponentInParent<WeaponController>();

        if (targetWc == null)
        {
            return;
        }
        GameObject attacker = targetWc.wm.am.gameObject;
        GameObject receiver = am.ac.model;

        if (col.tag=="Weapon")
        {
            am.TryDoDamage(targetWc,CheckAngleTarget(receiver,attacker,45),CheckAnglePlayer(receiver,attacker,30));
        }
    }
    public static bool CheckAnglePlayer(GameObject player,GameObject target,float playerAngleLimit)//玩家在敌人视角的角度
    {
        Vector3 counterDir = target.transform.position - player.transform.position;

        float counterAngle1 = Vector3.Angle(player.transform.forward, counterDir);
        float counterAngle2 = Vector3.Angle(target.transform.forward, player.transform.forward);

        bool counterValid = (counterAngle1 < playerAngleLimit && Mathf.Abs(counterAngle2 - 180) < playerAngleLimit);
        return counterValid;
    }
    public static bool CheckAngleTarget(GameObject player,GameObject target,float targetAngleLimit)//敌人在玩家的视角的角度
    {
        Vector3 attackingDir = player.transform.position - target.transform.position;

        float attackingAngle1 = Vector3.Angle(target.transform.forward, attackingDir);

        bool attackValid = (attackingAngle1 < targetAngleLimit);
        return attackValid;
    }
}
