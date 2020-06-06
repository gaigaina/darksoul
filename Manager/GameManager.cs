using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        GameObject prefab = (GameObject)Resources.Load("Sword");
        Instantiate(prefab, Vector3.zero, Quaternion.identity);
    }

}
