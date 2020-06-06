using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>

public class ActorController : MonoBehaviour
{
    public GameObject model;
    public CameraControl camcon;
    public IUserInput pi;
    public float walkSpeed;
    public float runSpeed;
    public float jumpVelocity = 5.0f;
    public float rollVelocity = 5.0f;
    public float rollSpeed;
    public Vector3 rollDvec;

    [Header("===== Friction Settings =====")]
    public PhysicMaterial frictionOne;
    public PhysicMaterial frictionZero;

    public Animator anim;
    private Rigidbody rigid;
    public Vector3 planarVec;//速度
    private Vector3 thrustVec;//冲量，跳跃
    private bool lockPlanar = false;
    private bool trackDirection = false;//追踪方向
    private bool canAttack = true;
    private bool landing = false;
    private bool lockroll;
    //public bool action=false;
    private CapsuleCollider col;
    //private float lerpTarget;
    private Vector3 deltaPos;
    private Vector3 deltaPos2;

    public delegate void OnActionDelegate();
    public event OnActionDelegate OnAction;

    private void Awake()
    {
        IUserInput[] inputs = GetComponents<IUserInput>();
        {
            foreach (var input in inputs)
            {
                if (input.enabled==true)
                {
                    pi = input;
                    break;
                }
            }
        }
        anim = model.GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
    }

    private void Update()
    {
        anim.SetFloat("fallSpeed", rigid.velocity.magnitude);
        if (pi.lockon)
        {
            camcon.LockUnlock();
        }
        if (camcon.lockState==false)
        {
            anim.SetFloat("Forward", pi.Dmag* Mathf.Lerp(anim.GetFloat("Forward"),(pi.run) ? 2.0f : 1.0f,0.05f));//设置走跑动画，平滑切换
            anim.SetFloat("right", 0);
        }
        else
        {
            Vector3 localDVec = transform.InverseTransformVector(pi.Dvec);
            anim.SetFloat("Forward", localDVec.z*((pi.run) ? 2.0f : 1.0f));
            anim.SetFloat("right", localDVec.x * ((pi.run) ? 2.0f : 1.0f));
        }

        if (CheckState("ground") || CheckState("blocked"))
        {
            anim.SetBool("defense", pi.defense);
            anim.SetLayerWeight(anim.GetLayerIndex("defense"), 1);
            anim.SetLayerWeight(anim.GetLayerIndex("defense2"), 1);
        }
        else
        {
            anim.SetBool("defense", false);
            anim.SetLayerWeight(anim.GetLayerIndex("defense"), 0);
            anim.SetLayerWeight(anim.GetLayerIndex("defense2"), 0);
        }

        if (pi.jump)
        {
            anim.SetTrigger("jump");
            canAttack = false;
        }

        
        if (pi.attack == true && canAttack && pi.heavyAttack == false )
        {
            if (anim.GetFloat("Forward") < 1.3f)
            {
                anim.SetTrigger("attack");
            }
            else
            {
                anim.SetTrigger("runAttack");
            }
        }

        if (pi.heavyAttack == true && canAttack)
        {
            anim.SetTrigger("heavyAttack");
        }
        if (pi.counterBack == true && CheckState("ground"))
        { 
            anim.SetTrigger("counterBack");
        }

        

        if (anim.GetBool("rollLock")==false)
        {
            if (pi.roll)  
            {
                anim.SetTrigger("roll");
                canAttack = false;
               
            }
        }

        if (trackDirection==true)//锁死人物方向
        {
            model.transform.forward = planarVec.normalized;
        }
        if (camcon.lockState==false)
        {
            if (pi.lockEnable == false)
            {
                if (pi.inputEnable == true)
                {
                    if (pi.Dmag > 0.01f)
                    {
                        model.transform.forward = Vector3.Slerp(model.transform.forward, pi.Dvec, 0.3f);//平滑转向
                    }
                }

                if (lockPlanar == false)
                {
                    planarVec = pi.Dmag * model.transform.forward * walkSpeed * ((pi.run) ? runSpeed : 1.0f);//移动速度
                }
                if (lockPlanar == true && landing == true)
                {
                    planarVec = Vector3.zero;
                }
            }
            else if (lockroll == false)
            {
                planarVec = rollDvec * rollSpeed;
            }
            else if (CheckState("die") && lockroll == true)
            {
                planarVec = Vector3.zero;
            }
        }
        else
        {
            if (pi.lockEnable == false)
            {
                if (trackDirection == false)
                {
                    model.transform.forward = transform.forward;
                }

                if (lockPlanar == false)
                {
                    planarVec = pi.Dvec * walkSpeed * ((pi.run) ? runSpeed : 1.0f);
                }
                if (lockPlanar == true && landing == true)
                {
                    planarVec = Vector3.zero;
                }
            }
            else if (lockroll == false)
            {
                planarVec = rollDvec * rollSpeed;
            }
            else if (CheckState("die") && lockroll == true)
            {
                planarVec = Vector3.zero;
            }
        }
        if (CheckState("lock"))
        {
            planarVec = Vector3.zero;
        }
        if (pi.action)
        {
            OnAction.Invoke();
        }
    }

