using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>

public class DummyIUserInput : IUserInput
{
    private void Start()
    {
        attack = true;
    }

    private void Update()
    {
        UpdateDmagDvec(Dup, Dright);
    }
}
