using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>

public class LeftArmAnimFix : MonoBehaviour
{
    private Animator anim;
    public Vector3 a;

    public void Awake()
    {
        anim=GetComponent<Animator>();
    }

    private void OnAnimatorIK()
    {
        if (anim.GetBool("defense")==false)
        {
        Transform leftLowerArm = anim.GetBoneTransform(HumanBodyBones.LeftLowerArm);
        leftLowerArm.localEulerAngles += a;
        anim.SetBoneLocalRotation(HumanBodyBones.LeftLowerArm,Quaternion.Euler(leftLowerArm.localEulerAngles));

        }
    }
    private bool CheckState(string stateName, string layerName)
    {
        return anim.GetCurrentAnimatorStateInfo(anim.GetLayerIndex(layerName)).IsName(stateName);
    }
}
