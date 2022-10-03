using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SkipClip : MonoBehaviour
{
    public UnityEvent OnSkipped;

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Escape) || Input.GetMouseButtonUp(0))
        {
            OnSkipped.Invoke();
        }
    }
}
