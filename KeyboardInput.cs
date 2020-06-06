using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>

public class KeyboardInput : IUserInput
{
    [Header("=====  key settings =====")]
    public string keyUp="w";
    public string keyDown="s";
    public string keyLeft="a";
    public string keyRight="d";
    public string keyA;
    public string keyB;
    public string keyC;
    public string keyD;
    public string keyE;
    public string keyF;

    public MyButton buttonA = new MyButton();
    public MyButton buttonB = new MyButton();
    public MyButton buttonC = new MyButton();
    public MyButton buttonD = new MyButton();
    public MyButton buttonE = new MyButton();
    public MyButton buttonF = new MyButton();

    public string keyJup = "up";
    public string keyJdown = "down";
    public string keyJright="right";
    public string keyJleft="left";
    public ActorController actor;
    [Header("===== Mouse settings =====")]
    public bool mouseEnalbe = false;
    public float mouseSensitivityX = 1.0f;
    public float mouseSensitivityY = 1.0f;

    private void Update()
    {
        buttonA.Tick(Input.GetKey(keyA));
        buttonB.Tick(Input.GetKey(keyB));
        buttonC.Tick(Input.GetKey(keyC));
        buttonD.Tick(Input.GetKey(keyD));
        buttonE.Tick(Input.GetKey(keyE));
        buttonF.Tick(Input.GetKey(keyF));

        if (mouseEnalbe == true) 
        {
            Jup = Input.GetAxis("Mouse Y") * 2f * mouseSensitivityY;
            Jright = Input.GetAxis("Mouse X") * 1.5f * mouseSensitivityX;
        }
        else
        {
            Jup = (Input.GetKey(keyJup) ? 1.0f : 0) - (Input.GetKey(keyJdown) ? 1.0f : 0);
            Jright = (Input.GetKey(keyJright) ? 1.0f : 0) - (Input.GetKey(keyJleft) ? 1.0f : 0);
        }

        targetDup = (Input.GetKey(keyUp) ? 1.0f : 0) - (Input.GetKey(keyDown) ? 1.0f : 0);
        targetDright = (Input.GetKey(keyRight) ? 1.0f : 0) - (Input.GetKey(keyLeft) ? 1.0f : 0);

        lockTargetDup = (Input.GetKey(keyUp) ? 1.0f : 0) - (Input.GetKey(keyDown) ? 1.0f : 0);
        lockTargetDright = (Input.GetKey(keyRight) ? 1.0f : 0) - (Input.GetKey(keyLeft) ? 1.0f : 0);

        if (inputEnable == false)
        {
            targetDup = 0;
            targetDright = 0;
        }

        Dup = Mathf.SmoothDamp(Dup, targetDup, ref velocityDup, 0.1f);
        Dright = Mathf.SmoothDamp(Dright, targetDright, ref velocityDright, 0.1f);
        lockDup = lockTargetDup;
        lockDright = lockTargetDright;

        tempDAxis = SquareToCircle(new Vector2(Dright, Dup));
        Dright2 = tempDAxis.x;
        Dup2 = tempDAxis.y;
        lockTempDAxis = SquareToCircle(new Vector2(lockDright, lockDup));
        lockDright2 = lockTempDAxis.x;
        lockDup2 = lockTempDAxis.y;

        Dmag = Mathf.Sqrt(Dup2 * Dup2 + Dright2 * Dright2);//速度
        Dvec = Dright2 * transform.right + Dup2 * transform.forward; //方向

        lockDvec= lockDright2 * transform.right + lockDup2 * transform.forward;


        if (locking == false)
        {
            run = (buttonA.IsPressing && !buttonA.IsDelaying) || buttonA.IsExtending;
            jump = buttonA.OnPressed && buttonA.IsExtending;
            roll = buttonA.OnReleased && buttonA.IsDelaying;
            attack = buttonC.OnPressed;
            action = buttonF.OnPressed;
            heavyAttack = buttonC.OnPressed && buttonE.IsPressing;
            counterBack = (buttonD.OnPressed && buttonE.IsPressing) || (buttonD.IsPressing && buttonE.OnPressed);
            defense = buttonD.IsPressing;
            lockon = buttonB.OnPressed;
        }
        else
        {
            run = (buttonA.IsPressing && !buttonA.IsDelaying) || buttonA.IsExtending;
            roll = buttonA.OnReleased && buttonA.IsDelaying;
            attack = buttonC.OnPressed;
            action = buttonF.OnPressed;
            heavyAttack = buttonC.OnPressed && buttonE.IsPressing;
            counterBack = (buttonD.OnPressed && buttonE.IsPressing) || (buttonD.IsPressing && buttonE.OnPressed);
            defense = buttonD.IsPressing;
            lockon = buttonB.OnPressed;
        }
    }

    //private Vector2 SquareToCircle(Vector2 input)
    //{
    //    Vector2 output = Vector2.zero;
    //    output.x = input.x * Mathf.Sqrt(1 - (input.y * input.y) / 2.0f);
    //    output.y = input.y * Mathf.Sqrt(1 - (input.x * input.x) / 2.0f);
    //    return output;
    //}
}
