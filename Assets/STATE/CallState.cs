using System;
using UnityEngine;

[CreateAssetMenu(menuName = "State/CallState", fileName = "CallState")]

public class CallState : State
{
    public static event Action<Animations.PoliceMan, bool> SetAnim = delegate { };
    public static event Action<Needs.NeedsCollection, float> Calling = delegate { };
    public static event Action<bool> Phone = delegate { };

    [Header("StateOptions")]
    [SerializeField] private GameObject _phoneCabin;
    [SerializeField] private Needs.NeedsCollection _thisNeed;
    [SerializeField] private float _callTime;
    [SerializeField] private float _distance;

    [Header("Anim Options")]
    [SerializeField] private Animations.PoliceMan _moveToTargetAnim;
    [SerializeField] private Animations.PoliceMan _triggerAnim;

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
        _nPC = FindObjectOfType<NPC>();
        SetAnim.Invoke(_moveToTargetAnim, SetRotate());
    }

    public override void Run()
    {
        if (IsFinished)
        {

            return;
        }
        MoveToTarget();
    }

    private void MoveToTarget()
    {
        var distance = (_phoneCabin.transform.position - Character.transform.position).magnitude;
        //Debug.Log($"distance: {distance}");
        if (distance > _distance)
        {
            Character.MoveTo(_phoneCabin.transform.position);
        }
        else
        {
            if (_nPC.Boredom > _nPC.MaxBoredom)
            {
                Calling.Invoke(Needs.NeedsCollection.boredom, _callTime);
                Phone.Invoke(true);
                SetAnim.Invoke(_triggerAnim, false);
            }
        }
    }

    private bool SetRotate()
    {
        bool rotate = false;

        Vector3 targetPos = Character.Cam.WorldToViewportPoint(_phoneCabin.transform.position);
        Vector3 CharacterPos = Character.Cam.WorldToViewportPoint(Character.transform.position);

        if (targetPos.x < CharacterPos.x)
        {
            return rotate;
        }
        rotate = true;
        return rotate;
    }

    private void RestoreNeeds(Needs.NeedsCollection needs)
    {
        if (needs == _thisNeed)
        {
            Phone.Invoke(false);
            IsFinished = true;
        }
    }
}