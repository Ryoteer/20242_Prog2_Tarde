using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class FadingPlatform : MonoBehaviour
{
    [Header("<color=#FF4500>Behaviour</color>")]
    [SerializeField] private float _fadeTime = 3.0f;
    [SerializeField] private float _interval = 5.0f;
    [SerializeField] private float _spawnTime = 3.0f;

    private bool _isActive = false;

    private Collider _col;
    private Material _mat;
    private Color _ogColor;
    private NavMeshModifier _mod;

    private void Start()
    {
        _col = GetComponent<Collider>();
        _mat = GetComponent<Renderer>().material;
        _ogColor = _mat.color;
        _mod = GetComponent<NavMeshModifier>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Player>() && !_isActive)
        {
            StartCoroutine(FadeBehaviour());
        }
    }

    private IEnumerator FadeBehaviour()
    {
        _isActive = true;

        float t = 0.0f;

        while(t < 1.0f)
        {
            t += Time.deltaTime / _fadeTime;
            _mat.color = new Color(_ogColor.r, _ogColor.g, _ogColor.b, Mathf.Lerp(1.0f, 0.0f, t));
            yield return null;
        }

        _mat.color = new Color(_ogColor.r, _ogColor.g, _ogColor.b, 0.0f);
        _col.enabled = false;
        _mod.enabled = false;

        GameManager.Instance.Surface.BuildNavMesh();

        yield return new WaitForSeconds(_interval);

        t = 0.0f;

        while (t < 1.0f)
        {
            t += Time.deltaTime / _spawnTime;
            _mat.color = new Color(_ogColor.r, _ogColor.g, _ogColor.b, Mathf.Lerp(0.0f, 1.0f, t));
            yield return null;
        }

        _mat.color = new Color(_ogColor.r, _ogColor.g, _ogColor.b, 1.0f);
        _col.enabled = true;
        _mod.enabled= true;

        GameManager.Instance.Surface.BuildNavMesh();

        _isActive = false;
    }
}
