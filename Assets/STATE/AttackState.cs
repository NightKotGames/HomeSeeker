using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "State/AttackState", fileName = "AttackState")]

public class AttackState : State
{
    public static event Action<Animations.PoliceMan, bool> SetAnim = delegate { };
    public static event Action TakeHit = delegate { };

    [Header("StateOptions")]
    [SerializeField] private float _distance;
    [SerializeField] private Transform _targetPos;
    [SerializeField] private GameObject _enemy;
    [SerializeField] private bool _alarm;
    [SerializeField] private float _policemanHitAnimLenth;
    [SerializeField] private float _timeHitAnimLength;

    [Header("Anim Options")]

    [SerializeField] private Animations.PoliceMan _moveAnim;
    [SerializeField] private Animations.PoliceMan _hitAnim;

    private MonoBehaviour _monoBehaviour;


    private void Awake()
    {
        _monoBehaviour = FindObjectOfType<MonoBehaviour>();
    }

    public override void Init()
    {
        if (IsFinished)
            return;
        if (_enemy != null)
        {
            _targetPos = _enemy.transform;
            SetAnim.Invoke(_moveAnim, SetRotate());
        }
    }

    public override void Run()
    {
        if (IsFinished) return;
        MoveToTarget();
    }

    private void OnEnable()
    {
        HideZone.ChageHideState += Hide;
        AlarmZoneDetector.AlarmTriggered += Alarm;
    }

    private void OnDisable()
    {
        HideZone.ChageHideState -= Hide;
        AlarmZoneDetector.AlarmTriggered -= Alarm;
    }

    private void Alarm(bool _alarm, GameObject _enemy)
    {
        if (_alarm == true)
        {
            this._enemy = _enemy;
            this._alarm = _alarm;
            IsFinished = true;
            Init();
        }
    }

    private void Hide(bool _hideState, GameObject _characterObject)
    {
        _enemy = null;
        IsFinished = true;
        Init();
    }

    private void MoveToTarget()
    {
        try
        {
            var _distance = (_targetPos.position - Character.transform.position).magnitude;
            if (_enemy != null && _distance > this._distance)
            {
                Character.MoveTo(_targetPos.position);
            }
            else if (_enemy != null && _distance < this._distance)
            {
                SetAnim.Invoke(_hitAnim, SetRotate());

                SeetPauseforAnim(_policemanHitAnimLenth);
                void SeetPauseforAnim(float _time)
                {
                    _monoBehaviour.StartCoroutine(Wait());
                    IEnumerator Wait()
                    {
                        yield return new WaitForSeconds(_time);
                        TakeHit.Invoke();
                    }
                    _monoBehaviour.StopCoroutine(Wait());
                }
            }
        }
        catch (Exception _err)
        {
            Debug.Log($"{_err}");
        }
    }

    private bool SetRotate()
    {
        bool _rotate = false;

        Vector3 _targetPos = Character.Cam.WorldToViewportPoint(this._targetPos.position);
        Vector3 _characterPos = Character.Cam.WorldToViewportPoint(Character.transform.position);

        if (_targetPos.x < _characterPos.x)
        {
            return _rotate;
        }
        _rotate = true;
        return _rotate;
    }
}
