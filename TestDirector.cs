using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

/// <summary>
/// 
/// </summary>

public class TestDirector : MonoBehaviour
{
    public PlayableDirector pd;

    public Animator Attacker;
    public Animator Victim;
    private void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            foreach (var track in pd.playableAsset.outputs)
            {
                if (track.streamName == "Attacker Animator")
                {
                    pd.SetGenericBinding(track.sourceObject, Attacker);
                }
                else if (track.streamName == "Victim Animator")
                {
                    pd.SetGenericBinding(track.sourceObject, Victim);
                }
            }
            pd.Play();
        }
    }
}
