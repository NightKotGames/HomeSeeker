using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody2D))]

public class AlarmZoneDetector : MonoBehaviour
{
    public static event Action<bool, GameObject> AlarmTriggered = delegate { };

    private void OnTriggerEnter2D(Collider2D _collider)
    {
        if (_collider.gameObject.TryGetComponent(out CharacterController objectStatus))
        {
            AlarmTriggered(true, _collider.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D _collider)
    {
        if (_collider.gameObject.TryGetComponent(out CharacterController objectStatus))
        {
            AlarmTriggered(false, _collider.gameObject);
        }
    }
}