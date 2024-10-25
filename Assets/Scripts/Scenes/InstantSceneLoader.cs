using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InstantSceneLoader : MonoBehaviour
{
    [Header("Scene Managment")]
    [SerializeField] private string _firstScene;

    private void Awake()
    {
        SceneManager.LoadSceneAsync(_firstScene, LoadSceneMode.Additive);

        Destroy(gameObject, 1f);
    }
}
