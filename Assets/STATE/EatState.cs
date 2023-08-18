using System;
using UnityEngine;

[CreateAssetMenu(menuName = "State/EatState", fileName = "EatState")]

public class EatState : State
{

    public static event Action<Needs.NeedsCollection, float> Eating = delegate { };
    public static event Action<Animations.PoliceMan, bool> SetAnim = delegate { };

    [Header("StateOptions")]
    [SerializeField] private GameObject _foodPlace;
    [SerializeField] private Needs.NeedsCollection _thisNeed;
    [SerializeField] private float _eatTime;
    [SerializeField] private float _saturationThreshold;
    [SerializeField] private float _distance;

    [Header("Anim Options")]
    [SerializeField] private Animations.PoliceMan _walkToTargetAnim;
    [SerializeField] private Animations.PoliceMan _eatingAnim;

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
        SetAnim.Invoke(_walkToTargetAnim, SetRotate());

    }

    public override void Run()
    {
        if (IsFinished) return;
        MoveToTarget();

    }

    private void MoveToTarget()
    {

        var distance = (_foodPlace.transform.position - Character.transform.position).magnitude;

        if (distance > _distance)
        {
            Character.MoveTo(_foodPlace.transform.position);

        }
        else
        {
            if (_nPC.Hungry > _nPC.MaxHungry)
            {
                SetAnim.Invoke(_eatingAnim, false);
                Eating(Needs.NeedsCollection.hungry, 5f);
            }


        }
    }


    private bool SetRotate()
    {
        bool rotate = false;

        Vector3 targetPos = Character.Cam.WorldToViewportPoint(_foodPlace.transform.position);
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
            IsFinished = true;
        }
    }
}
