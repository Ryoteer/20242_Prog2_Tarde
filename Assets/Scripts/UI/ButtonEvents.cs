using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonEvents : MonoBehaviour
{
    public void LoadSceneAsync(string scene)
    {
        SceneLoadManager.Instance.LoadSecenAsync(scene);
    }

    public void CloseApp()
    {
        Application.Quit();
    }
}
