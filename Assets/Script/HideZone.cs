using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody2D))]

public class HideZone : MonoBehaviour
{
    public static Action<bool, GameObject> ChageHideState = delegate { };

    private void OnTriggerEnter2D(Collider2D _otherCollider)
    {
        if (_otherCollider.gameObject.TryGetComponent(out CharacterController _character))
        {
            ChageHideState(true, _otherCollider.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D _otherCollider)
    {
        if (_otherCollider.gameObject.TryGetComponent(out CharacterController _character))
        {
            ChageHideState(false, _otherCollider.gameObject);
        }
    }
}