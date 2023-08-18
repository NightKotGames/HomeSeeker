using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]

public class Fog : MonoBehaviour
{
    [SerializeField] private float _speed;
    private RawImage _image;
    private float posx = 0.07f;

    private void Start()
    {
        _image = GetComponent<RawImage>();
    }

    private void OnGUI()
    {
        posx -= (Time.deltaTime + _speed) / 200;
        _image.uvRect = new Rect(posx, 0, _image.uvRect.width, _image.uvRect.height);
    }
}