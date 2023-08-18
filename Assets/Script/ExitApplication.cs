using UnityEngine;

public class ExitApplication : MonoBehaviour
{
    private void Start()
    {
        Cursor.visible = false;
    }

#if UNITY_STANDALONE
    private void OnGUI()
    {
        if (Input.GetKey(KeyCode.Escape))
        { Application.Quit(); }
    }
#endif
}