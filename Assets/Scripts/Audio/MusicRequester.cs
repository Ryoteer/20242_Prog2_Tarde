using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicRequester : MonoBehaviour
{
    [Header("<color=orange>Audio</color>")]
    [SerializeField] private AudioClip _clipToPlay;

    private void Start()
    {
        if (AudioManager.Instance)
        {
            AudioManager.Instance.PlayAudio(_clipToPlay);
        }
    }
}
