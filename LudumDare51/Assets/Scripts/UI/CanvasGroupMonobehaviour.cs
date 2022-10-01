using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasGroupMonobehaviour : MonoBehaviour
{
    [SerializeField]
    protected CanvasGroup masterCanvasGroup;

    protected float fadeSpeed = 1.0f;
    protected float desiredAlpha = 0;
    protected float currentAlpha = 0;

    public virtual void Start()
    {
        desiredAlpha = currentAlpha;
    }

    public virtual void Update()
    {
        if (masterCanvasGroup != null && masterCanvasGroup.alpha != desiredAlpha)
        {
            masterCanvasGroup.alpha = Mathf.MoveTowards(masterCanvasGroup.alpha, desiredAlpha, fadeSpeed * Time.deltaTime);
        }
    }

    public void FadeIn()
    {
        desiredAlpha = 1;
    }
    public void FadeOut()
    {
        desiredAlpha = 0;
    }
}
