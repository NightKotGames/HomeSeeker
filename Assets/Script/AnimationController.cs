using Spine.Unity;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(SkeletonAnimation))]

public class AnimationController : MonoBehaviour
{
    [SerializeField] private string _lostAnim;
    [SerializeField] private bool _lostRotate;

    private Animator _animator;
    private SkeletonAnimation _anim;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _anim = _animator.GetComponent<SkeletonAnimation>();
    }

    private void OnEnable()
    {
        PatrolState.SetAnim += SetAnimation;
        EatState.SetAnim += SetAnimation;
        AttackState.SetAnim += SetAnimation;
        WaitState.SetAnim += SetAnimation;
        CallState.SetAnim += SetAnimation;
    }
    private void OnDisable()
    {
        PatrolState.SetAnim -= SetAnimation;
        EatState.SetAnim -= SetAnimation;
        AttackState.SetAnim -= SetAnimation;
        WaitState.SetAnim -= SetAnimation;
        CallState.SetAnim -= SetAnimation;
    }

    private void SetAnimation(Animations.PoliceMan _policeManState, bool _rotate)
    {
        if (($"{_policeManState}" == _lostAnim) && (_rotate == _lostRotate)) 
        { 
            return; 
        }

        _animator.SetBool(_lostAnim, false);
        _animator.SetBool($"{_policeManState}", true);
        _anim.AnimationName = $"{_policeManState}";
        _anim.initialFlipX = _rotate;
        _anim.loop = true;
        _anim.Initialize(true);
        _lostAnim = $"{_policeManState}";
        _lostRotate = _rotate;
    } 
}