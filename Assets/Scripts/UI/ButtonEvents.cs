using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonEvents : MonoBehaviour
{
    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void LoadSceneAsync(string scene)
    {
        SceneLoadManager.Instance.LoadSecenAsync(scene);
    }

    public void CloseApp()
    {
        Application.Quit();
    }
}
