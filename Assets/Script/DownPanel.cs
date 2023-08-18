using UnityEngine;

public class DownPanel : MonoBehaviour
{
    [SerializeField] private GameObject _pcPanel;
    [SerializeField] private GameObject _androidPanel;

    private void OnEnable()
    {
#if (UNITY_STANDARTALONE || UNITY_EDITOR) && !UNITY_ANDROID

        _pcPanel.SetActive(true);
        _androidPanel.SetActive(false);

#elif UNITY_ANDROID && (!UNITY_STANDARTALONE || !UNITY_EDITOR)

        _pcPanel.SetActive(false);
        _androidPanel.SetActive(true);
#endif
    }
}