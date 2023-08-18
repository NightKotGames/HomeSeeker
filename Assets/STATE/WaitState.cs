using System;
using UnityEngine;

[CreateAssetMenu(menuName = "State/WaitState", fileName = "WaitState")]

public class WaitState : State
{

    public static event Action<Needs.NeedsCollection, float> Wait;
    public static event Action<Animations.PoliceMan, bool> SetAnim = delegate { };

    [Header("State Options")]
    [SerializeField] private float _waitingTime;
    [SerializeField] private Needs.NeedsCollection _thisNeed;

    [Header("Anim Options")]
    [SerializeField] private Animations.PoliceMan _anim;

    private NPC _nPC;

    private void OnEnable()
    {
        NPC.NeedsRestore += RestoreNeeds;
    }
    private void OnDisable()
    {
        NPC.NeedsRestore -= RestoreNeeds;
    }

    public override void Init()
    {
        if (_nPC == null) { _nPC = FindObjectOfType<NPC>(); }
        SetAnim.Invoke(_anim, false);
    }

    public override void Run()
    {
        if (IsFinished) return;
        if (_nPC.Fatique > _nPC.MaxFatique)
        {
            Wait(Needs.NeedsCollection.fatique, _waitingTime);
        }

    }

    private void RestoreNeeds(Needs.NeedsCollection needs)
    {
        if (needs == _thisNeed)
        {
            IsFinished = true;
        }
    }

}
