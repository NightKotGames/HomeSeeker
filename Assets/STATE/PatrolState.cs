using System;
using UnityEngine;

[CreateAssetMenu(menuName = "State/PatrolState", fileName = "PatrolState")]

public class PatrolState : State
{

    public static event Action<Animations.PoliceMan, bool> SetAnim = delegate { };

    [Header("State Options")]
    [SerializeField] private float _maxDistance = 5f;

    [Header("Anim Options")]
    [SerializeField] private Animations.PoliceMan _anim;

    private Vector3 _randomPos;

    public override void Init()
    {
        var rand = new Vector3(UnityEngine.Random.Range(-_maxDistance, _maxDistance), 0, 0f);
        _randomPos = Character.StartPos + rand;
        SetAnim.Invoke(_anim, SetRotate());
    }
    public override void Run()
    {
        if (IsFinished) return;

        var distance = (_randomPos - Character.transform.position).magnitude;
        if (distance > .5f)
        {
            Character.MoveTo(_randomPos);
        }
        else
        {
            IsFinished = true;
        }

    }

    private bool SetRotate()
    {
        bool rotate = false;

        Vector3 targetPos = Character.Cam.WorldToViewportPoint(_randomPos);
        Vector3 CharacterPos = Character.Cam.WorldToViewportPoint(Character.transform.position);

        if (targetPos.x < CharacterPos.x)
        {
            return rotate;
        }

        rotate = true;
        return rotate;
    }
}