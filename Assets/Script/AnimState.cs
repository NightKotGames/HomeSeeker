using UnityEngine;
using Spine.Unity;

public class AnimState : StateMachineBehaviour
{    
    [Header("Animation State Parameters")]
    [SerializeField] private string _animationName;
    [SerializeField] private string _skinName;
    [SerializeField] private float _speedAnimation;
     
    public override void OnStateEnter(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
    {
        SkeletonAnimation _animation = _animator.GetComponent<SkeletonAnimation>();
        _animation.initialSkinName = _skinName;
        _animation.Initialize(true);
        _animation.state.SetAnimation(0, _animationName, true).TimeScale = _speedAnimation;
    }
}