    private void FixedUpdate()
    {
        rigid.position += deltaPos;
        //rigid.position += planarVec * Time.fixedDeltaTime;
        rigid.velocity = new Vector3(planarVec.x, rigid.velocity.y, planarVec.z) + thrustVec;
        thrustVec = Vector3.zero;
        deltaPos = Vector3.zero;
    }

    public bool CheckState(string stateName, string layerName="base")
    {
        return anim.GetCurrentAnimatorStateInfo(anim.GetLayerIndex(layerName)).IsName(stateName);
    }
    public bool CheckTag(string tagName, string layerName= "base")
    {
        return anim.GetCurrentAnimatorStateInfo(anim.GetLayerIndex(layerName)).IsTag(tagName);
    }
    /// <summary>
    /// 处理各种信号
    /// </summary>
    public void OnJumpEnter()
    {
        lockPlanar = true;
        trackDirection = true;
        pi.inputEnable = false;
        lockroll = true;
        thrustVec = new Vector3(0, jumpVelocity, 0); 
    }

    public void IsGround()
    {
        anim.SetBool("isGround",true);

    }
    public void IsNotGround()
    {
        anim.SetBool("isGround", false);

    }
    public void OnGroundEnter()
    {
        lockPlanar = false;
        trackDirection = false;
        pi.inputEnable = true;
        pi.lockEnable = false;
        canAttack = true;
        landing = false;
        col.material = frictionOne;
    }
    public void OnGroundExit()
    {
        col.material = frictionZero;
    }

    public void OnFallEnter()
    {
        lockPlanar = true;
        pi.inputEnable = false;
        canAttack = false;
        lockroll = true;
    }
    public void OnRollEnter()
    {
        lockPlanar = true;
        trackDirection = true;
        pi.inputEnable = false;
        pi.lockEnable = true;
        rollDvec = pi.lockDvec;
        thrustVec = new Vector3(0, rollVelocity, 0);

    }
    public void OnRollExit()
    {
        lockroll = false;
        //rollDvec = Vector3.zero;
    }
    public void OnLandEnter()
    {
        pi.inputEnable = false;
        landing = true;
        pi.lockEnable = false;
    }
    public void OnHitEnter()
    {
        pi.inputEnable = false;
        pi.lockEnable = false;
        planarVec = Vector3.zero;
    }
    public void OnDieEnter()
    {
        pi.inputEnable = false;
        planarVec = Vector3.zero;
        if (camcon.lockState == true)
        {
            camcon.LockUnlock();
        }
    }
    public void OnBlockedEnter()
    {
        pi.inputEnable = false;
    }
    public void OnStunnedEnter()
    {
        pi.inputEnable = false;
        planarVec = Vector3.zero;
    }
    public void OnCounterBackEnter()
    {
        pi.inputEnable = false;
        planarVec = Vector3.zero;
    }
    public void OnCounterBackExit()
    {
        model.SendMessage("CounterBackDisable");
    }
    public void OnLockEnter()
    {
        pi.inputEnable = false;
        planarVec = Vector3.zero;
        model.SendMessage("WeaponDisable");
    }
    public void OnLandExit()
    {
        lockroll = false;
    }
    public void OnJabEnter()
    {
        lockPlanar = true;
        pi.inputEnable = false;
        lockroll = true;
    }
    public void OnJabUpdate()
    {
        thrustVec = model.transform.forward * anim.GetFloat("jabVelocity");
    }

    public void OnAttack1hAEnter()
    {
        pi.inputEnable = false;
    }
    public void OnAttackExit()
    {
        model.SendMessage("WeaponDisable");
    }

    public void OnUpdateRM(object _deltaPos)
    {
        if (pi.lockEnable == false)
        {
            if (CheckTag("attack") || CheckState("land")|| CheckState("die"))
            {
                 deltaPos += (Vector3)_deltaPos;
            }
        }
    }

    public void IssueTrigger(string triggerName)
    {
        anim.SetTrigger(triggerName);
    }
    public void SetBool(string boolName,bool value)
    {
        anim.SetBool(boolName,value);
    }
}
