using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    private Player _player;
    public Player Player 
    { 
        get { return _player; }
        set { _player = value; }
    }

    private List<Enemy> _enemies = new();
    public List<Enemy> Enemies 
    { 
        get { return _enemies; }
        set { _enemies = value; }
    }

    private NavMeshSurface _surface;
    public NavMeshSurface Surface 
    { 
        get { return _surface; } 
        set { _surface = value; }
    }
}
