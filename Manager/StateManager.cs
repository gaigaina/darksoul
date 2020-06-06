using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 存储数值
/// </summary>

public class StateManager : IActorManagerInterface
{
    public float HP = 15f;
    public float maxHP = 15f;
    public float ATK = 10f;

    [Header("1st order state flags")]
    public bool isGround;
    public bool isJump;
    public bool isFall;
    public bool isRoll;
    public bool isJab;
    public bool isAttack;
    public bool isHit;
    public bool isDie;
    public bool isBlocked;
    public bool isDefense;
    public bool isCounterBack;//是否在盾反动画
    public bool isCounterBackEnable;

    [Header("2nd order state flags")]
    public bool canDefense;
    public bool isImmortal;//无敌状态
    public bool isCounterBackSuccess;
    public bool isCounterBackFailure;
    private void Start()
    {
        HP = maxHP;
    }

    private void Update()
    {
        isGround = am.ac.CheckState("ground");
        isJump = am.ac.CheckState("jump");
        isFall = am.ac.CheckState("fall");
        isRoll = am.ac.CheckState("roll");
        isJab = am.ac.CheckState("jab");
        isAttack = am.ac.CheckTag("attack");
        isHit = am.ac.CheckState("hit");
        isDie = am.ac.CheckState("die");
        isBlocked = am.ac.CheckState("blocked");
        isCounterBack = am.ac.CheckState("counterBack");
        isCounterBackSuccess = isCounterBackEnable;
        isCounterBackFailure = isCounterBack && !isCounterBackEnable;

        canDefense = isGround || isBlocked;
        isDefense = canDefense && am.ac.CheckState("defense", "defense");
        isImmortal = isRoll || isJab;
    }
    public void AddHP(float value)
    {
        HP += value;
        HP = Mathf.Clamp(HP, 0, maxHP);
        
    }
    
}
