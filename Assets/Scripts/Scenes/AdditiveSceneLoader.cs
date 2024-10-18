using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdditiveSceneLoader : MonoBehaviour
{
    [Header("<color=gray>Scene Managment</color>")]
    [SerializeField] private string _sceneToLoad = "CaveScene";
    [SerializeField] private Animation _divisorAnim;
    
    private bool _isSceneLoaded;
    private Collider _trigger;

    private void Start()
    {
        _trigger = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            if (!_isSceneLoaded)
            {
                StartCoroutine(LoadSceneAdd(_sceneToLoad));
            }
            else
            {
                StartCoroutine(UnloadScene(_sceneToLoad));
            }
        }
    }

    private void FadeDivisor(AsyncOperation asyncOp)
    {
        _divisorAnim.clip = _divisorAnim.GetClip("Fade");
        _divisorAnim.Play();
    }

    private void SolidifyDivisor()
    {
        _divisorAnim.clip = _divisorAnim.GetClip("Solidify");
        _divisorAnim.Play();
    }

    private IEnumerator LoadSceneAdd(string sceneToLoad)
    {
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);

        asyncOp.completed += FadeDivisor;

        while(asyncOp.progress < 0.9f)
        {
            Debug.Log($"Loading {sceneToLoad}...");

            yield return null;
        }

        _isSceneLoaded = true;
    }

    private IEnumerator UnloadScene(string sceneToUnload)
    {
        SolidifyDivisor();

        AsyncOperation asyncOp = SceneManager.UnloadSceneAsync(sceneToUnload);

        while(asyncOp.progress < 0.9f)
        {
            yield return null;
        }

        _isSceneLoaded = false;
    }
}
