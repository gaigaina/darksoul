using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

/// <summary>
/// 
/// </summary>

[RequireComponent(typeof(PlayableDirector))]
public class DirectorManager : IActorManagerInterface
{
    public PlayableDirector pd;

    [Header("=== Timeline assets ===")]
    public TimelineAsset frontStab;
    public TimelineAsset openBox;
    public TimelineAsset leverUp;

    [Header("=== Assets Settings ===")]
    public ActorManager attacker;
    public ActorManager victim;
    
    private void Start()
    {
        pd = GetComponent<PlayableDirector>();
        pd.playOnAwake = false;
        //pd.playableAsset = Instantiate(frontStab);

    }

    private void Update()
    {

    }

    public bool IsPlaying()
    {
        if (pd.state == PlayState.Playing)
        {
            return true;
        }
        return false;
    }
    public void PlayFrontStab(string timelineName, ActorManager attcker, ActorManager victim)
    {

        if (timelineName == "frontStab")
        {
            pd.playableAsset = Instantiate(frontStab);

            TimelineAsset timeline = (TimelineAsset)pd.playableAsset;

            foreach (var track in timeline.GetOutputTracks())
            {
                if (track.name == "Attacker Scipt")
                {
                    pd.SetGenericBinding(track, attacker);
                    foreach (var clip in track.GetClips())
                    {
                        MyPlayableClip myclip = (MyPlayableClip)clip.asset;
                        MyPlayableBehaviour mybehav = myclip.template;
                        myclip.am.exposedName = System.Guid.NewGuid().ToString();
                        pd.SetReferenceValue(myclip.am.exposedName, attacker);

                    }
                }
                else if (track.name == "Victim Scipt")
                {
                    pd.SetGenericBinding(track, victim);
                    foreach (var clip in track.GetClips())
                    {
                        MyPlayableClip myclip = (MyPlayableClip)clip.asset;
                        MyPlayableBehaviour mybehav = myclip.template;
                        myclip.am.exposedName = System.Guid.NewGuid().ToString();
                        pd.SetReferenceValue(myclip.am.exposedName, victim);

                    }
                }
                else if (track.name == "Attacker Animator")
                {
                    pd.SetGenericBinding(track, attacker.ac.anim);
                }
                else if (track.name == "Victim Animator")
                {
                    pd.SetGenericBinding(track, victim.ac.anim);
                }
            }
            pd.Evaluate();
            pd.Play();
        }

        else if (timelineName == "openBox")
        {
            pd.playableAsset = Instantiate(openBox);

            TimelineAsset timeline = (TimelineAsset)pd.playableAsset;

            foreach (var track in timeline.GetOutputTracks())
            {
                if (track.name == "Player Scipt")
                {
                    pd.SetGenericBinding(track, attacker);
                    foreach (var clip in track.GetClips())
                    {
                        MyPlayableClip myclip = (MyPlayableClip)clip.asset;
                        MyPlayableBehaviour mybehav = myclip.template;
                        myclip.am.exposedName = System.Guid.NewGuid().ToString();
                        pd.SetReferenceValue(myclip.am.exposedName, attacker);

                    }
                }
                else if (track.name == "Box Scipt")
                {
                    pd.SetGenericBinding(track, victim);
                    foreach (var clip in track.GetClips())
                    {
                        MyPlayableClip myclip = (MyPlayableClip)clip.asset;
                        MyPlayableBehaviour mybehav = myclip.template;
                        myclip.am.exposedName = System.Guid.NewGuid().ToString();
                        pd.SetReferenceValue(myclip.am.exposedName, victim);

                    }
                }
                else if (track.name == "Player Animation")
                {
                    pd.SetGenericBinding(track, attacker.ac.anim);
                }
                else if (track.name == "Box Animation")
                {
                    pd.SetGenericBinding(track, victim.ac.anim);
                }
            }
            pd.Evaluate();
            pd.Play();
        }

        else if (timelineName == "leverUp")
        {
            pd.playableAsset = Instantiate(leverUp);

            TimelineAsset timeline = (TimelineAsset)pd.playableAsset;

            foreach (var track in timeline.GetOutputTracks())
            {
                if (track.name == "Player Scipt")
                {
                    pd.SetGenericBinding(track, attacker);
                    foreach (var clip in track.GetClips())
                    {
                        MyPlayableClip myclip = (MyPlayableClip)clip.asset;
                        MyPlayableBehaviour mybehav = myclip.template;
                        myclip.am.exposedName = System.Guid.NewGuid().ToString();
                        pd.SetReferenceValue(myclip.am.exposedName, attacker);

                    }
                }
                else if (track.name == "Lever Scipt")
                {
                    pd.SetGenericBinding(track, victim);
                    foreach (var clip in track.GetClips())
                    {
                        MyPlayableClip myclip = (MyPlayableClip)clip.asset;
                        MyPlayableBehaviour mybehav = myclip.template;
                        myclip.am.exposedName = System.Guid.NewGuid().ToString();
                        pd.SetReferenceValue(myclip.am.exposedName, victim);

                    }
                }
                else if (track.name == "Player Animation")
                {
                    pd.SetGenericBinding(track, attacker.ac.anim);
                }
                else if (track.name == "Lever Animation")
                {
                    pd.SetGenericBinding(track, victim.ac.anim);
                }
            }
            pd.Evaluate();
            pd.Play();
        }
    }

}
