using Spine.Unity;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(SkeletonAnimation))]
[RequireComponent(typeof(BoxCollider2D))]

public class CharacterController : MonoBehaviour
{
    [Header("Options: ")] 
    [SerializeField] private float _speed;
    
    [Header("Need Components: ")]
    [SerializeField] private CharacterController _character;

    private Animator _animator;
    private SkeletonAnimation _anim;
    private Vector2 direction;

    private const string Walk = "Walk";
    private const string Hit = "Hit";

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _anim = _animator.GetComponent<SkeletonAnimation>();

        // Инициализация анимации Idle при старте
        _anim.Initialize(true);
    }

    private void OnEnable()
    {
#if UNITY_ANDROID && (!UNITY_STANDALONE || !UNITY_EDITOR)
        AndroidJoystik.HandleVector += AndroidUpdate;
#endif

        AttackState.TakeHit += TakingDamage;
    }

    private void OnDisable()
    {
#if UNITY_ANDROID && (!UNITY_STANDALONE || !UNITY_EDITOR)
        AndroidJoystik.HandleVector -= AndroidUpdate;
#endif

        AttackState.TakeHit -= TakingDamage;
    }

    private void Update()
    {
        // Флип спрайта на основе initialFlipX
        _anim.skeleton.ScaleX = _anim.initialFlipX ? -1 : 1;

#if (UNITY_STANDALONE || UNITY_EDITOR) && !UNITY_ANDROID
        if (Input.GetKeyDown(KeyCode.None))
        {
            Stay();
        }

        if (Input.anyKey)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                MoveLeft();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                MoveRight();
            }
            _character.transform.Translate(direction.normalized * _speed);
        }
        else
        {
            Stay();
        }
#endif
    }

    private void AndroidUpdate(Vector2 _handleJoystickDelta)
    {
        float _handleOffset = _handleJoystickDelta.x;

        if (_handleOffset == 0)
        {
            Stay();
        }
        else
        {
            if (_handleOffset < 0)
            {
                MoveLeft();
            }
            else
            {
                MoveRight();
            }
            _character.transform.Translate(direction.normalized * _speed);
        }
    }

    private void Stay()
    {
        _animator.SetBool(Walk, false);
        direction.x = 0;
    }

    private void MoveLeft()
    {
        _anim.initialFlipX = false;
        _animator.SetBool(Walk, true);
        direction.x = -1f;
    }

    private void MoveRight()
    {
        _anim.initialFlipX = true;
        _animator.SetBool(Walk, true);
        direction.x = 1f;
    }

    private void TakingDamage()
    {
        _animator.SetTrigger(Hit);
    }
}