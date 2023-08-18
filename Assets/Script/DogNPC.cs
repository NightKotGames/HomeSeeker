using Spine.Unity;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator), typeof(SkeletonAnimation))]

public class DogNPC : MonoBehaviour
{
    [Header("State kit: ")]
    [SerializeField] private State _startState;
    [SerializeField] private State _attackState;
    [SerializeField] private State _eatState;
    [SerializeField] private State _pissState;
    [SerializeField] private State _walkState;

    [Header("Actual State: ")]
    [SerializeField] private State _currentState;

    [Header("NPC settings: ")]
    [SerializeField] private float _speed;

    [HideInInspector] public Vector3 StartPos;


    private void Start()
    {
        StartPos = transform.position;
        SetState(_startState);
    }

    private void OnEnable()
    {
        WalkState.RestorePos += RestorePos;
    }
    private void OnDisable()
    {
        WalkState.RestorePos -= RestorePos;
    }

    public void SetState(State state)
    {
        _currentState = Instantiate(state);
        _currentState.DogNPC = this;
        _currentState.Init();
    }

    private void Update()
    {
        if (!_currentState.IsFinished)
        {
            _currentState.Run();
        }
    }

    public void MoveTo(Vector3 position)
    {
        position.z = transform.position.z;
        position.y = transform.position.y;
        transform.position = Vector3.MoveTowards(transform.position, position, Time.deltaTime * _speed);
    }

    private void RestorePos(float posX)
    {
        transform.position = new Vector3(posX, transform.position.y, transform.position.z);
        SetState(_startState);
    }
}