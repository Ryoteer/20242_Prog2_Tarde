using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class SurfaceReference : MonoBehaviour
{
    private NavMeshSurface _surface;

    private void Start()
    {
        _surface = GetComponent<NavMeshSurface>();
        _surface.BuildNavMesh();
        GameManager.Instance.Surface = _surface;
    }
}
