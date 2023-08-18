using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]

public class JoystickUIActivityComponent : MonoBehaviour
{
    [Header("Options: ")]
    [SerializeField] private JoystickUIActivityComponentType _componentType;

    public Image Init(JoystickUIActivityComponentType _componentType)
    {
        if (_componentType != this._componentType) { return null; }
        Image _uiComponent = GetComponent<Image>();
        return _uiComponent;
    }
}