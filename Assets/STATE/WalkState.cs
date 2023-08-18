using System;
using UnityEngine;

[CreateAssetMenu(menuName = "State/WalkState", fileName = "WalkState")]

public class WalkState : State
{

    public static event Action<float> RestorePos = delegate { };

    [SerializeField] private float _buffer;
    [SerializeField] private Vector3 _finishPos;

    private Camera _camera;
    private float _camWidth;

    private void Awake()
    {
        _camera = FindObjectOfType<Camera>();
    }

    public override void Init()
    {
        if (DogIsFinished) return;
        _camWidth = _camera.orthographicSize * _camera.aspect;
        _finishPos = new Vector3(-_camWidth - _buffer, DogNPC.transform.position.y, DogNPC.transform.position.z);

    }

    public override void Run()
    {

        if (IsFinished) return;
        var distance = (_finishPos - DogNPC.transform.position).magnitude;
        if (distance > .1f)
        {

            DogNPC.MoveTo(_finishPos);
        }
        else
        {
            RestorePos(_camWidth + _buffer);
            IsFinished = true;
        }
    }
}
