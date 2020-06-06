using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 
/// </summary>

public class CameraControl : MonoBehaviour
{
    public IUserInput pi;
    private GameObject cameraHandle;
    private GameObject playerHandle;
    private GameObject model;
    public GameObject camera;
    public Image lockDot;
    public bool lockState;
    public bool isAI;

    [SerializeField]
    private LockTarget lockTarget;

    public float horizontalSpeed;//左右旋转速度
    public float verticalSpeed;//上下旋转速度
    private float tempEulerX;//存储上下旋转的角度
    Vector3 tempModelEuler;//保持视角旋转时模型的方向不变
    private void Start()
    {
        tempEulerX = 20;
        
        cameraHandle = transform.parent.gameObject;
        playerHandle = cameraHandle.transform.parent.gameObject;
        ActorController ac = playerHandle.GetComponent<ActorController>();
        model = ac.model;
        pi = ac.pi;

        if (!isAI)
        {
            camera = Camera.main.gameObject;
            Cursor.lockState = CursorLockMode.Locked;
            lockDot.enabled = false;
        }

        lockState = false;
    }

    private void FixedUpdate()
    {
        if (lockTarget == null) 
        {
            tempModelEuler = model.transform.eulerAngles;//保持视角旋转时模型的方向不变

            playerHandle.transform.Rotate(Vector3.up, pi.Jright * horizontalSpeed * Time.fixedDeltaTime);
            tempEulerX -= pi.Jup * verticalSpeed * Time.fixedDeltaTime;
            tempEulerX = Mathf.Clamp(tempEulerX, -40, 40);
            cameraHandle.transform.localEulerAngles = new Vector3(tempEulerX, 0, 0);

            model.transform.eulerAngles = tempModelEuler;//保持视角旋转时模型的方向不变

        }
        else
        {
            Vector3 tempForward = lockTarget.obj.transform.position - model.transform.position;//锁定方向
            tempForward.y = 0;
            playerHandle.transform.forward = tempForward;
            cameraHandle.transform.LookAt(lockTarget.obj.transform.position + new Vector3(0, 0.5f * lockTarget.halfHeight, 0));
        }
        if (!isAI)
        {
            //camera.transform.position = transform.position;
            camera.transform.position = Vector3.Lerp(camera.transform.position, transform.position,0.2f);
            camera.transform.LookAt(cameraHandle.transform);
        }
    }

    public void Update()
    {
        if (lockTarget != null) 
        {
            if (!isAI)
            {
                lockDot.rectTransform.position = Camera.main.WorldToScreenPoint(
                    lockTarget.obj.transform.position+new Vector3(0,lockTarget.halfHeight,0));
            }
            ActorManager targetAm = lockTarget.obj.GetComponent<ActorManager>();
            if (Vector3.Distance(model.transform.position, lockTarget.obj.transform.position) > 13f 
                || (targetAm != null && targetAm.sm.isDie))
            {
                lockTarget = null;
                lockState = false;
                if (!isAI)
                {
                    lockDot.enabled = false;
                }
            }

        }
    }

    public void LockUnlock()
    {
        Vector3 modelOrigin1 = model.transform.position;
        Vector3 modelOrigin2 = modelOrigin1 + new Vector3(0, 1, 0);
        Vector3 boxCenter;
        if (!isAI)
        {
            boxCenter = modelOrigin2 + camera.transform.forward * 6f;
        }
        else
        {
            boxCenter = modelOrigin2 + model.transform.forward * 6f;
        }
        
        Collider[] cols = Physics.OverlapBox(boxCenter, new Vector3(2f, 2f, 6f),
            isAI? model.transform.rotation:camera.transform.rotation,LayerMask.GetMask(isAI?"Player":"Enemy"));
        if (cols.Length == 0)  
        {
            lockTarget = null;
            lockState = false;
            pi.locking = false;
            if (!isAI)
            {
                lockDot.enabled = false;
            }
        }
        else
        {
            int index = 0;
            float minDistance = Vector3.Distance(model.transform.position, cols[0].transform.position);
            float nowDistance;
            for (int i = 0; i < cols.Length; i++)
            {
                nowDistance = Vector3.Distance(model.transform.position, cols[i].transform.position);
                if (nowDistance<minDistance)
                {
                    minDistance = nowDistance;
                    index = i;
                }
            }

            if (lockTarget != null) 
            {
                lockTarget = null;
                if (!isAI)
                {
                    lockDot.enabled = false;
                }
                lockState = false;
                pi.locking = false;
            }
            else
            {
                lockTarget = new LockTarget(cols[index].gameObject, cols[index].bounds.extents.y);
                if (!isAI)
                {
                    lockDot.enabled = true;
                }
                lockState = true;
                pi.locking = true;
            }
        }
    }

    private class LockTarget
    {
        public GameObject obj;
        public float halfHeight;

        public LockTarget(GameObject _obj,float _halfHeight)
        {
            obj = _obj;
            halfHeight = _halfHeight;
        }
    }
}
