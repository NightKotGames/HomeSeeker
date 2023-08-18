using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AndroidJoystik : MonoBehaviour
{
#if UNITY_ANDROID && (!UNITY_STANDARTALONE || !UNITY_EDITOR)

    [Header("Options: ")]
    [SerializeField, Range(1f, 10f)] private float _handleOffsetRadiusMultiplier = 3.0f;
    [Space(10)]
    [SerializeField] private JoystickUIActivityComponentType _handleStickType;
    [SerializeField] private JoystickUIActivityComponentType _moveStickWorkFieldType;

    [Header("Need Components: ")]       
    [SerializeField] private JoystickUIActivityComponent _joystickHandle;
    [SerializeField] private JoystickUIActivityComponent _joystickWorkField;

    [Header("Indicator: ")]
    [Space(20)]
    [SerializeField] private Vector2 _handleVector;

    public static event Action<Vector2> HandleVector = delegate { };
    
    private float _handleOffsetRadius;
    
    private Image _moveHandleStick;
    private Image _workfieldJoystick;

    private void Start()
    {
        _moveHandleStick = _joystickHandle.Init(_handleStickType);
        _workfieldJoystick = _joystickWorkField.Init(_moveStickWorkFieldType);
        _handleOffsetRadius = _workfieldJoystick.GetComponent<RectTransform>().sizeDelta.magnitude / _handleOffsetRadiusMultiplier;
    }

    public void PointerUp()
    {
        _handleVector = Vector2.zero;
        _moveHandleStick.rectTransform.anchoredPosition = _handleVector;  // change here
    }

    public void PointerDown(BaseEventData _eventData)
    {
        UpdateInput(_eventData as PointerEventData);
    }
    public void Drag(BaseEventData _eventData)
    {
        UpdateInput(_eventData as PointerEventData);
    }

    private void UpdateInput(PointerEventData _eventData)
    {
        Vector2 _newPosition;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_workfieldJoystick.GetComponent<RectTransform>(),
            _eventData.position, _eventData.pressEventCamera, out _newPosition))
        {
            _handleVector = _newPosition;

            float _handleOffset = _handleVector.magnitude;

            if (_handleOffset > _handleOffsetRadius)
            {
                _handleVector *= _handleOffsetRadius / _handleOffset;
                
            }

            _moveHandleStick.rectTransform.anchoredPosition = _handleVector;
        }
    }

    private void Update()
    {
        HandleVector.Invoke(_handleVector);
    }

#endif
}