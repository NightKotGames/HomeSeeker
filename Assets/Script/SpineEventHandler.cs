using System;
using Spine;
using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SkeletonAnimation))]

public class SpineEventHandler : MonoBehaviour
{
    public static event Action<string> AnimEvent;
    
    [SerializeField] private List<string> _spineEvent;
    private SkeletonAnimation _animation;

    private void Awake()
    {
        _animation = GetComponent<SkeletonAnimation>();
    }

    private void OnEnable()
    {
        _animation.state.Event += AnSpineEventHappened;
    }
    private void OnDisable()
    {
        _animation.state.Event -= AnSpineEventHappened;
    }

    private void AnSpineEventHappened(TrackEntry trackEntry, Spine.Event e)
    {
        Debug.Log($"SpineEvent {e}");
       foreach(string nameEvent in _spineEvent)
       {
            if (e.Data.Name == nameEvent)
            {
                AnimEvent.Invoke(nameEvent);
            }
       }
    }
}