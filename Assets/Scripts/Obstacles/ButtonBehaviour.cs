using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBehaviour : MonoBehaviour
{
    public void OnPress(string name)
    {
        Debug.Log($"<color=orange>{name}</color> apretó el botonsito.");
    }
}
